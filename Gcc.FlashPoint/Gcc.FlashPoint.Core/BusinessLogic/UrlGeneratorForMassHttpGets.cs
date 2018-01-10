using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Gcc.FlashPoint.Core.BusinessLogic
{
    public class UrlGeneratorForMassHttpGets
    {
        public List<string> Urls { get; protected set; } = new List<string>();

        public UrlGeneratorForMassHttpGets(string baseUrl, string jsonArrayOfUrlSegmentData, string jsonArrayOfQueryStringData)
        {
            JObject[] jsonBodiesForUrlSegments = null, jsonBodiesForQueryStrings = null;
            if (!string.IsNullOrEmpty(jsonArrayOfUrlSegmentData))
            {
                jsonBodiesForUrlSegments = JArray.Parse(jsonArrayOfUrlSegmentData).Children<JObject>().ToArray();
            }
            if (!string.IsNullOrEmpty(jsonArrayOfQueryStringData))
            {
                jsonBodiesForQueryStrings = JArray.Parse(jsonArrayOfQueryStringData).Children<JObject>().ToArray();
            }

            //Taking a design decision to match one to one
            int count = 0;
            if (jsonBodiesForUrlSegments != null && jsonBodiesForQueryStrings != null)
            {
                count = jsonBodiesForQueryStrings.Length >= jsonBodiesForUrlSegments.Length
                    ? jsonBodiesForQueryStrings.Length
                    : jsonBodiesForUrlSegments.Length;
            }
            else if (jsonBodiesForQueryStrings == null)
            {
                count = jsonBodiesForUrlSegments.Length;
            }
            else
            {
                count = jsonBodiesForQueryStrings.Length;
            }

            for (int i = 0; i < count; i++)
            {
                JObject queryStringToken = null, urlSegmentToken = null;
                if (jsonBodiesForQueryStrings != null && jsonBodiesForQueryStrings.Length > i)
                {
                    queryStringToken = jsonBodiesForQueryStrings[i];
                }
                if (jsonBodiesForUrlSegments != null &&jsonBodiesForUrlSegments.Length > i)
                {
                    urlSegmentToken = jsonBodiesForUrlSegments[i];
                }
                UrlCreatorForHttpGet urlCreator = new UrlCreatorForHttpGet(baseUrl, urlSegmentToken, queryStringToken);
                Urls.Add(urlCreator.Url);
            }
        }
    }
}