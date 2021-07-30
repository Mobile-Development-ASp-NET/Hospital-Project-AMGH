using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Hospital_Project.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        //Users can have many admissions 
        public ICollection<Admission> Admissions { get; set; }

        //Users can have many feedbacks 
        public ICollection<Feedback> Feedbacks { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }


        // Feedback - Detail Entities

        public DbSet<Feedback> Feedbacks { get; set; }

        // ####### Doctor - Detail Entities

        public DbSet<Department> Departments { get; set; }

        public DbSet<DoctorDetails> DoctorDetails { get; set; }
        // #######
        public DbSet<Questions> Questions { get; set; }
        public DbSet<Surveys> Surveys { get; set; }

        //Volunteer Application Entities
        public DbSet<Application> Applications { get; set; }
        public DbSet<Position> Positions { get; set; }

        //greeting card and admission entities

        public DbSet<GreetingCard> GreetingCards { get; set; }
        public DbSet<Admission> Admissions { get; set; }

        // Blog entities
        public DbSet<SubscribedUser> SubscribedUses { get; set; }
        public DbSet<Blog> Blogs { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}