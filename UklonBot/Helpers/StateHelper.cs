using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using UklonBot.Models;

namespace UklonBot.Helpers
{
    public static class StateHelper
    {

        public static async void SetUserLanguageCode(Activity activity, string languageCode)
        {
            try
            {
                StateClient stateClient = activity.GetStateClient();
                BotData userData = stateClient.BotState.GetUserData(activity.ChannelId, activity.From.Id);

                userData.SetProperty<string>("LanguageCode", languageCode);
                await stateClient.BotState.SetUserDataAsync(activity.ChannelId, activity.From.Id, userData);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static void SetUserLanguageCode(IDialogContext context, string languageCode)
        {
            try
            {
                context.UserData.SetValue("LanguageCode", languageCode);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static string GetUserLanguageCode(Activity activity)
        {
            try
            {
                StateClient stateClient = activity.GetStateClient();
                BotData userData = stateClient.BotState.GetUserData(activity.ChannelId, activity.From.Id);

                var languageCode = userData.GetProperty<string>("LanguageCode");

                return languageCode;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static string GetUserLanguageCode(IDialogContext context)
        {
            try
            {
                string result;
                context.UserData.TryGetValue("LanguageCode", out result);

                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        public static async void SetCurrentUserData(Activity activity, ChannelUser channelUser)
        {
            try
            {
                StateClient stateClient = activity.GetStateClient();
                BotData userData = stateClient.BotState.GetUserData(activity.ChannelId, activity.From.Id);

                userData.SetProperty<ChannelUser>("CurrentUser", channelUser);
                await stateClient.BotState.SetUserDataAsync(activity.ChannelId, activity.From.Id, userData);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static void SetCurrentUserData(IDialogContext context, ChannelUser channelUser)
        {
            try
            {
                context.UserData.SetValue("CurrentUser", channelUser);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static ChannelUser GetCurrentUser(Activity activity)
        {
            try
            {
                StateClient stateClient = activity.GetStateClient();
                BotData userData = stateClient.BotState.GetUserData(activity.ChannelId, activity.From.Id);

                var currentUser = userData.GetProperty<ChannelUser>("CurrentUser");

                return currentUser;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static ChannelUser GetCurrentUserData(IDialogContext context)
        {
            try
            {
                ChannelUser result;
                context.UserData.TryGetValue("CurrentUser", out result);

                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


    }
}