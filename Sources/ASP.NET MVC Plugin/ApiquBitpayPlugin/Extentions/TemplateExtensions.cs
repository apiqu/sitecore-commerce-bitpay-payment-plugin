using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiquBitpayPlugin.Helpers
{
    public static class TemplateExtensions
    {
        /// <summary>
        /// Checks if a specific template derives from another template.
        /// https://laubplusco.net/sitecore-extensions-does-a-sitecore-item-derive-from-a-template/
        /// </summary>
        /// <param name="template"></param>
        /// <param name="templateId"></param>
        /// <returns></returns>
        public static bool IsDerived([NotNull] this Template template, [NotNull] ID templateId)
        {
            return template.ID == templateId || template.GetBaseTemplates().Any(baseTemplate => IsDerived(baseTemplate, templateId));
        }
    }
}