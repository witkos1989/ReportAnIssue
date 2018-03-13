using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ReportAnIssue.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace ReportAnIssue.Extensions
{
    public static class IdentityExtensions
    {
        public static bool OneTimePassword(this IIdentity identity)
        {
            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new UserManager<ApplicationUser>(store);
            ApplicationUser user = userManager.FindByNameAsync(identity.Name).Result;
            if (user == null)
                return false;
            else
                return user.OneTimePassword;
        }
        public static string GetIssues(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("Issues");
            return (claim != null) ? claim.Value : string.Empty;
        }
        public static string GetComments(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("Comments");
            return (claim != null) ? claim.Value : string.Empty;
        }
        public static string GetTypes(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("Types");
            return (claim != null) ? claim.Value : string.Empty;
        }
    }
}