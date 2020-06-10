using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using Owin.Security.Providers.LinkedIn;

[assembly: OwinStartup(typeof(CalendarWebDemo.StartupOwin))]

namespace CalendarWebDemo
{
    public partial class StartupOwin
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login")
            });
            // Use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);
         
            app.UseLinkedInAuthentication(new LinkedInAuthenticationOptions() { ClientId = "86r5ibq7rosr97", ClientSecret = "iGs11czgu4ByFndn" });
            
        }
    }
}
