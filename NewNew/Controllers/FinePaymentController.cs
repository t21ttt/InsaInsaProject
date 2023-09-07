using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using PayPal.Api;
using System;
using System.Collections.Generic;
using NewNew.Models;

namespace NewNew.Controllers
{
    public class FinePaymentController : Controller
    {
        private readonly ILogger<FinePaymentController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public FinePaymentController(ILogger<FinePaymentController> logger, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        public ActionResult PaymentWithPaypal(string Cancel = null, string blogId = "", string guid = "")
        {
            var clientID = _configuration.GetValue<string>("PayPal:ClientID");
            var clientSecret = _configuration.GetValue<string>("PayPal:ClientSecret");
            var mode = _configuration.GetValue<string>("PayPal:Mode");

            APIContext apiContext = PaypalConfiguration.GetAPIContext(clientID, clientSecret, mode);

            try
            {
                string payerId = Request.Query["PayerID"];
                if (string.IsNullOrEmpty(payerId))
                {
                    string baseURI = $"{Request.Scheme}://{Request.Host}/Home/PaymentWithPayPay?";
                    guid = Guid.NewGuid().ToString();
                    var createdPayment = CreatePayment(apiContext, baseURI + "guid=" + guid, blogId);
                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = null;
                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;
                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            paypalRedirectUrl = lnk.href;
                        }
                    }
                    _httpContextAccessor.HttpContext.Session.SetString("payment", createdPayment.id);
                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    var paymentId = _httpContextAccessor.HttpContext.Session.GetString("payment");
                    var executedPayment = ExecutePayment(apiContext, payerId, paymentId);
                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return View("PaymentFailed");
                    }
                    var blogIds = executedPayment.transactions[0].item_list.items[0].sku;
                    return View("PaymentSuccess");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during PayPal payment.");
                return View("PaymentFailed");
            }
        }

        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerId
            };
            var payment = new Payment()
            {
                id = paymentId
            };
            return payment.Execute(apiContext, paymentExecution);
        }

        private Payment CreatePayment(APIContext apiContext, string redirectUrl, string blogId)
        {
            var itemList = new ItemList()
            {
                items = new List<Item>()
                {
                    new Item()
                    {
                        name = "Item Detail",
                        currency = "USD",
                        price = "1.00",
                        quantity = "1",
                        sku = "asd"
                    }
                }
            };

            var payer = new Payer()
            {
                payment_method = "paypal"
            };

            var redirectUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl + "&Cancel=true",
                return_url = redirectUrl
            };

            var amount = new Amount()
            {
                currency = "USD",
                total = "1.00",
            };

            var transactionList = new List<Transaction>()
            {
                new Transaction()
                {
                    description = "Transaction description",
                    invoice_number = Guid.NewGuid().ToString(),
                    amount = amount,
                    item_list = itemList
                }
            };

            var payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirectUrls
            };

            return payment.Create(apiContext);
        }
    }
}
