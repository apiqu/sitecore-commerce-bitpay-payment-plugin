using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiquBitpayPlugin.Models
{
    
    public class BuyerFields
    {
        public string buyerEmail { get; set; }
        public bool buyerNotify { get; set; }
    }

    public class PaymentSubtotals
    {
        public int BCH { get; set; }
        public int BTC { get; set; }
    }

    public class PaymentTotals
    {
        public int BCH { get; set; }
        public int BTC { get; set; }
    }

    public class BTC
    {
        public double USD { get; set; }
        public double BCH { get; set; }
    }

    public class BCH
    {
        public double USD { get; set; }
        public double BTC { get; set; }
    }

    public class ExchangeRates
    {
        public BTC BTC { get; set; }
        public BCH BCH { get; set; }
    }

    public class BitpayRequest
    {
        public string id { get; set; }
        public string url { get; set; }
        public string posData { get; set; }
        public string status { get; set; }
        public string btcPrice { get; set; }
        public string price { get; set; }
        public string currency { get; set; }
        public long invoiceTime { get; set; }
        public long expirationTime { get; set; }
        public long currentTime { get; set; }
        public string btcPaid { get; set; }
        public string btcDue { get; set; }
        public double rate { get; set; }
        public bool exceptionStatus { get; set; }
        public BuyerFields buyerFields { get; set; }
        public PaymentSubtotals paymentSubtotals { get; set; }
        public PaymentTotals paymentTotals { get; set; }
        public string transactionCurrency { get; set; }
        public string amountPaid { get; set; }
        public ExchangeRates exchangeRates { get; set; }
    }
}