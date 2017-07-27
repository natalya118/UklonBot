﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Configuration;
using Newtonsoft.Json;
using UklonBot.Helpers.Abstract;
using UklonBot.Models;
using UklonBot.Models.BotSide.OrderTaxi;
using UklonBot.Models.Repositories.Abstract;
using UklonBot.Models.UklonSide;

namespace UklonBot.Helpers.Exact
{
    public class UklonApiService : IUklonApiService
    {
        private readonly IUnitOfWork _uow;
        
        public UklonApiService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public string CreateOrder(TaxiLocations locations, string providerId)
        {
            string url = "https://test.uklon.com.ua/api/v1/orders";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers.Add("client_id", WebConfigurationManager.AppSettings["UklonClientId"]);
            request.Headers.Add("Locale", "ru");
            request.Headers.Add("City", "kiev");

            request.ContentType = "application/json";
            request.Method = "POST";

            Location startPoint, endPoint;

            //try
            //{
            //    startPoint = locations.;
            //    endPoint = GetPlaceLocation(locations.ToLocation.AddressName, locations.ToLocation.HouseNumber);
            //}
            //catch (Exception)
            //{
            //    return null;
            //}


            if (locations.FromLocation == null || locations.ToLocation == null)
                throw new ArgumentException("Wrong addresses");

            RoutePoint start = new RoutePoint
            {
                AddressName = locations.FromLocation.AddressName,
                CityId = locations.FromLocation.CityId,
                HouseNumber = locations.FromLocation.HouseNumber,
                IsPlace = locations.FromLocation.IsPlace,
                Lat = locations.FromLocation.Lat,
                Lng = locations.FromLocation.Lng,
                Atype = locations.FromLocation.Atype
            };

            RoutePoint end = new RoutePoint
            {
                AddressName = locations.ToLocation.AddressName,
                CityId = locations.ToLocation.CityId,
                HouseNumber = locations.ToLocation.HouseNumber,
                IsPlace = locations.ToLocation.IsPlace,
                Lat = locations.ToLocation.Lat,
                Lng = locations.ToLocation.Lng,
                Atype = locations.ToLocation.Atype
            };

            Route route = new Route
            {
                Comment = "created from bot",
                Entrance = 1,
                IsOfficeBuilding = false,
                RoutePoints = new List<RoutePoint>
                {
                    start,
                    end
                }
            };
            var cur = _uow.ChannelUsers.FirstOrDefault(u => u.ProviderId == providerId);

            OrderToCreate order = new OrderToCreate
            {
                CityId = locations.FromLocation.CityId,
                //// todo implement different types
                CarType = 1,
                Phone = _uow.ChannelUsers.FirstOrDefault(u => u.ProviderId == cur.ProviderId).PhoneNumber,
                Route = route,
                ClientName = cur.ProviderId,
                PaymentType = 0
            };

            var postData = JsonConvert.SerializeObject(order);

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(postData);
            }

            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException e)
            {
                throw;
            }

