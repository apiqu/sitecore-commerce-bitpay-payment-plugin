using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Plugin.Apiqu.Payments.Bitpay.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Framework.Pipelines;

namespace Plugin.Apiqu.Payments.Bitpay.Pipelines
{
    public interface IAddBitpayPaymentPipeline : IPipeline<AddBitpayPaymentArgument, Cart, CommercePipelineExecutionContext>, IPipelineBlock<AddBitpayPaymentArgument, Cart, CommercePipelineExecutionContext>, IPipelineBlock, IPipeline
    {
    }
}
