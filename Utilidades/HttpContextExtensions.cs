using Microsoft.EntityFrameworkCore;

namespace tickets.Utilidades
{
    public static class HttpContextExtensions
    {
        public async static Task InsertaPaginacionEnCabecera<T>(this HttpContext httpContext,
            IQueryable<T> queryable)
        {
            if(httpContext== null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }
            double cantidad = await queryable.CountAsync();
            httpContext.Response.Headers.Add("total",cantidad.ToString());
        }
    }
}