            string orderId = new StreamReader(response.GetResponseStream()).ReadToEnd();
            OrderIdInfo orderIdInfo = JsonConvert.DeserializeObject<OrderIdInfo>(orderId);
            return orderIdInfo.Uid;
        }

        public double CalculateAmmount(Location fromLocation, Location toLocation)
        {
            string url = "https://test.uklon.com.ua/api/v1/orders/cost";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers.Add("client_id", WebConfigurationManager.AppSettings["UklonClientId"]);
            request.Headers.Add("Locale", "ru");
            request.Headers.Add("City", "kiev");

            request.ContentType = "application/json";
            request.Method = "POST";

            
            RoutePoint start = new RoutePoint
            {
                AddressName = fromLocation.AddressName,
                CityId = fromLocation.CityId,
                HouseNumber = fromLocation.HouseNumber,
                IsPlace = fromLocation.IsPlace,
                Lat = fromLocation.Lat,
                Lng = fromLocation.Lng,
                Atype = fromLocation.Atype
            };

            RoutePoint end = new RoutePoint
            {
                AddressName = toLocation.AddressName,
                CityId = toLocation.CityId,
                HouseNumber = toLocation.HouseNumber,
                IsPlace = toLocation.IsPlace,
                Lat = toLocation.Lat,
                Lng = toLocation.Lng,
                Atype = toLocation.Atype
            };

            Route route = new Route
            {
                Comment = "created from bot",
                Entrance = 1,
                IsOfficeBuilding = false,
                RoutePoints = new List<RoutePoint>
                {
                    start,
                    end
                }
            };

            OrderToCreate order = new OrderToCreate
            {
                CityId = fromLocation.CityId,
                //// todo implement different types
                CarType = 1,
               // Phone = _uow.Users.FirstOrDefault(u => u.ViberId == currentDialog.ViberUserId).PhoneNumber,
                Route = route,
               // ClientName = currentDialog.ViberUserId,
                PaymentType = 0
            };

            var postData = JsonConvert.SerializeObject(order);

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(postData);
            }

            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }

            catch (WebException)
            {
                throw new WebException("Server not responding");
            }

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            OrderDetails orderInfo = JsonConvert.DeserializeObject<OrderDetails>(responseString);
            return orderInfo.Cost;
        }

        public Location GetPlaceLocation(string currentDialogPickupStreet, string currentDialogPickupHouse)
        {
            string url = "https://test.uklon.com.ua/api/v1/addresses/location";
            var uriBuilder = new UriBuilder(url);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["address"] = currentDialogPickupStreet;
            query["houseNumber"] = currentDialogPickupHouse;
            uriBuilder.Query = query.ToString();
            url = uriBuilder.ToString();

            string jsonResult = string.Empty;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers.Add("client_id", WebConfigurationManager.AppSettings["UklonClientId"]);
            request.Headers.Add("Locale", "ru");
            request.Headers.Add("City", "kiev");

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                {
                    if (response.StatusCode == HttpStatusCode.NotFound)
                        throw new Exception("address not found");

                    if (stream != null)
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            jsonResult = reader.ReadToEnd();
                        }
                }
            }
            catch (WebException)
            {
                return null;
            }

            Location location = JsonConvert.DeserializeObject<Location>(jsonResult);

            return location;
        }

        public IEnumerable<string> GetPlaces(string queryPlace)
        {
            string url = "https://test.uklon.com.ua/api/v1/addresses";
            var uriBuilder = new UriBuilder(url);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["q"] = queryPlace;
            query["limit"] = "10";
            uriBuilder.Query = query.ToString();
            url = uriBuilder.ToString();

            string jsonResult = string.Empty;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers.Add("client_id", WebConfigurationManager.AppSettings["UklonClientId"]);
            request.Headers.Add("Locale", "ru");
            request.Headers.Add("City", "kiev");

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                    if (stream != null)
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            jsonResult = reader.ReadToEnd();
                        }
            }
            catch (WebException)
            {
                throw new WebException("Server not responding");
            }


            IEnumerable<AddressQueryInfo> objects = JsonConvert.DeserializeObject<IEnumerable<AddressQueryInfo>>(jsonResult);

            if (objects != null)
            {
                foreach (AddressQueryInfo address in objects)
                {
                    yield return address.AddressName;
                }
            }
            else
            {
                throw new ArgumentException();
            }
        }

        public OrderInfo GetOrderState(string dialogOrderId)
        {
            string url = $"https://test.uklon.com.ua/api/v1/orders/{dialogOrderId}";

            var uriBuilder = new UriBuilder(url);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);

            uriBuilder.Query = query.ToString();
            url = uriBuilder.ToString();

            string jsonResult = string.Empty;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers.Add("client_id", WebConfigurationManager.AppSettings["UklonClientId"]);
            request.Headers.Add("Locale", "ru");
            request.Headers.Add("City", "kiev");

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                    if (stream != null)
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            jsonResult = reader.ReadToEnd();
                        }
            }
            catch (WebException)
            {
                return null;
            }

            OrderInfo orderInfo = JsonConvert.DeserializeObject<OrderInfo>(jsonResult);

            return orderInfo;
        }

        public void CancelOrder(string currentDialogOrderId)
        {
            string url = $"https://test.uklon.com.ua/api/v1/orders/{currentDialogOrderId}/cancel";

            var uriBuilder = new UriBuilder(url);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);

            uriBuilder.Query = query.ToString();
            url = uriBuilder.ToString();

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "PUT";
            request.Headers.Add("client_id", WebConfigurationManager.AppSettings["UklonClientId"]);
            request.Headers.Add("Locale", "ru");
            request.Headers.Add("City", "kiev");

            var bytes = Encoding.UTF8.GetBytes("{\"client_cancel_reason\": \"bot\", \"cancel_comment\": \"bot\"}");
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = bytes.Length;

            Stream reqStream = request.GetRequestStream();
            reqStream.Write(bytes, 0, bytes.Length);

            try
            {
                request.GetResponse();
            }
            catch (WebException)
            {
                Trace.TraceError("Could not cancel order because server did not respond");
            }
        }

        public string RecreateOrder(string orderId)
        {
            string url = $"https://test.uklon.com.ua/api/v1/orders/{orderId}/recreate";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers.Add("client_id", WebConfigurationManager.AppSettings["UklonClientId"]);
            request.Headers.Add("Locale", "ru");
            request.Headers.Add("City", "kiev");

            request.ContentType = "application/json";
            request.Method = "POST";

            RecreateOrderInfo recreateOrderInfo = new RecreateOrderInfo
            {
                ExtraCost = 0,
                UklonDriverOnly = true
            };

            var postData = JsonConvert.SerializeObject(recreateOrderInfo);

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(postData);
            }

            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }

            catch (WebException)
            {
                return null;
            }

            string newOrderId = new StreamReader(response.GetResponseStream()).ReadToEnd();
            OrderIdInfo orderIdInfo = JsonConvert.DeserializeObject<OrderIdInfo>(newOrderId);
            return orderIdInfo.Uid;
        }

        public string GetRecreatedOrderId(string orderId)
        {
            string url = $"https://test.uklon.com.ua/api/v1/orders/{orderId}/recreated";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers.Add("client_id", WebConfigurationManager.AppSettings["UklonClientId"]);
            request.Headers.Add("Locale", "ru");
            request.Headers.Add("City", "kiev");

            request.ContentType = "application/json";
            request.Method = "POST";

            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }

            catch (WebException)
            {
                return null;
            }

            string newOrderId = new StreamReader(response.GetResponseStream()).ReadToEnd();
            OrderIdInfo orderIdInfo = JsonConvert.DeserializeObject<OrderIdInfo>(newOrderId);
            return orderIdInfo.Uid;
        }

        public bool Authenticate(string phoneNumber, string viberId)
        {
            string url = "https://test.uklon.com.ua/api/bots/ms/account/auth";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers.Add("client_id", WebConfigurationManager.AppSettings["UklonClientId"]);

            request.ContentType = "application/json";
            request.Method = "POST";

            AuthInfo authInfo = new AuthInfo()
            {
                Provider = "emulator",
                ProviderId = viberId
            };

            var postData = JsonConvert.SerializeObject(authInfo);

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(postData);
            }

            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
                string resp = new StreamReader(response.GetResponseStream()).ReadToEnd();

                AuthResult authResult = JsonConvert.DeserializeObject<AuthResult>(resp);

                User currentUser = _uow.Users.FirstOrDefault(u => u.ViberId == viberId);
                currentUser.UklonUserToken = authResult.AccessToken;
                currentUser.UklonTokenExpirationDate = authResult.Expires;
                _uow.Users.Update(currentUser);
            }

            catch (WebException e)
            {
                return false;
            }

            return true;
        }

        public bool Register(string phoneNumber, string provider, string providerId, string phoneValidationCode)
        {

            //TODO move it to user service
            var cur = _uow.ChannelUsers.FirstOrDefault(u => u.ProviderId == providerId);

            if (cur == null)
            {
                cur = new ChannelUser()
                {
                    ProviderId = providerId,
                    Provider = provider,
                    IsPhoneNumberConfirmed = false

                };

                _uow.ChannelUsers.Create(cur);
                _uow.Save();

            }
            //
            string url = "https://test.uklon.com.ua/api/bots/ms/account/register";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers.Add("client_id", WebConfigurationManager.AppSettings["UklonClientId"]);

            request.ContentType = "application/json";
            request.Method = "POST";

            AutoRegisterInfo autoRegisterInfo = new AutoRegisterInfo
            {
                Provider = provider,
                ProviderId = providerId,
                Phone = phoneNumber,
                Code = "267671"
            };

            var postData = JsonConvert.SerializeObject(autoRegisterInfo);
            
            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(postData);
            }

            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                string resp = new StreamReader(response.GetResponseStream()).ReadToEnd();

                AuthResult authResult = JsonConvert.DeserializeObject<AuthResult>(resp);

                ChannelUser currentUser = _uow.ChannelUsers.FirstOrDefault(u => u.ProviderId == providerId);
                currentUser.UklonUserToken = authResult.AccessToken;
                currentUser.UklonTokenExpirationDate = authResult.Expires;
                _uow.ChannelUsers.Update(currentUser);
            }

            catch (WebException e)
            {
                return false;
            }

            return true;
        }

        public void ConfirmPhone(string phoneNumber)
        {
            string url = "https://test.uklon.com.ua/api/v1/phone/verification";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            request.Headers.Add("client_id", WebConfigurationManager.AppSettings["UklonClientId"]);
            request.ContentType = "application/json";
            request.Method = "POST";

            PhoneToConfirm phoneToConfirm = new PhoneToConfirm
            {
                Phone = phoneNumber
            };

            var postData = JsonConvert.SerializeObject(phoneToConfirm);

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(postData);
            }

            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                string test = new StreamReader(response.GetResponseStream()).ReadToEnd();
            }
            catch (WebException)
            {
                //// todo throw
                Trace.WriteLine("Server not responding");
            }
        }
    
}
}