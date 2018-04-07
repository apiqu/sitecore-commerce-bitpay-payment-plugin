using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Commerce.Plugin.Payments;

namespace Plugin.Apiqu.Payments.Bitpay.Pipelines.Arguments
{
    public class AddBitpayPaymentArgument: CartPaymentsArgument
    {
        public string Email { get; set; }
        public string InvoiceId { get; set; }
        public string InvoiceUrl { get; set; }
        public string BitcoinAmount { get; set; }

        public AddBitpayPaymentArgument(Cart cart, IEnumerable<PaymentComponent> payments, string email,
            string invoiceId, string invoiceUrl, string bitcoinAmount) : base(cart, payments)
        {
            this.Email = email;
            this.InvoiceId = invoiceId;
            this.InvoiceUrl = invoiceUrl;
            this.BitcoinAmount = bitcoinAmount;
        }
    }
}
