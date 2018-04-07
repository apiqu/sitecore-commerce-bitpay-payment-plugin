using ApiquBitpayPlugin.Models;
using BitPayAPI;
using Sitecore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data.Items;

namespace ApiquBitpayPlugin.Helpers
{
    public static class BitpayHelper
    {
        public static Item GetBitpaySettingItem()
        {
            return Sitecore.Context.Database.GetItem(BitpaySetting.BitpaySettingItem);
        }
    }
}