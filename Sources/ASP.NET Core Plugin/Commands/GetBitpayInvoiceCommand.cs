using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Plugin.Apiqu.Payments.Bitpay.Components;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Core.Commands;
using Sitecore.Commerce.Plugin.Carts;

namespace Plugin.Apiqu.Payments.Bitpay.Commands
{
    public class GetBitpayInvoiceCommand : CommerceCommand
    {
        private readonly IFindEntityPipeline _getCartPipeline;

        public GetBitpayInvoiceCommand(IFindEntityPipeline getCartPipeline)
        {
            _getCartPipeline = getCartPipeline;
        }

        public virtual async Task<string> Process(CommerceContext commerceContext, string cartId)
        {
            GetBitpayInvoiceCommand getBitpayInvoiceCommand = this;
            Cart result = (Cart)null;
            Cart cart1;
            string invoiceId = "";

            using (CommandActivity.Start(commerceContext, (CommerceCommand)getBitpayInvoiceCommand))
            {
                Func<Task> func = await getBitpayInvoiceCommand.PerformTransaction(commerceContext, (Func<Task>)(async () =>
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
                        if (cart.HasComponent<BitpayPaymentComponent>())
                        {
                            var bitpayComponent = cart.GetComponent<BitpayPaymentComponent>();
                            invoiceId = bitpayComponent.InvoiceId;
                        }
                    }
                }));
                cart1 = result;
            }
            return invoiceId;
        }
    }
}
