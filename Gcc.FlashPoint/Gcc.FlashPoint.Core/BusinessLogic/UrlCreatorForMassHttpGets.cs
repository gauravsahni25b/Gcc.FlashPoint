using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Gcc.FlashPoint.Core.BusinessLogic
{
    public class UrlCreatorForMassHttpGets
    {
        public string Url { get; protected set; }

        public UrlCreatorForMassHttpGets(string url, string jsonDataForUrlSegments, string jsonDataForQueryStrings)
        {
            Url = url;
            if (!string.IsNullOrEmpty(jsonDataForUrlSegments))
            {
                ExtendUrl(jsonDataForUrlSegments, UrlParameterAppendOperationType.UrlSegment); 
            }
            if (!string.IsNullOrEmpty(jsonDataForQueryStrings))
            {
                ExtendUrl(jsonDataForQueryStrings, UrlParameterAppendOperationType.QueryString);
            }
        }

        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        private void ExtendUrl(string dataForUrls, UrlParameterAppendOperationType operationType)
        {
            JObject dataObjct = JObject.Parse(dataForUrls);
            var properties = dataObjct.Properties();
            StringBuilder builder = new StringBuilder(Url);

            if (operationType == UrlParameterAppendOperationType.UrlSegment)
            {
                foreach (var property in properties)
                {
                    builder.Append($"/{property.Name}/{property.Value.ToString()}");
                }
                Url = builder.ToString();
            }
            else
            {
                var propertiesArray = properties.ToArray();
                for(int i=0; i< properties.Count(); i++)
                {
                    if (i == 0)
                    {
                        builder.Append($"?{propertiesArray[i].Name}={propertiesArray[i].Value.ToString()}");
                    }
                    else
                    {
                        builder.Append($"&{propertiesArray[i].Name}={propertiesArray[i].Value.ToString()}");
                    }
                }
                Url = builder.ToString();
            }
        }

        private enum UrlParameterAppendOperationType
        {
            UrlSegment,
            QueryString
        }
    }
}
