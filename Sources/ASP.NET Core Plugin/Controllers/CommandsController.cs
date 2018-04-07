using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.OData;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Plugin.Apiqu.Payments.Bitpay.Commands;
using Plugin.Apiqu.Payments.Bitpay.Components;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Commerce.Plugin.Payments;

namespace Plugin.Apiqu.Payments.Bitpay.Controllers
{
    
    public class CommandsController : CommerceController
    {
        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Sitecore.Commerce.Plugin.Sample.CommandsController" /> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="globalEnvironment">The global environment.</param>
        public CommandsController(IServiceProvider serviceProvider, CommerceEnvironment globalEnvironment)
            : base(serviceProvider, globalEnvironment)
        {
        }

        /// <summary>
        /// Samples the command.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>A <see cref="IActionResult"/></returns>
        [HttpPut]
        [Route("AddBitpayPayment()")]
        public async Task<IActionResult> AddBitpayPayment([FromBody] ODataActionParameters value)
        {

            CommandsController commandsController = this;
            if (!commandsController.ModelState.IsValid || value == null)
                return (IActionResult)new BadRequestObjectResult(commandsController.ModelState);
            if (value.ContainsKey("cartId"))
            {
                object obj1 = value["cartId"];
                
                var email = value["email"].ToString();
                var invoiceId = value["invoiceId"].ToString();
                var invoiceUrl = value["invoiceUrl"].ToString();
                var bitcoinAmount = value["bitcoinAmount"].ToString();

                if (!string.IsNullOrEmpty(obj1 != null ? obj1.ToString() : (string)null) && value.ContainsKey("payment"))
                {
                    object obj2 = value["payment"];
                    if (!string.IsNullOrEmpty(obj2 != null ? obj2.ToString() : (string)null))
                    {
                        string cartId = value["cartId"].ToString();
                       
                        FederatedPaymentComponent paymentComponent = JsonConvert.DeserializeObject<FederatedPaymentComponent>(value["payment"].ToString());
                        AddBitpayPaymentCommand command = commandsController.Command<AddBitpayPaymentCommand>();
                        Cart cart = await command.Process(commandsController.CurrentContext, cartId,
                            (IEnumerable<PaymentComponent>) new List<PaymentComponent>()
                            {
                                (PaymentComponent) paymentComponent
                            }, email, invoiceId, invoiceUrl, bitcoinAmount);
                        return (IActionResult)new ObjectResult((object)command);
                    }
                }
            }
            return (IActionResult)new BadRequestObjectResult((object)value);
        }

        [HttpPut]
        [Route("GetBitpayInvoice()")]
        public async Task<string> GetBitpayInvoice([FromBody] ODataActionParameters value)
        {

            CommandsController commandsController = this;
            if (!commandsController.ModelState.IsValid || value == null)
                return "";
            if (value.ContainsKey("cartId"))
            {
                object obj1 = value["cartId"];

                if (!string.IsNullOrEmpty(obj1 != null ? obj1.ToString() : ""))
                {

                    string cartId = value["cartId"].ToString();
                    GetBitpayInvoiceCommand command = commandsController.Command<GetBitpayInvoiceCommand>();
                    string id = await command.Process(commandsController.CurrentContext, cartId);
                    return id;

                }
            }
            return "";
        }
    }
}
