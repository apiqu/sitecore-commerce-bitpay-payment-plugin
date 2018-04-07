using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using ApiquBitpayPlugin.Manager;
using Sitecore.Mvc.Controllers;
using Sitecore.Mvc.Presentation;
using System.Web.Mvc;
using ApiquBitpayPlugin.Helpers;
using ApiquBitpayPlugin.Models;
using BitPayAPI;
using Sitecore.Commerce.Connect.CommerceServer.Orders.Models;
using Sitecore.Commerce.Engine.Connect;
using Sitecore.Commerce.Entities.Carts;
using Sitecore.Commerce.Services.Orders;
using Sitecore.Commerce.Services.Payments;
using Sitecore.Diagnostics;
using Sitecore.Links;
using PaymentOption = Sitecore.Commerce.Entities.Payments.PaymentOption;

namespace ApiquBitpayPlugin.Controllers
{
    public class BipayPaymentController : SitecoreController
    {
        
        [HttpPost]
        [AllowAnonymous]
        public ActionResult CreateInvoice(BitpayViewmodel model)
        {
            var visitorContext = new VisitorContext();
            var cartManager = new CartManager(model.ShopName, CartManager.ShoppingCartName, visitorContext.GetCustomerId());
            var cart = cartManager.GetCart();
            if (string.IsNullOrEmpty(cart.Email))
            {
                cart.Email = model.Email;
            }
            var bitpay = new BitpayManager();
            var requestInvoice =
                new Invoice((double)cart.Total.Amount, cart.Total.CurrencyCode)
                {
                    NotificationURL = GetCallbackUrl(),
                    RedirectURL = BitpayHelper.GetBitpaySettingItem()[BitpaySetting.RedirectUrl],
                    PosData = cart.ExternalId,
                    BuyerEmail = cart.Email,
                };

            var invoice = bitpay.CreateBitpayInvoice(requestInvoice);
            var result = InvokeHttpClientPost(cart, invoice, model.ShopName);
            if (result) return Json(invoice.Url, JsonRequestBehavior.AllowGet);
            return HttpNotFound();
        }

        public ActionResult GetBitpyButton()
        {
            var shopName = RenderingContext.Current.ContextItem.GetSite().Name;
            var visitorContext = new VisitorContext();
            var cartManager = new CartManager(shopName, CartManager.ShoppingCartName, visitorContext.GetCustomerId());
            var cart = cartManager.GetCart();
            var paymentService = new PaymentServiceProvider();
            var request = new GetPaymentOptionsRequest(shopName, cart);
            var result = paymentService.GetPaymentOptions(request);
            var options = new List<PaymentOption>();
            var model = new BitpayViewmodel { ShopName = shopName };
            if (!result.Success)
            {
                LogHelpers.LogSystemMessages(result.SystemMessages, result);
            }
            else
            {
                options = result.PaymentOptions.ToList();
            }

            model.IsBitpayEnable = options.Any(x => x.Description == "Bitpay");
            return View(model);
        }


        [HttpPost]
        [AllowAnonymous]
        public ActionResult BitpayHookHandling(BitpayRequest request)
        {
            try
            {
                var bitpay = new BitpayManager();
                var invoice = bitpay.GetBitpayInvoice(request.id);
                if (invoice != null)
                {
                    if (invoice.ExceptionStatus == "false")
                    {
                        switch (invoice.Status)
                        {
                            // TODO: implement business logic here
                            case "new":
                                break;
                            case "paid":
                                break;
                            case "confirmed":
                                break;
                            case "complete":
                                break;
                            case "expired":
                                break;
                            case "invalid":
                                break;
                        }
                    }
                    else
                    {
                        switch (invoice.ExceptionStatus)
                        {
                            // TODO: implement business logic here
                            case "paidPartial":
                                break;
                            case "paidOver":
                                break;
                            case "paidLate":
                                break;

                        }
                    }

                }
                return Json("OK");
            }
            catch (Exception ex)
            {
                Log.Error($"handle bitpay request fail: {ex.Message}", ex);
                throw;
            }
            
        }

        private string GetCallbackUrl()
        {
            Sitecore.Links.UrlOptions urlOptions = new UrlOptions()
            {
                AddAspxExtension = false,
                AlwaysIncludeServerUrl = true,
            };
            var startItem = Sitecore.Context.Database.GetItem(Sitecore.Context.Site.ContentStartPath);
            var url = LinkManager.GetItemUrl(startItem, urlOptions);
            return url + "/api/sitecore/BipayPayment/BitpayHookHandling";
        }

        public bool InvokeHttpClientPost(Cart cart, Invoice invoice, string shopname)
        {
            try
            {
                var party = (CommerceParty) cart.Parties.FirstOrDefault(); 
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(EngineConnectUtility.EngineConfiguration.ShopsServiceUrl + "/AddBitpayPayment");
                httpWebRequest.Timeout = Int32.MaxValue;
                httpWebRequest.Method = "PUT";
                httpWebRequest.Headers.Add("ShopName", EngineConnectUtility.EngineConfiguration.DefaultShopName);
                httpWebRequest.Headers.Add("Language", "en-US");
                httpWebRequest.Headers.Add("ShopperId", "1");
                httpWebRequest.Headers.Add("Environment", EngineConnectUtility.EngineConfiguration.DefaultEnvironment);
                httpWebRequest.ContentType = "application/json";
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json =
                        "{\"cartId\":\"" + cart.ExternalId + "\", \"payment\": {\"@odata.type\": \"Sitecore.Commerce.Plugin.Payments.FederatedPaymentComponent\", \"PaymentMethod\": {\"EntityTarget\": \"0CFFAB11-2674-4A18-AB04-228B1F8A1DEC\", \"Name\": \"Federated\"}, \"PaymentMethodNonce\": \"212132\"," +
                        "\"Amount\": { \"Amount\": " + cart.Total.Amount + "}, " + "\"BillingParty\": {\"AddressName\": \"" + cart.Email +
                        "\", \"FirstName\": \"" + party?.FirstName  + "\", \"LastName\":\"" + party?.LastName + "\", \"City\": \"" + party?.City + "\", \"Address1\": \"" + party?.Address1 + "\", \"State\": \"" + party?.RegionCode
                        + "\", \"Country\": \"" + party?.CountryCode + "\", \"ZipPostalCode\":\"" + party?.ZipPostalCode + "\"} }, \"email\":\"" + cart.Email + "\", \"invoiceId\":\"" + invoice.Id + "\", \"invoiceUrl\":\"" + invoice.Url + "\", \"bitcoinAmount\":\"" + invoice.BtcPrice+ "\"  }";

                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();

                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        if (result.Contains("ResponseCode\":\"Error")) return false;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;

            }

            return true;
        }

    }
}