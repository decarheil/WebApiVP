using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace WebApplication.Helpers
{
    public static class HttpRequestMessageExtensions
    {
        public static string GetHeaderValue(this HttpRequestMessage request, string name)
        {
            IEnumerable<string> values;
            var found = request.Headers.TryGetValues(name, out values);
            if (found)
            {
                return values.FirstOrDefault();
            }

            return null;
        }
    }
}