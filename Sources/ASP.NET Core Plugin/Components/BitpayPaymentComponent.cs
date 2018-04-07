using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Payments;

namespace Plugin.Apiqu.Payments.Bitpay.Components
{
    public class BitpayPaymentComponent : Component
    { 
        public string InvoiceId { get; set; }
        public string InvoiceUrl { get; set; }
        public string Currency { get; set; }
        public decimal Price { get; set; }
        public string BtcAmount { get; set; }
    }
}
