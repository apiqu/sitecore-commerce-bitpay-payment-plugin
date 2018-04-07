using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Sitecore.Commerce.Entities.Payments;
using Sitecore.Pipelines.GetAboutInformation;
using Sitecore.Pipelines.WebDAV.Processors;

namespace ApiquBitpayPlugin.Models
{
    public class BitpayViewmodel
    {
        public string ShopName { get; set; }
        public bool IsBitpayEnable { get; set; }
        public string Email { get; set; }
    }
}