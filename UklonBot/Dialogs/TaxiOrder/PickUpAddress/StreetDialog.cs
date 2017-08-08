﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using UklonBot.Helpers;
using UklonBot.Helpers.Abstract;
using UklonBot.Properties;

namespace UklonBot.Dialogs.TaxiOrder.PickUpAddress
{
    [Serializable]
    public class StreetDialog : IDialog<string>
    {
        private static ILuisService _luisService;
        private static IUklonApiService _uklonApiService;
        
        public StreetDialog(ILuisService luisService, IUklonApiService uklonApiService)
        {
            _luisService = luisService;
            _uklonApiService = uklonApiService;
        }
        public async Task StartAsync(IDialogContext context)
        {
            StateHelper.SetUserLanguageCode(context, StateHelper.GetUserLanguageCode(context));
            await context.PostAsync(Resources.street_or_place);
            context.Wait(MessageReceivedAsync);
        }
        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            
            var message = await result;
            
                var luisAnswer = await _luisService.GetResult(message.Text);
                switch (luisAnswer.topScoringIntent.intent)
                {
                    case "Cancel":
                       
                        context.Fail(null);
                    break;
                    default:
                    {
                        StateHelper.SetUserLanguageCode(context, StateHelper.GetUserLanguageCode(context));
                        try
                        {
                            List<string> places = _uklonApiService.GetPlaces(message.Text, context).ToList();
                           
                            if (places.Any())
                            {
                               
                                    if (context.Activity.ChannelId.Equals("telegram"))
                                        PromptDialog.Choice(context,
                                            LocationDialogResumeAfter, places,
                                            Resources.choose_place, Resources.choose_place_from_list, 3,
                                            PromptStyle.Keyboard);
                                    else
                                        PromptDialog.Choice(context,
                                            LocationDialogResumeAfter, places,
                                            Resources.choose_place, Resources.choose_place_from_list);
                                
                                
                            }
                            else
                            {
                                await context.PostAsync(Resources.place_or_street_not_found);
                                context.Wait(MessageReceivedAsync);
                            }
                        }
                        catch (Exception)
                        {
                            StateHelper.SetUserLanguageCode(context, StateHelper.GetUserLanguageCode(context));
                            await context.PostAsync(Resources.place_or_street_not_found);
                            context.Wait(MessageReceivedAsync);
                        }
                        


                        break;
                    }
                }
           

        }

        private async Task LocationDialogResumeAfter(IDialogContext context, IAwaitable<string> result)
        {
            var res = await result;
            if (res != null)
            {
                context.Done(res);
            }
            else
            {
                context.Fail(null);
            }
        }



    }
}