using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Gcc.FlashPoint.Core.BusinessLogic
{
    public class UrlCreatorForHttpGet
    {
        public string Url { get; protected set; }

        public UrlCreatorForHttpGet(string url, JObject jsonDataForUrlSegments, JObject jsonDataForQueryStrings)
        {
            Url = url;
            if (jsonDataForUrlSegments != null)
            {
                ExtendUrl(jsonDataForUrlSegments, UrlParameterAppendOperationType.UrlSegment); 
            }
            if (jsonDataForQueryStrings != null)
            {
                ExtendUrl(jsonDataForQueryStrings, UrlParameterAppendOperationType.QueryString);
            }
        }

        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        private void ExtendUrl(JObject dataObjct, UrlParameterAppendOperationType operationType)
        {
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
