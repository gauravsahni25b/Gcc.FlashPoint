using System;
using System.Diagnostics;
using System.Linq;
using FluentAssertions;
using Gcc.FlashPoint.Core.BusinessLogic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gcc.FlashPoint.Tests
{
    [TestClass]
    public class HttpCallTimeSpanCalculatorTests
    {
        [TestMethod]
        public void Get丨Request丨OnlyOnce()
        {
            HttpCallTimeSpanCalculator timeCalculator = new HttpCallTimeSpanCalculator();
            var result = timeCalculator.MeasureGetCall("http://designbuilderapi.localtest.me/system/health-check").Result;
            Debug.WriteLine($"Resultant Time : {result.TimeTaken} | HttpStatusCode : {result.StatusCode}");
            result.Should().NotBe(new TimeSpan(0));
        }

        [TestMethod]
        public void Post丨Request丨OnlyOnce()
        {
            HttpCallTimeSpanCalculator timeCalculator = new HttpCallTimeSpanCalculator();
            string data = @"{
                'customer': {
                    'customerId': '8012045',
                    'name': 'Roddie Brailsford',
                    'email': 'Roddie@homedepot.com',
                    'primaryPhone': '764-456-7890',
                    'secondaryPhone': '9876543211',
                    'address': '2525 OldFarm, Houston, TX 77079'
                },
                'store': {
                    'associate': 'Vijay R',
                    'storeAddress': '10255 Richmond Ave, Houston, TX, 77042',
                            'storeNumber': '111',
                            'storePhone': '8877995544'
                    },
                    'designs': [
                    {
                        'designGuid': '22D4A606-257D-413A-B812-45E5399FEFD4',
                            'agreementNumber': 'CT-1234'

                    }
                    ]
                }";
            var result = timeCalculator.MeasurePostCall("http://designbuilderapi.localtest.me/window-treatments/sessions", data).Result;
            Debug.WriteLine($"Resultant Time : {result.TimeTaken} | HttpStatusCode : {result.StatusCode}");
            result.Should().NotBe(new TimeSpan(0));
        }

        [TestMethod]
        public void Post丨Request丨Multiple()
        {
            HttpCallTimeSpanCalculator timeCalculator = new HttpCallTimeSpanCalculator();
            string dataArray = @"[
              {
                'customer': {
                    'customerId': '8012045',
                    'name': 'Roddie Brailsford',
                    'email': 'Roddie@homedepot.com',
                    'primaryPhone': '764-456-7890',
                    'secondaryPhone': '9876543211',
                    'address': '2525 OldFarm, Houston, TX 77079'
                },
                'store': {
                    'associate': 'Vijay R',
                    'storeAddress': '10255 Richmond Ave, Houston, TX, 77042',
                            'storeNumber': '111',
                            'storePhone': '8877995544'
                    },
                    'designs': [
                    {
                        'designGuid': '22D4A606-257D-413A-B812-45E5399FEFD4',
                            'agreementNumber': 'CT-1234'
  
                    }
                    ]
              },
              {
                'customer': {
                    'customerId': '8012045',
                    'name': 'Roddie Brailsford',
                    'email': 'Roddie@homedepot.com',
                    'primaryPhone': '764-456-7890',
                    'secondaryPhone': '9876543211',
                    'address': '2525 OldFarm, Houston, TX 77079'
                },
                'store': {
                    'associate': 'Vijay R',
                    'storeAddress': '10255 Richmond Ave, Houston, TX, 77042',
                            'storeNumber': '111',
                            'storePhone': '8877995544'
                    },
                    'designs': [
                    {
                        'designGuid': '22D4A606-257D-413A-B812-45E5399FEFD4',
                            'agreementNumber': 'CT-1234'
  
                    }
                    ]
              }
            ]";
            var result = timeCalculator.MeasureNPostCalls("http://designbuilderapi.localtest.me/window-treatments/sessions", dataArray).Result;
            result.ToList().ForEach(a =>
            {
                Debug.WriteLine($"Resultant Time : {a.TimeTaken} | HttpStatusCode : {a.StatusCode}");
            });
            result.Should().HaveCount(2);
        }

        [TestMethod]
        public void Get丨Request丨Multiple()
        {
            HttpCallTimeSpanCalculator timeCalculator = new HttpCallTimeSpanCalculator();
            var result = timeCalculator.MeasureNGetCalls("http://designbuilderapi.localtest.me/system/health-check", 2).Result;
            result.ToList().ForEach(a =>
            {
                Debug.WriteLine($"Resultant Time : {a.TimeTaken} | HttpStatusCode : {a.StatusCode}");
            });
            result.Should().HaveCount(2);
        }
    }
}
