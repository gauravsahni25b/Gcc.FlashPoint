using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gcc.FlashPoint.Core.BusinessLogic;

namespace Gcc.FlashPoint.ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpCallTimeSpanCalculator timeCalculator = new HttpCallTimeSpanCalculator();
            var result = timeCalculator.MeasureGetCall("http://designbuilderapi.localtest.me/system/health-check").Result;
            Console.WriteLine($"Resultant Time : {result}");
            Console.ReadKey();
            Console.WriteLine();

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

            var resultPost = timeCalculator.MeasurePostCall("http://designbuilderapi.localtest.me/window-treatments/sessions", data).Result;
            Console.WriteLine($"Resultant Time : {resultPost}");
            Console.ReadKey();
            Console.WriteLine();

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

            var resultMultiplePost = timeCalculator.MeasureNPostCalls("http://designbuilderapi.localtest.me/window-treatments/sessions", dataArray).Result;
            Console.WriteLine($"Resultant Time for N");
            resultMultiplePost.ToList().ForEach(a => Console.WriteLine(a));
            Console.ReadKey();
            Console.WriteLine();
        }
    }
}
