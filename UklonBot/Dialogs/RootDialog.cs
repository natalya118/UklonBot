using System;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using UklonBot.Helpers;
using Microsoft.Bot.Builder.Luis;
using Newtonsoft.Json;
using UklonBot.Factories;
using UklonBot.Factories.Abstract;
using UklonBot.Helpers.Abstract;
using UklonBot.Models.BotSide;
using UklonBot.Properties;

namespace UklonBot.Dialogs
{
    [LuisModel("ef002f59-e196-4dd8-a208-387c6c38bf3a", "fdac920ffdb9473c85f5f73eb274d076")]
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        private static Helpers.Abstract.ILuisService _luisService;
        private static ITranslatorService _translatorService;
        
        private static IDialogStrategy _dialogStrategy;
        private static IUserService _userService;
        public RootDialog(Helpers.Abstract.ILuisService luisService, ITranslatorService translatorService, IDialogStrategy dialogStrategy, IUserService userService)
        {
            _luisService = luisService;
            _translatorService = translatorService;
            _dialogStrategy = dialogStrategy;
            _userService = userService;
        }

        public Task StartAsync(IDialogContext context)
        {
            CultureInfo culture = CultureInfo.CreateSpecificCulture("ru-RU");

            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;


            StateHelper.SetUserLanguageCode(context, "ru");

            context.Wait(MessageReceivedAsync);
            return Task.CompletedTask;
        }
   

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var activity = await result as Activity;

            if (activity != null)
            {
                StateHelper.SetUserLanguageCode(context, await _translatorService.GetLanguage(activity.Text));

                if (! _userService.IsUserRegistered(context.Activity.From.Id))
                {
                    context.Call(_dialogStrategy.CreateDialog(DialogFactoryType.Root.Register),
                        RegistrationDialogResumeAfter);
                }
                else if(!_userService.IsUserCitySaved(context.Activity.From.Id))
                {
                
                    context.Call(_dialogStrategy.CreateDialog(DialogFactoryType.Root.ChangeCity), DialogResumeAfter);
                }
                else
                {
                
            
                    var luisAnswer = await _luisService.GetResult(activity.Text);
                
                    switch (luisAnswer.topScoringIntent.intent)
                    {
                        case "Order taxi":

                            context.Call(_dialogStrategy.CreateDialog(DialogFactoryType.Root.Order), OrderDialogResumeAfter);
                            break;

                        case "Loss":
                            if (luisAnswer.entities.Length > 0)
                            {
                                var ent = luisAnswer.entities.First().ToString();
                                var res = await _translatorService.TranslateText(
                                    JsonConvert.DeserializeObject<LuisEntity>(ent).entity,
                                    StateHelper.GetUserLanguageCode(context));

                                await context.PostAsync(Resources.loss_thing + await _translatorService.TranslateText(
                                    
                                    res, StateHelper.GetUserLanguageCode(context)));
                            }
                            else { 
                                await context.PostAsync(UklonBot.Properties.Resources.ResourceManager.GetString("loss_sth"));
                            }
                            context.Call(_dialogStrategy.CreateDialog(DialogFactoryType.Root.Loss), DialogResumeAfter);
                            break;

                        case "Change city":
                            context.Call(_dialogStrategy.CreateDialog(DialogFactoryType.Root.ChangeCity), DialogResumeAfter);
                            break;

                        case "Help":
                            context.Call(_dialogStrategy.CreateDialog(DialogFactoryType.Root.Help), DialogResumeAfter);
                            break;

                        case "Greeting":
                            await context.PostAsync(Resources.Greeting);
                            context.Wait(MessageReceivedAsync);
                            break;

                        case "Complaint":
                            context.Call(_dialogStrategy.CreateDialog(DialogFactoryType.Root.Complaint), DialogResumeAfter);
                            break;

                        default:
                            await context.PostAsync(Resources.Not_understand);
                            context.Wait(MessageReceivedAsync);
                            break;
                    }
                }
            }
        }

        private async Task OrderDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            context.Call(_dialogStrategy.CreateDialog(DialogFactoryType.Order.Rank), RankDialogAfter);
        }
        private async Task RankDialogAfter(IDialogContext context, IAwaitable<object> result)
        {
            await context.PostAsync(Resources.thank_you);
            context.Done((Activity)null);
        }

        private async Task RegistrationDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            if ((bool) await result)
            {
                await context.PostAsync(Resources.registration_success);
                context.Wait(MessageReceivedAsync);
            }
            else
            {
                await context.PostAsync(Resources.registration_fail);
                //uncomment this line to turn on token validation 
                //context.Call(_dialogStrategy.CreateDialog(DialogFactoryType.Root.Register), this.RegistrationDialogResumeAfter);
            }

        }
        private async Task DialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            context.Wait(MessageReceivedAsync);
        }

    }
}