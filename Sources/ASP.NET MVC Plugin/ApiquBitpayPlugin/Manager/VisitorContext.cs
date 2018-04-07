using Sitecore.Analytics;
using Sitecore.Commerce.Entities.Customers;
using Sitecore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiquBitpayPlugin.Manager
{
    public class VisitorContext
    {
        private const string StaticVisitorId = "{74E29FDC-8523-4C4F-B422-23BBFF0A342A}";        // Just a random id
        private string _userId = string.Empty;
        private CommerceUser _commerceUser;

        public VisitorContext()
        {
            if (Sitecore.Context.User != null)
            {
                var commerceUser = AccountManager.GetUser(Sitecore.Context.User.Name);
                if (commerceUser != null)
                {   // If the user is logged in and is a commerce user we are going to set the commerce user as the user
                    SetCommerceUser(commerceUser);
                }
            }
        }

        /// <summary>
        /// Resolve the CommerceUser from the Visitor
        /// </summary>
        /// <param name="user">The user.</param>
        public void SetCommerceUser(CommerceUser user)
        {
            if (Tracker.Current == null || Tracker.Current.Contact == null || Tracker.Current.Contact.ContactId == Guid.Empty)
            {
                // This only occurs if we are authenticated but there is no ExternalUser assigned.
                // This happens in preview mode we want to supply the default user to use in Preview mode
                // Tracker.Visitor.ExternalUser = "1";
                return;
            }

            this._commerceUser = user;

            Assert.IsNotNull(this._commerceUser.Customers, "The user '{0}' does not contain a Customers collection.", user.UserName);

            this._userId = this._commerceUser.Customers.FirstOrDefault();
        }

        /// <summary>
        /// Gets the current customer Id
        /// </summary>
        /// <returns>the id</returns>
        public string GetCustomerId()
        {
            // Use the VisitorId if we have not set a UserId
            if (string.IsNullOrEmpty(this._userId))
            {
                return this.VisitorId;
            }

            return this._userId;
        }

        /// <summary>
        /// Gets the visitor id.
        /// </summary>
        /// <value>The visitor id.</value>
        private string VisitorId
        {
            get
            {
                if (Tracker.Current != null && Tracker.Current.Contact != null && Tracker.Current.Contact.ContactId != Guid.Empty)
                {
                    return Tracker.Current.Contact.ContactId.ToString();
                }

                // Generate our own tracking id if needed for the experience editor.
                if (Sitecore.Context.PageMode.IsExperienceEditor)
                {
                    return GetExperienceEditorVisitorTrackingId();
                }

                throw new Exception("Tracking not enabled");
            }
        }

        /// <summary>
        /// Gets a visitor tracking ID when in the Experience Editor.
        /// </summary>
        /// <returns>the id of this visitor</returns>
        private static string GetExperienceEditorVisitorTrackingId()
        {
            return StaticVisitorId;
        }
    }
}