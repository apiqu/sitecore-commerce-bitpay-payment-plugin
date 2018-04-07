using Sitecore.Commerce.Connect.CommerceServer.Orders.Models;
using Sitecore.Commerce.Entities;
using Sitecore.Commerce.Entities.Customers;
using Sitecore.Commerce.Services.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Analytics;
using Sitecore.Diagnostics;

namespace ApiquBitpayPlugin.Manager
{
    public class AccountManager
    {
        /// <summary>
        ///     Retrieve a commerce user based on a username
        /// </summary>
        /// <param name="userName">Username to retrieve the commerce user for</param>
        /// <returns>CommerceUser or null if commerce user doesn't exist</returns>
        public static CommerceUser GetUser(string userName)
        {
            var request = new GetUserRequest(userName);
            var result = new CustomerServiceProvider().GetUser(request);
            return result?.CommerceUser;
        }

        public static CommerceCustomer GetCustomer(string userName)
        {
            var commerceUser = AccountManager.GetUser(userName);
            if (commerceUser != null)
            {
                return new CommerceCustomer { ExternalId = commerceUser.ExternalId };
            }
            else
            {
                return null;
            }
        }

        public static List<CommerceParty> GetParties(CommerceCustomer customer)
        {
            var request = new GetPartiesRequest(customer);
            var result = new CustomerServiceProvider().GetParties(request);

            if (result.Success && result.Parties != null)
            {
                var parties = result.Parties.Cast<CommerceParty>().ToList();
                return parties;
            }

            return null;
        }

        public static void AddOrUpdateParty(CommerceCustomer customer, CommerceParty commerceParty)
        {
            var parties = GetParties(customer);
            var updatedParty = parties?.Where(party => party.Name == commerceParty.Name).SingleOrDefault();

            if (updatedParty != null)
            {
                commerceParty.ExternalId = updatedParty.ExternalId;

                var updatePartiesRequest = new UpdatePartiesRequest(customer, new List<Party> { commerceParty });
                var updatePartyResult = new CustomerServiceProvider().UpdateParties(updatePartiesRequest);
            }
            else
            {
                var addPartiesRequest = new AddPartiesRequest(customer, new List<Party> { commerceParty });
                var addPartyResult = new CustomerServiceProvider().AddParties(addPartiesRequest);
            }
        }

        public static void UpdateUser(CommerceUser commerceUser)
        {
            var customerService = new CustomerServiceProvider();

            var request = new UpdateUserRequest(commerceUser);
            customerService.UpdateUser(request);
        }

    }
}