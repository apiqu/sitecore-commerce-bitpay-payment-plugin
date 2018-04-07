using Sitecore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiquBitpayPlugin.Models
{
    public class BitpaySetting
    {
        public static ID BitpaySettingItem = new ID("{6D99E257-FB71-45B7-9FA0-EA7CF446249B}");

        public static ID ClientName = new ID("{0A697E19-E1C2-4DCE-A9A5-017096D3EFB6}");

        public static ID PairingCode = new ID("{8429DD18-20CB-4C65-92BD-EA22A89242DD}");        

        public static ID ServerUrl = new ID("{138B3D4D-2D01-4460-8D2C-CF695BFCD32E}");

        public static ID RedirectUrl = new ID("{7B886986-9EB9-4766-9812-5E27141230EF}");

    }
}