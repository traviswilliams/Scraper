using System.Web.Http;

#pragma warning disable 1591
namespace Scraper.Web
{
    public static class WebApiConfig
    {
        /// <summary>
        /// Register
        /// </summary>
        /// <param name="config"></param>
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();
        }
    }
}
#pragma warning restore 1591

