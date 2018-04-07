using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Commerce.Plugin.Payments;
using Sitecore.Commerce.Plugin.Pricing;
using Sitecore.Framework.Conditions;
using Sitecore.Framework.Pipelines;

namespace Plugin.Apiqu.Payments.Bitpay.Pipelines.Blocks
{
    /// <summary>
    /// Defines a block
    /// </summary>
    /// <seealso>
    ///     <cref>
    ///         Sitecore.Framework.Pipelines.PipelineBlock{Sitecore.Commerce.Plugin.Sample.SampleArgument,
    ///         Sitecore.Commerce.Plugin.Sample.SampleEntity, Sitecore.Commerce.Core.CommercePipelineExecutionContext}
    ///     </cref>
    /// </seealso>
   public class AddBitpayPaymentBlock : PipelineBlock<Cart, Cart, CommercePipelineExecutionContext>
    {
        /// <summary>
        /// The execute.
        /// </summary>
        /// <param name="arg">
        /// The SampleArgument argument.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <returns>
        /// The <see cref="SampleEntity"/>.
        /// </returns>
        /// 
        public AddBitpayPaymentBlock()
            : base((string)null)
        {
        }
        public override async Task<Cart> Run(Cart arg, CommercePipelineExecutionContext context)
        {

            AddBitpayPaymentBlock addPaymentsBlock = this;
            // ISSUE: explicit non-virtual call
            Condition.Requires<Cart>(arg).IsNotNull<Cart>(string.Format("{0}: The argument cannot be null.", addPaymentsBlock.Name));
            CartPaymentsArgument argument = context.CommerceContext.GetObjects<CartPaymentsArgument>().FirstOrDefault();
            CommercePipelineExecutionContext executionContext;
            if (argument == null)
            {
                executionContext = context;
                CommerceContext commerceContext = context.CommerceContext;
                string error = context.GetPolicy<KnownResultCodes>().Error;
                string commerceTermKey = "ArgumentNotFound";
                object[] args = new object[1]
                {
                    (object) typeof (CartPaymentsArgument).Name
                };
                string defaultMessage = string.Format("Argument of type {0} was not found in context.", (object)typeof(CartPaymentsArgument).Name);
                executionContext.Abort(await commerceContext.AddMessage(error, commerceTermKey, args, defaultMessage), (object)context);
                executionContext = (CommercePipelineExecutionContext)null;
                return (Cart)null;
            }
            Cart cart = argument.Cart;
            foreach (PaymentComponent payment in argument.Payments)
            {
                PaymentComponent p = payment;
                if (p != null)
                {
                    if (string.IsNullOrEmpty(p.Id))
                        p.Id = Guid.NewGuid().ToString("N");
                    // ISSUE: explicit non-virtual call
                    context.Logger.LogInformation(string.Format("{0} - Adding Payment {1} Amount:{2}", (addPaymentsBlock.Name), (object)p.Id, (object)p.Amount.Amount), Array.Empty<object>());
                    if (string.IsNullOrEmpty(p.Amount.CurrencyCode))
                        p.Amount.CurrencyCode = context.CommerceContext.CurrentCurrency();
                    else if (!p.Amount.CurrencyCode.Equals(context.CommerceContext.CurrentCurrency(), StringComparison.OrdinalIgnoreCase))
                    {
                        executionContext = context;
                        CommerceContext commerceContext = context.CommerceContext;
                        string error = context.GetPolicy<KnownResultCodes>().Error;
                        string commerceTermKey = "InvalidCurrency";
                        object[] args = new object[2]
                        {
                              (object) p.Amount.CurrencyCode,
                              (object) context.CommerceContext.CurrentCurrency()
                        };
                        string defaultMessage = string.Format("Invalid currency '{0}'. Valid currency is '{1}'.", (object)p.Amount.CurrencyCode, (object)context.CommerceContext.CurrentCurrency());
                        executionContext.Abort(await commerceContext.AddMessage(error, commerceTermKey, args, defaultMessage), (object)context);
                        executionContext = (CommercePipelineExecutionContext)null;
                        return (Cart)null;
                    }
                    if (context.GetPolicy<GlobalPricingPolicy>().ShouldRoundPriceCalc)
                    {
                        p.Amount.Amount = Decimal.Round(p.Amount.Amount, context.GetPolicy<GlobalPricingPolicy>().RoundDigits, context.GetPolicy<GlobalPricingPolicy>().MidPointRoundUp ? MidpointRounding.AwayFromZero : MidpointRounding.ToEven);
                        context.Logger.LogDebug(string.Format("{0} - After Rounding: {1}", (addPaymentsBlock.Name), (object)p.Amount.Amount), Array.Empty<object>());
                    }

                    p.Comments = "Bitcoin";

                    cart.SetComponent((Component)p);
                    p = (PaymentComponent)null;
                }
            }


            return cart;
        }
    }
}
