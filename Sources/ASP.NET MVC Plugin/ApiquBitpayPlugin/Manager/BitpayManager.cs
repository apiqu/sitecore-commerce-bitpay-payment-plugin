using ApiquBitpayPlugin.Models;
using BitPayAPI;
using Sitecore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ApiquBitpayPlugin.Helpers;
using Sitecore.Data;

namespace ApiquBitpayPlugin.Manager
{
    public class BitpayManager
    {
        private readonly BitPay _bitpay;
        public string RedirectUrl { get; set; }
        public BitpayManager()
        {
            try
            {
                var settingItem = BitpayHelper.GetBitpaySettingItem();
                if (settingItem != null)
                {
                    var clientName = settingItem[BitpaySetting.ClientName];
                    var serverUrl = settingItem[BitpaySetting.ServerUrl];
                    var pairingCode = settingItem[BitpaySetting.PairingCode];

                    _bitpay = new BitPay(clientName, serverUrl);
                    if (!_bitpay.clientIsAuthorized(BitPay.FACADE_POS))
                    {
                        _bitpay.authorizeClient(pairingCode);
                    }
                }
            }
            catch (Exception ex)

            {
                Log.Error($"Fail to pair with bitpay server: {ex.Message}", ex);
            }
        }

        public Invoice CreateBitpayInvoice(Invoice requestInvoice)
        {
            var result = _bitpay.createInvoice(requestInvoice);
            return result;
        }

        public Invoice GetBitpayInvoice(string invoiceId)
        {
            var result = _bitpay.getInvoice(invoiceId);
            return result;
        }

    }
}