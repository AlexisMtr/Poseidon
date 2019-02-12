using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;

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
            if (!hasNextPage) return null;

            ExtractUriInfo(context, out string scheme, out string host, out string port, out string apiCall);
            string[] queryParams = RebuildUriParams(context, currentPageNumber => currentPageNumber + 1);

            return BuildUri(scheme, host, port, apiCall, queryParams);
        }

        public static string GetPreviousPageUrl(this HttpContext context, bool hasPreviousPage = false)
        {
            if (!hasPreviousPage) return null;

            ExtractUriInfo(context, out string scheme, out string host, out string port, out string apiCall);
            string[] queryParams = RebuildUriParams(context, currentPageNumber => currentPageNumber - 1);

            return BuildUri(scheme, host, port, apiCall, queryParams);
        }

        private static void ExtractUriInfo(HttpContext context, out string scheme, out string host, out string port, out string apiCall)
        {
            scheme = context.Request.Scheme;
            host = context.Request.Host.Host;
            port = context.Request.Host.Port?.ToString();
            apiCall = context.Request.Path.Value;
        }

        private static string[] RebuildUriParams(HttpContext context, Func<int, int> pageNumberFunction)
        {
            string pageNumberKey = "pageNumber";
            bool containPageNumber = context.Request.Query.Any(e => string.Compare(e.Key, pageNumberKey, true) == 0);

            string[] queryParams = new string[containPageNumber ? context.Request.Query.Count : context.Request.Query.Count + 1];
            int i = 0;
            foreach (var param in context.Request.Query)
            {
                string value = param.Value.ToString();
                if (string.Compare(param.Key, pageNumberKey, true) == 0)
                    value = pageNumberFunction.Invoke(int.Parse(param.Value)).ToString();
                queryParams[i] = $"{param.Key}={value}";
                i++;
            }

            if (!containPageNumber && pageNumberFunction.Invoke(1) >= 1) queryParams[i] = $"{pageNumberKey}=2";

            return queryParams;
        }

        private static string BuildUri(string scheme, string host, string port, string apiCall, params string[] queryParams)
            => $"{scheme}://{host}:{port}{apiCall}?{string.Join("&", queryParams)}";

        public static string GetAcknowledgmentAlarmUri(this HttpContext context, int alarmId)
        {
            string scheme = context.Request.Scheme;
            string host = context.Request.Host.Host;
            string port = context.Request.Host.Port?.ToString();

            return $"{scheme}://{host}:{port}/api/alarms/{alarmId}/ack";
        }
    }
}
