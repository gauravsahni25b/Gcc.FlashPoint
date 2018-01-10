using System;
using System.Diagnostics;
using System.Linq;
using FluentAssertions;
using Gcc.FlashPoint.Core.BusinessLogic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gcc.FlashPoint.Tests
{
    [TestClass]
    public class UrlCreatorForMassHttpGetsTests
    {
        [TestMethod]
        public void OneElement丨JsonArray丨JsonObjectProperties丨ConvertedToUrlSegments()
        {
            //Arrange
            string dataForUrls = @"[{
              'Name': 'Zeus',
              'Job': 'God',
              'Title': 'ThunderGod',
              'Address': 'MountOlympus'
            }]";

            //Act
            UrlGeneratorForMassHttpGets urlCreator = new UrlGeneratorForMassHttpGets("localhost:3000", dataForUrls, null);

            //Assert
            urlCreator.Urls.Should().HaveCount(1);
            urlCreator.Urls.First().Should().Be("localhost:3000/Name/Zeus/Job/God/Title/ThunderGod/Address/MountOlympus");

            //Output
            Debug.WriteLine(urlCreator.Urls.First());
        }

        [TestMethod]
        public void OneElement丨JsonArray丨JsonObjectProperties丨ConvertedToQueryStringParams()
        {
            //Arrange
            string dataForQueryString = @"[{
              'Name': 'Zeus',
              'Job': 'God',
              'Title': 'ThunderGod',
              'Address': 'MountOlympus'
            }]";

            //Act
            UrlGeneratorForMassHttpGets urlCreator = new UrlGeneratorForMassHttpGets("localhost:3000", null, dataForQueryString);

            //Assert
            urlCreator.Urls.Should().HaveCount(1);
            urlCreator.Urls.First().Should().Be("localhost:3000?Name=Zeus&Job=God&Title=ThunderGod&Address=MountOlympus");

            //Output
            Debug.WriteLine(urlCreator.Urls.First());
        }

        [TestMethod]
        public void OneElement丨JsonArray丨JsonObjectProperties丨ConvertedToUrlSegments丨ConvertedToQueryStringParams()
        {
            //Arrange
            string data = @"[{
              'Name': 'Zeus',
              'Job': 'God',
              'Title': 'ThunderGod',
              'Address': 'MountOlympus'
            }]";

            //Act
            UrlGeneratorForMassHttpGets urlCreator = new UrlGeneratorForMassHttpGets("localhost:3000", data, data);

            //Assert
            urlCreator.Urls.Should().HaveCount(1);
            urlCreator.Urls.First().Should().Be("localhost:3000/Name/Zeus/Job/God/Title/ThunderGod/Address/MountOlympus?Name=Zeus&Job=God&Title=ThunderGod&Address=MountOlympus");

            //Output
            Debug.WriteLine(urlCreator.Urls.First());
        }

        [TestMethod]
        public void MultiElement丨JsonArray丨JsonObjectProperties丨ConvertedToUrlSegments丨ConvertedToQueryStringParams丨AllTestDataCasesInThisOne()
        {
            //Arrange
            string dataForUrlSegment = @"[
            {
              'Name': 'Zeus',
              'Job': 'God',
              'Title': 'ThunderGod',
              'Address': 'MountOlympus'
            },
            {
              'Name': 'Zeus',
              'Job': 'God',
              'Title': 'ThunderGod',
              'Address': 'MountOlympus'
            },
            {
              'Name': 'Zeus',
              'Job': 'God',
              'Title': 'ThunderGod',
              'Address': 'MountOlympus'
            },
            {
              'Name': 'Zeus',
              'Job': 'God',
              'Title': 'ThunderGod',
              'Address': 'MountOlympus'
            },
            {
              'Name': 'Zeus',
              'Job': 'God',
              'Title': 'ThunderGod',
              'Address': 'MountOlympus'
            },
            {
            }
            ]";

            string dataForQueryString = @"[
              {
              'Name': 'Zeus',
              'Job': 'God',
              'Title': 'ThunderGod',
              'Address': 'MountOlympus'
            },
            {
              'Name': 'Zeus',
              'Job': 'God',
              'Title': 'ThunderGod',
              'Address': 'MountOlympus'
            },
            {
            },
            {
              'Name': 'Zeus',
              'Job': 'God',
              'Title': 'ThunderGod',
              'Address': 'MountOlympus'
            },
            {
            },
            {
              'Name': 'Zeus',
              'Job': 'God',
              'Title': 'ThunderGod',
              'Address': 'MountOlympus'
            }
            ]";

            //Act
            UrlGeneratorForMassHttpGets urlCreator = new UrlGeneratorForMassHttpGets("localhost:3000", dataForUrlSegment, dataForQueryString);

            //Assert
            urlCreator.Urls.Should().HaveCount(6);
            var urlsArray = urlCreator.Urls.ToArray();
            urlsArray[0].Should().Be("localhost:3000/Name/Zeus/Job/God/Title/ThunderGod/Address/MountOlympus?Name=Zeus&Job=God&Title=ThunderGod&Address=MountOlympus");
            urlsArray[1].Should().Be("localhost:3000/Name/Zeus/Job/God/Title/ThunderGod/Address/MountOlympus?Name=Zeus&Job=God&Title=ThunderGod&Address=MountOlympus"); // 0 == 1
            urlsArray[2].Should().Be("localhost:3000/Name/Zeus/Job/God/Title/ThunderGod/Address/MountOlympus"); // Only UrlSegment
            urlsArray[3].Should().Be("localhost:3000/Name/Zeus/Job/God/Title/ThunderGod/Address/MountOlympus?Name=Zeus&Job=God&Title=ThunderGod&Address=MountOlympus"); // 0 == 1 == 3
            urlsArray[4].Should().Be("localhost:3000/Name/Zeus/Job/God/Title/ThunderGod/Address/MountOlympus"); // 2 == 4
            urlsArray[5].Should().Be("localhost:3000?Name=Zeus&Job=God&Title=ThunderGod&Address=MountOlympus"); // Only Query String

            //Output
            urlCreator.Urls.ForEach(a => Debug.WriteLine(a));
        }
    }
}
