using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.OData.Builder;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Core.Commands;
using Sitecore.Commerce.Plugin.Payments;
using Sitecore.Framework.Conditions;
using Sitecore.Framework.Pipelines;

namespace Plugin.Apiqu.Payments.Bitpay
{
    public class ConfigureServiceApiBlock : PipelineBlock<ODataConventionModelBuilder, ODataConventionModelBuilder, CommercePipelineExecutionContext>
    {
        /// <summary>
        /// The _pipeline.
        /// </summary>
        private readonly IPersistEntityPipeline _pipeline;

        public ConfigureServiceApiBlock(IPersistEntityPipeline persistEntityPipeline)
        {
            this._pipeline = persistEntityPipeline;
        }

        public override Task<ODataConventionModelBuilder> Run(ODataConventionModelBuilder modelBuilder, CommercePipelineExecutionContext context)
        {
            Condition.Requires(modelBuilder).IsNotNull($"{base.Name}: The argument can not be null");

            var configuration = modelBuilder.Action("AddBitpayPayment");
            configuration.Parameter<string>("cartId");
            configuration.Parameter<string>("email");
            configuration.Parameter<string>("invoiceId");
            configuration.Parameter<string>("invoiceUrl");
            configuration.Parameter<string>("bitcoinAmount");
            configuration.Parameter<FederatedPaymentComponent>("payment");
            configuration.ReturnsFromEntitySet<CommerceCommand>("Commands");

            var actionConfiguration2 = modelBuilder.Action("GetBitpayInvoice");
            actionConfiguration2.Parameter<string>("cartId");
            actionConfiguration2.ReturnsFromEntitySet<CommerceCommand>("Commands");

            return Task.FromResult(modelBuilder);
        }
    }
}
