using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Plugin.Apiqu.Payments.Bitpay.Pipelines;
using Plugin.Apiqu.Payments.Bitpay.Pipelines.Blocks;
using Plugin.Sample.Payments.Braintree;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Commerce.Plugin.Orders;
using Sitecore.Framework.Configuration;
using Sitecore.Framework.Pipelines.Definitions.Extensions;
using ConfigureServiceApiBlock = Sitecore.Commerce.Core.ConfigureServiceApiBlock;
namespace Plugin.Apiqu.Payments.Bitpay
{
    public class ConfigureSitecore : IConfigureSitecore
    {
        /// <summary>
        /// The configure services.
        /// </summary>
        /// <param name="services">
        /// The services.
        /// </param>
        public void ConfigureServices(IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();
            services.RegisterAllPipelineBlocks(assembly);

            services.Sitecore().Pipelines(config => config

                .AddPipeline<IAddBitpayPaymentPipeline, AddBitpayPaymentPipeline>(
                    configure =>
                    {
                        configure.Add<ValidateCartAndBitpayPaymentsBlock>()
                            .Add<ValidateBitpayPaymentBlock>()
                            .Add<AddBitpayPaymentBlock>()
                            .Add<ICalculateCartLinesPipeline>()
                            .Add<ICalculateCartPipeline>()
                            .Add<PersistCartBlock>()
                            .Add<WriteCartTotalsToContextBlock>();
                    })

                .ConfigurePipeline<ICreateOrderPipeline>(builder => builder.Replace<CreateFederatedPaymentBlock, CreateFederatedPaymentBlockBitpay>())

                .ConfigurePipeline<IConfigureServiceApiPipeline>(configure => configure.Add<ConfigureServiceApiBlock>()));

            services.RegisterAllCommands(assembly);
        }
    }
}
