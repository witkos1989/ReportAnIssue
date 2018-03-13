using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ReportAnIssue.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public bool OneTimePassword { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var UserIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            UserIdentity.AddClaim(new Claim("Issues", "0"));
            UserIdentity.AddClaim(new Claim("Comments", "0"));
            UserIdentity.AddClaim(new Claim("Types", "0"));
            return UserIdentity;
        }

        public ICollection<Issue> Issues { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Type> Types { get; set; }
    }
}