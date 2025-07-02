using Microsoft.EntityFrameworkCore;

namespace SimpleProde.Utilities
{
    public static class HttpContextExtensions
    {
        public async static Task InsertPaginationParametersInHeader<T>(this HttpContext httpContext,
            IQueryable<T> queryable)
        {
            if (httpContext is null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            double cantidad = await queryable.CountAsync();
            httpContext.Response.Headers.Append("total-amount-of-records", cantidad.ToString());
        }

        public static void InsertPaginationParametersInHeaderByCounter(this HttpContext httpContext,
            int quantity)
        {
            if (httpContext is null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            httpContext.Response.Headers.Append("total-amount-of-records", quantity.ToString());
        }
    }
}
