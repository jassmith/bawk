using System;
using System.Web.Http;
using CodeRinseRepeat.Chicken;
using Microsoft.Owin;
using Microsoft.Owin.Diagnostics;
using Newtonsoft.Json.Serialization;
using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace CodeRinseRepeat.Chicken
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseErrorPage(new ErrorPageOptions
            {
                ShowCookies = true,
                ShowEnvironment = true,
                ShowExceptionDetails = true,
                ShowHeaders = true,
                ShowQuery = true,
                ShowSourceCode = true,
                SourceCodeLineCount = 20,
            });

            app.UseWelcomePage("/");

            var config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            app.UseWebApi(config);
        }
    }
}