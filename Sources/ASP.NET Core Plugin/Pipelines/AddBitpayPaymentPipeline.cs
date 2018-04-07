using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Plugin.Apiqu.Payments.Bitpay.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Framework.Pipelines;

namespace Plugin.Apiqu.Payments.Bitpay.Pipelines
{
    public class AddBitpayPaymentPipeline : CommercePipeline<AddBitpayPaymentArgument, Cart>, IAddBitpayPaymentPipeline
    {
        public AddBitpayPaymentPipeline(IPipelineConfiguration<IAddBitpayPaymentPipeline> configuration, ILoggerFactory loggerFactory)
            : base((IPipelineConfiguration)configuration, loggerFactory)
        {
        }
    }
}
