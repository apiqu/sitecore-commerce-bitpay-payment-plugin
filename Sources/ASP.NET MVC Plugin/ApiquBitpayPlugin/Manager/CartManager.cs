using Sitecore.Commerce.Connect.CommerceServer.Orders.Pipelines;
using Sitecore.Commerce.Entities.Carts;
using Sitecore.Commerce.Services.Carts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiquBitpayPlugin.Manager
{
    /// <summary>
    ///  The CartManager contains methods to manage the cart (retrieve, merge) for a specific customer
    /// </summary>
    public class CartManager
    {
        public const string ShoppingCartName = "Default";

        private string shopName;
        private string shoppingCartName;
        private string customerId;

        private CartServiceProvider cartServiceProvider;

        /// <summary>
        ///     Constructor for the CartManager
        /// </summary>
        /// <param name="shopName">Name of the shop</param>
        /// <param name="shoppingCartName">Name of the shopping cart. A customer could have multiple shopping carts with different names.</param>
        /// <param name="customerId">The customer id</param>
        public CartManager(string shopName, string shoppingCartName, string customerId)
        {
            this.shopName = shopName;
            this.shoppingCartName = shoppingCartName;
            this.customerId = customerId;

            cartServiceProvider = new CartServiceProvider();
        }

        /// <summary>
        ///  Retrieves the cart for specified customer and shop
        /// </summary>
        /// <returns>The cart for specified visitor and shop</returns>
        public Cart GetCart()
        {
            return GetCart(this.customerId);
        }


        /// <summary>
        ///     Retrieves the cart for specified customer id and shop
        /// </summary>
        /// <param name="customerId">Id of customer to retrieve cart of</param>
        /// <returns>Cart for specified customer id</returns>
        public Cart GetCart(string customerId)
        {
            var loadCartRequest = new LoadCartByNameRequest(shopName, shoppingCartName, customerId);

            // Get a cart, new or existing
            var cart = cartServiceProvider.LoadCart(loadCartRequest).Cart;

            return cart;
        }

        
    }
}