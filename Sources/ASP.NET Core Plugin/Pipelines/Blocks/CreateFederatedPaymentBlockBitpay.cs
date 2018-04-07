using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Braintree;
using Braintree.Exceptions;
using Plugin.Sample.Payments.Braintree;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Orders;
using Sitecore.Commerce.Plugin.Payments;
using Sitecore.Framework.Conditions;
using Sitecore.Framework.Pipelines;

namespace Plugin.Apiqu.Payments.Bitpay.Pipelines.Blocks
{
    
   
    public class CreateFederatedPaymentBlockBitpay : PipelineBlock<CartEmailArgument, CartEmailArgument, CommercePipelineExecutionContext>
    {
        public override async Task<CartEmailArgument> Run(CartEmailArgument arg, CommercePipelineExecutionContext context)
        {
            Condition.Requires(arg).IsNotNull($"{this.Name}: The cart can not be null");

            var cart = arg.Cart;
            if (!cart.HasComponent<FederatedPaymentComponent>())
            {
                return arg;
            }

            var payment = cart.GetComponent<FederatedPaymentComponent>();

            if (!string.IsNullOrEmpty(payment.Comments) && string.Equals("Bitcoin", payment.Comments, System.StringComparison.CurrentCultureIgnoreCase))
            {
                return arg;
            }
            if (string.IsNullOrEmpty(payment.PaymentMethodNonce))
            {
                context.Abort(
                    await context.CommerceContext.AddMessage(
                        context.GetPolicy<KnownResultCodes>().Error,
                        "InvalidOrMissingPropertyValue",
                        new object[] { "PaymentMethodNonce" },
                        "Invalid or missing value for property 'PaymentMethodNonce'."),
                    context);
                return arg;
            }

            var braintreeClientPolicy = context.GetPolicy<BraintreeClientPolicy>();
            if (string.IsNullOrEmpty(braintreeClientPolicy.Environment) || string.IsNullOrEmpty(braintreeClientPolicy.MerchantId)
                || string.IsNullOrEmpty(braintreeClientPolicy.PublicKey) || string.IsNullOrEmpty(braintreeClientPolicy.PrivateKey))
            {
                await context.CommerceContext.AddMessage(
                   context.GetPolicy<KnownResultCodes>().Error,
                   "InvalidClientPolicy",
                   new object[] { "BraintreeClientPolicy" },
                    $"{this.Name}. Invalid BraintreeClientPolicy");
                return arg;
            }

            try
            {
                var gateway = new BraintreeGateway(braintreeClientPolicy.Environment, braintreeClientPolicy.MerchantId, braintreeClientPolicy.PublicKey, braintreeClientPolicy.PrivateKey);

                var request = new TransactionRequest
                {
                    Amount = payment.Amount.Amount,
                    PaymentMethodNonce = payment.PaymentMethodNonce,
                    BillingAddress = TranslatePartyToAddressRequest(payment.BillingParty, context),
                    Options = new TransactionOptionsRequest
                    {
                        SubmitForSettlement = false
                    }
                };

                var result = gateway.Transaction.Sale(request);
                if (result.IsSuccess())
                {
                    var transaction = result.Target;
                    payment.TransactionId = transaction?.Id;
                    payment.TransactionStatus = transaction?.Status?.ToString();
                    payment.PaymentInstrumentType = transaction?.PaymentInstrumentType?.ToString();

                    var cc = transaction?.CreditCard;
                    payment.MaskedNumber = cc?.MaskedNumber;
                    payment.CardType = cc?.CardType?.ToString();
                    if (cc?.ExpirationMonth != null)
                    {
                        payment.ExpiresMonth = int.Parse(cc.ExpirationMonth);
                    }

                    if (cc?.ExpirationYear != null)
                    {
                        payment.ExpiresYear = int.Parse(cc.ExpirationYear);
                    }
                }
                else
                {
                    var errorMessages = string.Concat(result.Message, " ", result.Errors.DeepAll().Aggregate(string.Empty, (current, error) => current + ("Error: " + (int)error.Code + " - " + error.Message + "\n")));
                    context.Abort(
                        await context.CommerceContext.AddMessage(
                           context.GetPolicy<KnownResultCodes>().Error,
                           "CreatePaymentFailed",
                           new object[] { "PaymentMethodNonce" },
                           $"{this.Name}. Create payment failed :{ errorMessages }"),
                        context);
                }

                return arg;
            }
            catch (BraintreeException ex)
            {
                context.Abort(
                    await context.CommerceContext.AddMessage(
                       context.GetPolicy<KnownResultCodes>().Error,
                       "CreatePaymentFailed",
                       new object[] { "PaymentMethodNonce", ex },
                       $"{this.Name}. Create payment failed."),
                    context);
                return arg;
            }
        }

        internal static protected AddressRequest TranslatePartyToAddressRequest(Party party, CommercePipelineExecutionContext context)
        {
            var addressRequest = new AddressRequest();
            addressRequest.CountryCodeAlpha2 = party.CountryCode;
            addressRequest.CountryName = party.Country;
            addressRequest.FirstName = party.FirstName;
            addressRequest.LastName = party.LastName;
            addressRequest.PostalCode = party.ZipPostalCode;
            addressRequest.StreetAddress = string.Concat(party.Address1, ",", party.Address2);

            return addressRequest;
        }
    }
}
