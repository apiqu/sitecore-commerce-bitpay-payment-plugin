using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BitPayAPI;
using Newtonsoft.Json;

namespace ApiquBitpayPlugin.Tests
{
    [TestClass]
    public class BitpayTest
    {
        private BitPay bitpay;
        private Invoice basicInvoice;

        private static String pairingCode = "BArizGy";
        private static String clientName = "BitPay C# Library Tester on " + System.Environment.MachineName;
        public BitpayTest()
        {
            try
            {
                
                bitpay = new BitPay(clientName, "https://test.bitpay.com/");

                if (!bitpay.clientIsAuthorized(BitPay.FACADE_POS))
                {
                   bitpay.authorizeClient(pairingCode);
                }
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public void testShouldGetInvoiceId()
        {
            try
            {
                basicInvoice = bitpay.createInvoice(new Invoice(50.0, "USD")
                {
                    RedirectURL = "https://bitpay.com/api"
                });
                Assert.IsNotNull(basicInvoice.Id, "Invoice created with id=NULL");
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public void testShouldGetInvoiceURL()
        {
            try
            {
                basicInvoice = bitpay.createInvoice(new Invoice(50.0, "USD"));
                Assert.IsNotNull(basicInvoice.Url, "Invoice created with url=NULL");
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public void testShouldGetInvoiceStatus()
        {
            try
            {
                basicInvoice = bitpay.createInvoice(new Invoice(50.0, "USD"));
                Assert.AreEqual(Invoice.STATUS_NEW, basicInvoice.Status, "Status is incorrect");
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public void testShouldGetInvoiceBTCPrice()
        {
            try
            {
                basicInvoice = bitpay.createInvoice(new Invoice(50.0, "USD"));
                Assert.IsNotNull(basicInvoice.BtcPrice, "Invoice created with btcPrice=NULL");
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public void testShouldCreateInvoiceOneTenthBTC()
        {
            try
            {
                Invoice invoice = this.bitpay.createInvoice(new Invoice(0.1, "BTC"));
                Assert.AreEqual(0.1, invoice.BtcPrice, 0.0000001, "Invoice not created correctly: 0.1BTC");
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public void testShouldCreateInvoice100USD()
        {
            try
            {
                Invoice invoice = this.bitpay.createInvoice(new Invoice()
                {
                    Price = 10.0,
                    Currency = "USD",
                    NotificationURL = "https://testwebhooks.com/c/ninhdt",
                    RedirectURL = "https://testwebhooks.com/c/ninhdt",
                    
                });
                var json = JsonConvert.SerializeObject(invoice);
                Assert.AreEqual(100.0, invoice.Price, "Invoice not created correctly: 100USD");
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public void testShouldCreateInvoice100EUR()
        {
            try
            {
                Invoice invoice = this.bitpay.createInvoice(new Invoice(100.0, "EUR"));
                Assert.AreEqual(100.0, invoice.Price, "Invoice not created correctly: 100EUR");
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public void testShouldGetInvoice()
        {
            try
            {
                Invoice invoice = this.bitpay.createInvoice(new Invoice(100.0, "EUR"));
                Invoice retreivedInvoice = this.bitpay.getInvoice(invoice.Id);
                Assert.AreEqual(invoice.Id, retreivedInvoice.Id, "Expected invoice not retreived");
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public void testShouldCreateInvoiceWithAdditionalParams()
        {
            try
            {
                Invoice invoice = new Invoice(100.0, "USD");
                invoice.BuyerName = "Satoshi";
                invoice.BuyerEmail = "satoshi@bitpay.com";
                invoice.FullNotifications = true;
                invoice.NotificationEmail = "satoshi@bitpay.com";
                invoice.PosData = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
                invoice = this.bitpay.createInvoice(invoice);
                var json = JsonConvert.SerializeObject(invoice);
                Assert.AreEqual(Invoice.STATUS_NEW, invoice.Status, "Status is incorrect");
                Assert.AreEqual("Satoshi", invoice.BuyerName, "BuyerName is incorrect");
                Assert.AreEqual("satoshi@bitpay.com", invoice.BuyerEmail, "BuyerEmail is incorrect");
                Assert.AreEqual(true, invoice.FullNotifications, "FullNotifications is incorrect");
                Assert.AreEqual("satoshi@bitpay.com", invoice.NotificationEmail, "NotificationEmail is incorrect");
                Assert.AreEqual("ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890", invoice.PosData, "PosData is incorrect");
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public void testShouldGetExchangeRates()
        {
            try
            {
                Rates rates = this.bitpay.getRates();
                Assert.IsNotNull(rates.getRates(), "Exchange rates not retrieved");
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public void testShouldGetUSDExchangeRate()
        {
            Rates rates = this.bitpay.getRates();
            Assert.IsTrue(rates.getRate("USD") != 0, "Exchange rate not retrieved: USD");
        }

        [TestMethod]
        public void testShouldGetEURExchangeRate()
        {
            Rates rates = this.bitpay.getRates();
            Assert.IsTrue(rates.getRate("EUR") != 0, "Exchange rate not retrieved: EUR");
        }

        [TestMethod]
        public void testShouldGetCNYExchangeRate()
        {
            Rates rates = this.bitpay.getRates();
            Assert.IsTrue(rates.getRate("CNY") != 0, "Exchange rate not retrieved: CNY");
        }

        [TestMethod]
        public void testShouldUpdateExchangeRates()
        {
            Rates rates = this.bitpay.getRates();
            rates.update();
            Assert.IsNotNull(rates.getRates(), "Exchange rates not retrieved after update");
        }
    }
}
