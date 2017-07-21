using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UklonBot.Helpers
{
    public static class OrderHelper
    {

        public static async void SetUserPickUpStreet(Activity activity, string languageCode)
        {
            try
            {
                StateClient stateClient = activity.GetStateClient();
                BotData userData = stateClient.BotState.GetUserData(activity.ChannelId, activity.From.Id);

                userData.SetProperty<string>("PickUpStreet", languageCode);
                await stateClient.BotState.SetUserDataAsync(activity.ChannelId, activity.From.Id, userData);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public static void SetUserPickUpStreet(IDialogContext context, string languageCode)
        {
            try
            {
                context.UserData.SetValue("PickUpStreet", languageCode);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static string GetUserPickUpStreet(Activity activity)
        {
            try
            {
                StateClient stateClient = activity.GetStateClient();
                BotData userData = stateClient.BotState.GetUserData(activity.ChannelId, activity.From.Id);

                var languageCode = userData.GetProperty<string>("PickUpStreet");

                return languageCode;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static string GetUserPickUpStreet(IDialogContext context)
        {
            try
            {
                string result;
                context.UserData.TryGetValue("PickUpStreet", out result);

                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
    }
}