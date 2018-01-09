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
        public void JsonObjectProperties丨ConvertedToUrlSegments()
        {
            string dataForUrls = @"{
              'Name': 'Zeus',
              'Job': 'God',
              'Title': 'ThunderGod',
              'Address': 'MountOlympus'
            }";
            UrlCreatorForMassHttpGets urlCreator = new UrlCreatorForMassHttpGets("localhost:3000", dataForUrls, null);
            urlCreator.Url.Should().Be("localhost:3000/Name/Zeus/Job/God/Title/ThunderGod/Address/MountOlympus");
        }

        [TestMethod]
        public void JsonObjectProperties丨ConvertedToQueryStringParams()
        {
            string dataForQueryString = @"{
              'Name': 'Zeus',
              'Job': 'God',
              'Title': 'ThunderGod',
              'Address': 'MountOlympus'
            }";
            UrlCreatorForMassHttpGets urlCreator = new UrlCreatorForMassHttpGets("localhost:3000", null, dataForQueryString);
            urlCreator.Url.Should().Be("localhost:3000?Name=Zeus&Job=God&Title=ThunderGod&Address=MountOlympus");
        }

        [TestMethod]
        public void JsonObjectProperties丨ConvertedToUrlSegments丨ConvertedToQueryStringParams()
        {
            string data = @"{
              'Name': 'Zeus',
              'Job': 'God',
              'Title': 'ThunderGod',
              'Address': 'MountOlympus'
            }";
            UrlCreatorForMassHttpGets urlCreator = new UrlCreatorForMassHttpGets("localhost:3000", data, data);
            urlCreator.Url.Should().Be("localhost:3000/Name/Zeus/Job/God/Title/ThunderGod/Address/MountOlympus?Name=Zeus&Job=God&Title=ThunderGod&Address=MountOlympus");
        }
    }
}
