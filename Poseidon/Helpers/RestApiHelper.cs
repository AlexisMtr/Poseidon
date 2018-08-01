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

        public static string GetNextPageUrl(this HttpContext context, int currentPage, int rowsPerPage, bool hasNextPage = true)
        {
            string scheme = context.Request.Scheme;
            string host = context.Request.Host.Host;
            string port = context.Request.Host.Port?.ToString();
            string apiCall = context.Request.Path.Value;

            return hasNextPage ? $"{scheme}://{host}:{port}{apiCall}?rowsPerPage={rowsPerPage}&pageNumber={currentPage + 1}" : null;
        }
    }
}
