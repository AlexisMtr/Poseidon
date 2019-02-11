using Microsoft.AspNetCore.Http;
using System;
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

            string scheme = context.Request.Scheme;
            string host = context.Request.Host.Host;
            string port = context.Request.Host.Port?.ToString();
            string apiCall = context.Request.Path.Value;

            bool containPageNumber = context.Request.Query.Any(e => string.Compare(e.Key, "pageNumber", true) == 0);
            string[] queryParams = new string[containPageNumber ? context.Request.Query.Count : context.Request.Query.Count + 1];
            int i = 0;
            foreach (var param in context.Request.Query)
            {
                string value = param.Value.ToString();
                if (string.Compare(param.Key, "pageNumber", true) == 0)
                    value = (int.Parse(param.Value) + 1).ToString();
                queryParams[i] = $"{param.Key}={value}";
                i++;
            }
            if (!containPageNumber) queryParams[i] = $"pageNumber=2";


            return $"{scheme}://{host}:{port}{apiCall}?{string.Join("&", queryParams)}";
        }

        public static string GetPreviousPageUrl(this HttpContext context, bool hasPreviousPage = false)
        {
            if (!hasPreviousPage) return null;

            string scheme = context.Request.Scheme;
            string host = context.Request.Host.Host;
            string port = context.Request.Host.Port?.ToString();
            string apiCall = context.Request.Path.Value;

            bool containPageNumber = context.Request.Query.Any(e => string.Compare(e.Key, "pageNumber", true) == 0);
            string[] queryParams = new string[containPageNumber ? context.Request.Query.Count : context.Request.Query.Count + 1];
            int i = 0;
            foreach (var param in context.Request.Query)
            {
                string value = param.Value.ToString();
                if (string.Compare(param.Key, "pageNumber", true) == 0)
                    value = (int.Parse(param.Value) - 1).ToString();
                queryParams[i] = $"{param.Key}={value}";
                i++;
            }


            return $"{scheme}://{host}:{port}{apiCall}?{string.Join("&", queryParams)}";
        }

        public static string GetAcknowledgmentAlarmUri(this HttpContext context, int alarmId)
        {
            string scheme = context.Request.Scheme;
            string host = context.Request.Host.Host;
            string port = context.Request.Host.Port?.ToString();

            return $"{scheme}://{host}:{port}/api/alarms/{alarmId}/ack";
        }
    }
}
