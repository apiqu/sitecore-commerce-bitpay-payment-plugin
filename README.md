# BitPay Payment Plugin for Sitecore Commerce

[Apiqu Team](http://www.apiqu.com/) is proud to present BitPay plugin and hope it will make your customers' payment experience more secure, faster and anonymous anywhere on earth.

Accept Bitcoin and Bitcoin Cash for your business using Sitecore Commerce Solution

Receive settlement for Bitcoin and Bitcoin Cash directly to your bank account in your own currency, with zero price volatility or risk.

<div style="text-align:center" markdown="1">

![Bitpay](https://github.com/apiqu/sitecore-commerce-bitpay-payment-plugin/blob/master/Assets/bitpay_payment.png?raw=true)

</div>

## Getting Started

---

### Merchant Account Setup

* [Sign up for a BitPay testnet account](https://test.bitpay.com/dashboard/signup) - Make sure to verify your email and fill in your business information & settlement information. See "Settlement Testing" below for what information to enter.
* Accounts on test server will be auto-approved to process up to $1,000 per day and $10,000 annually in test transactions.

---

### Getting a Testnet Wallet

* Visit [bitpay.com/wallet](https://bitpay.com/wallet) and download the wallet. Available for all major mobile and desktop platforms.
* Once you have created your first standard wallet in the app, select the + icon on your wallet's home screen. Select "Create New Wallet," verify that "Personal Wallet" is selected, and provide a name for your testnet wallet.

<div style="text-align:center" markdown="1">

![Bitpay](https://github.com/apiqu/sitecore-commerce-bitpay-payment-plugin/blob/master/Assets/bitpay1.jpg?raw=true)

</div>

<div style="text-align:center" markdown="1">

![Bitpay](https://github.com/apiqu/sitecore-commerce-bitpay-payment-plugin/blob/master/Assets/bitpay2.jpg?raw=true)

</div>

* Select "Advanced Options" and activate the "Testnet" option provided.

<div style="text-align:center" markdown="1">

![Bitpay](https://github.com/apiqu/sitecore-commerce-bitpay-payment-plugin/blob/master/Assets/bitpay3.jpg?raw=true)

</div>

* Click on "Create new wallet" and verify that "Testnet" is displayed below the wallet name.

---

### Getting Testnet Coins

* Open your BitPay wallet and click on the "Receive" tab. Scroll through your wallets to find your testnet wallet. You'll see an address at which you can receive funds. Copy this address to your clipboard.

* Find a testnet faucet and paste your testnet wallet address into the request form provided. We recommend the following testnet faucet:

    * [testnet.coinfaucet.eu](https://testnet.coinfaucet.eu/)
    * If this option is unavailable, please contact your BitPay Sales Engineer.

---

### Payment for Bitpay Invoice

* Click to "Send" button and provide Recipient address

<div style="text-align:center" markdown="1">

![Bitpay](https://github.com/apiqu/sitecore-commerce-bitpay-payment-plugin/blob/master/Assets/bitpay4.jpg?raw=true)

</div>

* Choose send account and confirm payment

<div style="text-align:center" markdown="1">

![Bitpay](https://github.com/apiqu/sitecore-commerce-bitpay-payment-plugin/blob/master/Assets/bitpay5.jpg?raw=true)

</div>

<div style="text-align:center" markdown="1">

![Bitpay](https://github.com/apiqu/sitecore-commerce-bitpay-payment-plugin/blob/master/Assets/bitpay6.jpg?raw=true)

</div>

---

### Create a Pairing Code

* From Bitpay Dashboard choose "Payment Tools" in left panel and click "Point of Sale App"

<div style="text-align:center" markdown="1">

![Bitpay](https://github.com/apiqu/sitecore-commerce-bitpay-payment-plugin/blob/master/Assets/bitpay_dashboard1.jpg?raw=true)

</div>

* Click "Add New Pairing Code"

<div style="text-align:center" markdown="1">

![Bitpay](https://github.com/apiqu/sitecore-commerce-bitpay-payment-plugin/blob/master/Assets/bitpay_dashboard2.jpg?raw=true)

</div>

---

### Plugin Configurations

<div style="text-align:center" markdown="1">

![Bitpay](https://github.com/apiqu/sitecore-commerce-bitpay-payment-plugin/blob/master/Assets/bitpay_plugin1.png?raw=true)

</div>

* **Name**: Payment option name
* **TypeID**: Unique number for payment type

<div style="text-align:center" markdown="1">

![Bitpay](https://github.com/apiqu/sitecore-commerce-bitpay-payment-plugin/blob/master/Assets/bitpay_plugin2.png?raw=true)

</div>

* **Client Name**: Connection name to Bitpay server
* **Server URL**: Url of Bitpay server (https://test.bitpay.com for sandbox and https://bitpay.com for production)
* **Redirect Url**: URL to redirect back to your website after a successful purchase. Be sure to include "http://" or "https://" in the url.
* **Pairing code**: Pairing code that you create from Bitpay Dashboard

---

### Switch between Bitpay Sandbox and Production 

* **Sandbox**  
  * Server Url: https://test.bitpay.com
  * Pairing Code: paring code that created from https://test.bitpay.com
* **Production**
	* Server Url: https://bitpay.com
    * Pairing Code: paring code that created from https://bitpay.com 

---

## References

* [BitPay Documentation](https://bitpay.com/docs/testing)

## Deployment

You can simply switch between sandbox testing account and production mode by checking/unchecking "checkboxname" checkbox. 

## Built With

* [Bitcoin Sharp](david-garcia-garcia/bitcoinsharp)
* [Bouncy Castle](https://www.bouncycastle.org/csharp/index.html)

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details