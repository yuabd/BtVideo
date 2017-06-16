using System.Linq;
using System.Web;
using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Infrastructure;
using BtVideo.Models.Spider;

namespace BtVideo.Models
{
    public abstract class DbAccess : IDisposable
    {
        protected SiteDataContext db { get; set; }

        public DbAccess(SiteDataContext _db = null)
        {
            if (db == null)
            {
                db = new SiteDataContext();
            }
            else
            {
                db = _db;
            }
        }

        #region IDisposable 成员

        public void Dispose()
        {
            db.Database.Connection.Close();
            db.Dispose();
            db = null;
        }

        #endregion
    }

    public class SiteDataContext : DbContext
    {
        public SiteDataContext()
            : base()
        {
            //this.Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<MovieComment> MovieComments { get; set; }
        public DbSet<MovieCategory> MovieCategories { get; set; }
        public DbSet<MovieCategoryJoin> MovieCategoryJoins { get; set; }
        public DbSet<MovieTag> MovieTags { get; set; }
        public DbSet<MovieLink> MovieLinks { get; set; }
        
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UserRoleJoin> UserRoleJoins { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }

        public DbSet<Picture> Pictures { get; set; }

        public DbSet<Links> Links { get; set; }

        public DbSet<MovieStar> MovieStars { get; set; }
        public DbSet<MovieStarJoin> MovieStarJoins { get; set; }

        public DbSet<MovieArea> MovieAreas { get; set; }

        //public DbSet<WebLink> WebLinks { get; set; }
        public DbSet<SpiderTask> SpiderTasks { get; set; }

        public DbSet<HotKeyword> HotKeywords { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            // 还可以移除对MetaData表的查询验证
            modelBuilder.Conventions.Remove<IncludeMetadataConvention>();

            base.OnModelCreating(modelBuilder);
        }
    }

    public class SiteDataContextInitializer : DropCreateDatabaseIfModelChanges<SiteDataContext>
    {
        protected override void Seed(SiteDataContext context)
        {
            // users
            var user = new User { UserID = 1, Password = "DAC177D41FD48F8627BD10D2689E8544", DateCreated = DateTime.Now, DateLastLogin = DateTime.Now };

            context.Users.Add(user);

            var roles = new List<UserRole>
            {
                new UserRole { RoleID = "Administrator" },
                new UserRole { RoleID = "Editor" },
                new UserRole { RoleID = "Customer" },
                new UserRole { RoleID = "Guest" }
            };

            roles.ForEach(m => context.UserRoles.Add(m));

            var userRoleJoin = new UserRoleJoin { UserID = 1, RoleID = "Administrator" };
            context.UserRoleJoins.Add(userRoleJoin);

            context.SaveChanges();
        }
    }
}