using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace BtVideo.Helpers
{
    public class HttpErrorModule : IHttpModule
    {
        private void Context_Error(object sender, EventArgs e)
        {
            HttpContext context = ((HttpApplication)sender).Context;

            int statusCode = ((HttpException)context.Error).GetHttpCode();

            if ((object.ReferenceEquals(context.Error.GetType(), typeof(HttpException))) && (statusCode == 404 || statusCode == 500))
            {
                // Get the Web application configuration. 
                System.Configuration.Configuration configuration = WebConfigurationManager.OpenWebConfiguration("~/web.config");
                // Get the section. 
                CustomErrorsSection customErrorsSection = (CustomErrorsSection)configuration.GetSection("system.web/customErrors");
                // Get the collection 
                CustomErrorCollection customErrorsCollection = customErrorsSection.Errors;


                //Clears existing response headers and sets the desired ones. 
                context.Response.ClearHeaders();
                context.Response.StatusCode = statusCode;

                if ((customErrorsCollection.Get(statusCode.ToString()) != null))
                {
                    context.Server.Transfer(customErrorsCollection.Get(statusCode.ToString()).Redirect);
                }

                else
                {
                    context.Response.Flush();
                }
            }
        }

        #region IHttpModule Members

        public void Dispose()
        {
            //Do nothing here 
        }

        public void Init(HttpApplication context)
        {
            context.Error += new EventHandler(Context_Error);
        }

        #endregion
    }
}