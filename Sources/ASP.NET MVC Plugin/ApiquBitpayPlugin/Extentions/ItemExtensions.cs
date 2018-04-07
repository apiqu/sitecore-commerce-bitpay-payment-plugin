using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Data.Managers;
using Sitecore.Links;
using Sitecore.Web;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApiquBitpayPlugin.Helpers
{
    public static class ItemExtensions
    {
        /// <summary>
        /// Returns if an item is derived from a specific template
        /// https://laubplusco.net/sitecore-extensions-does-a-sitecore-item-derive-from-a-template/
        /// </summary>
        /// <param name="item"></param>
        /// <param name="templateId"></param>
        /// <returns></returns>
        public static bool IsDerived([NotNull] this Item item, [NotNull] ID templateId)
        { 
            return TemplateManager.GetTemplate(item).IsDerived(templateId);
        }

        /// <summary>
        /// Return all children of an item that derive from a specific template
        /// https://laubplusco.net/sitecore-extensions-does-a-sitecore-item-derive-from-a-template/
        /// </summary>
        /// <param name="item"></param>
        /// <param name="templateId"></param>
        /// <returns></returns>
        public static IEnumerable<Item> GetChildrenDerivedFrom(this Item item, ID templateId)
        {
            return item.GetChildren().Where(c => c.IsDerived(templateId));
        }

        public static string Url(this Item item, UrlOptions options = null)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return options != null ? LinkManager.GetItemUrl(item, options) : LinkManager.GetItemUrl(item);
        }

        public static Item GetAncestorOrSelfOfTemplate(this Item item, ID templateID)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return item.IsDerived(templateID) ? item : item.Axes.GetAncestors().Reverse().FirstOrDefault(i => i.IsDerived(templateID));
        }

        public static SiteInfo GetSite(this Item item)
        {
            var siteInfoList = Sitecore.Configuration.Factory.GetSiteInfoList();
            var defaultSiteName = "storefront";

            var currentSiteinfo = siteInfoList.SingleOrDefault(s => s.Name == defaultSiteName);

            return currentSiteinfo;
        }

        public static string GetPaymentOptionId(string paymentType)
        {
            var paymentOptions = Sitecore.Context.Database.GetItem("/sitecore/Commerce/Commerce Control Panel/Shared Settings/Payment Options");
            if (paymentOptions == null || !paymentOptions.Children.Any()) return string.Empty;
            var paymentOption = paymentOptions.Children.FirstOrDefault(o => o.Name.Equals(paymentType, StringComparison.OrdinalIgnoreCase));
            return paymentOption?.ID.ToGuid().ToString("D") ?? string.Empty;
        }
    }
}