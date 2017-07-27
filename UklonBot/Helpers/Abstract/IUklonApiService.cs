﻿using System.Collections.Generic;
using UklonBot.Models.BotSide.OrderTaxi;
using UklonBot.Models.UklonSide;

namespace UklonBot.Helpers.Abstract
{
    public interface IUklonApiService
    {
        string CreateOrder(TaxiLocations locations, string providerId);
        double CalculateAmmount(Location fromLocation, Location toLocation);
        IEnumerable<string> GetPlaces(string query);
        OrderInfo GetOrderState(string dialogOrderId);
        void CancelOrder(string currentDialogOrderId);
        string RecreateOrder(string orderId);
        string GetRecreatedOrderId(string orderId);
        Location GetPlaceLocation(string currentDialogPickupAddress, string currentDialogPickupHouse);
        bool Authenticate(string phoneNumber, string viberId);
        bool Register(string phoneNumber, string provider, string providerId, string phoneValidationCode);
        void ConfirmPhone(string phoneNumber);
    }
}
