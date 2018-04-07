using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Plugin.Apiqu.Payments.Bitpay.Pipelines;
using Plugin.Apiqu.Payments.Bitpay.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Core.Commands;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Commerce.Plugin.Payments;

namespace Plugin.Apiqu.Payments.Bitpay.Commands
{
    public class AddBitpayPaymentCommand : CommerceCommand
    {
        private readonly IAddBitpayPaymentPipeline _addBitpayPaymentPipeline;
        private readonly IFindEntityPipeline _getCartPipeline;

        public AddBitpayPaymentCommand(IFindEntityPipeline getCartPipeline, IAddBitpayPaymentPipeline addBitpayPaymentPipeline, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _addBitpayPaymentPipeline = addBitpayPaymentPipeline;
            _getCartPipeline = getCartPipeline;
        }

        public virtual async Task<Cart> Process(CommerceContext commerceContext, string cartId, IEnumerable<PaymentComponent> payments, string email, string invoiceId, string invoiceUrl, string bitcoinAmount)
        {
            AddBitpayPaymentCommand addPaymentsCommand = this;
            Cart result = (Cart)null;
            Cart cart1;

            using (CommandActivity.Start(commerceContext, (CommerceCommand)addPaymentsCommand))
            {
                Func<Task> func = await addPaymentsCommand.PerformTransaction(commerceContext, (Func<Task>)(async () =>
                {
                    CommercePipelineExecutionContextOptions pipelineContextOptions = commerceContext.GetPipelineContextOptions();

                    Cart cart = await this._getCartPipeline.Run(new FindEntityArgument(typeof(Cart), cartId, false), pipelineContextOptions) as Cart;

                    if (cart == null)
                    {

                        string str = await commerceContext.AddMessage(commerceContext.GetPolicy<KnownResultCodes>().ValidationError, "EntityNotFound", new object[1]
                        {
                         (object) cartId
                        }, string.Format("Entity {0} was not found.", (object)cartId));
                    }
                    else
                    {

                        Cart cart2 = await this._addBitpayPaymentPipeline.Run(new AddBitpayPaymentArgument(cart, payments, email, invoiceId, invoiceUrl, bitcoinAmount), commerceContext.GetPipelineContextOptions());
                        result = cart2;
                    }
                }));
                cart1 = result;
            }
            return cart1;
        }
    }
}
