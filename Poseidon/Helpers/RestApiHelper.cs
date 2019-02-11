using Microsoft.AspNetCore.Http;
using System;

namespace Poseidon.Helpers
{
    public static class RestApiHelper
    {
        public static int GetPageCount(int totalElement, int rowsPerPage)
        {
            return (int)Math.Ceiling(totalElement / (double)rowsPerPage);
        }

        public static string GetNextPageUrl(this HttpContext context, bool hasNextPage = true)
        {
            string scheme = context.Request.Scheme;
            string host = context.Request.Host.Host;
            string port = context.Request.Host.Port?.ToString();
            string apiCall = context.Request.Path.Value;

            string[] queryParams = new string[context.Request.Query.Count];
            int i = 0;
            foreach (var param in context.Request.Query)
            {
                string value = param.Value.ToString();
                if (string.Compare(param.Key, "pageNumber", true) == 0)
                    value = (int.Parse(param.Value) + 1).ToString();
                queryParams[i] = $"{param.Key}={value}";
                i++;
            }

            return hasNextPage ? $"{scheme}://{host}:{port}{apiCall}?{string.Join("&", queryParams)}" : null;
        }
    }
}
