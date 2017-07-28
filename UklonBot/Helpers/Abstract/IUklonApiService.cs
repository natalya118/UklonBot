using System.Collections.Generic;
using Microsoft.Bot.Builder.Dialogs;
using UklonBot.Models.BotSide.OrderTaxi;
using UklonBot.Models.UklonSide;

namespace UklonBot.Helpers.Abstract
{
    public interface IUklonApiService
    {
        string CreateOrder(TaxiLocations locations, string providerId, IDialogContext context);
        double CalculateAmmount(Location fromLocation, Location toLocation, IDialogContext context);
        IEnumerable<string> GetPlaces(string query, IDialogContext context);
        OrderInfo GetOrderState(string dialogOrderId, IDialogContext context);
        void CancelOrder(string currentDialogOrderId, IDialogContext context);
        string RecreateOrder(string orderId, IDialogContext context);
        string GetRecreatedOrderId(string orderId, IDialogContext context);
        Location GetPlaceLocation(string currentDialogPickupAddress, string currentDialogPickupHouse, IDialogContext context);
        bool Authenticate(string phoneNumber, string viberId, IDialogContext context);
        bool Register(string phoneNumber, string provider, string providerId, string phoneValidationCode, IDialogContext context);
        void ConfirmPhone(string phoneNumber);
    }
}
