using Microsoft.AspNet.Identity.EntityFramework;
using ParkBlog.Data.Contracts;
using ParkBlog.Data.Migrations;
using ParkBlog.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Reflection;

namespace ParkBlog.Data
{
    public class DataContext : IdentityDbContext<User, CustomRole, int, CustomUserLogin, CustomUserRole, CustomUserClaim>, IQueryableContext
    {
        //private readonly DataContext _context;

        static DataContext()
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<DataContext>());
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DataContext, Configuration>());
        }

        public DataContext()
            : base("DataConnection")
        { }

        #region Dbsets

        public DbSet<BasePost> Posts { get; set; }

        public DbSet<Profile> Profiles { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<UserVotedArticle> UserVotedArticles { get; set; }

        #endregion Dbsets

        #region IQueryableContext members

        public DataContext Context
        {
            get { return this; }
        }

        public void Attach<TEntity>(TEntity item) where TEntity : class
        {
            var entry = Entry(item);

            //attach and set as unchanged
            if (entry.State != EntityState.Unchanged)
                Entry(item).State = EntityState.Unchanged;
        }

        public void SetModified<TEntity>(TEntity item, int id) where TEntity : class
        {
            //this operation also attach item in object state manager
            var entityInDb = Set<TEntity>().Find(id);
            Entry(entityInDb).CurrentValues.SetValues(item);
            Entry(entityInDb).State = EntityState.Modified;
        }

        public void ApplyCurrentValues<TEntity>(TEntity original, TEntity current) where TEntity : class
        {
            //if it is not attached, attach original and set current values
            Entry(original).CurrentValues.SetValues(current);
        }

        public IEnumerable<TEntity> ExecuteQuery<TEntity>(string sqlQuery, params object[] parameters)
        {
            return Database.SqlQuery<TEntity>(sqlQuery, parameters);
        }

        public int ExecuteCommand(string sqlCommand, params object[] parameters)
        {
            return Database.ExecuteSqlCommand(sqlCommand, parameters);
        }

        public void Commit()
        {
            base.SaveChanges();
        }

        public void CommitAndRefreshChanges()
        {
            bool saveFailed;

            // If during save concurrency exception has occured, save changes with new values
            do
            {
                try
                {
                    base.SaveChanges();
                    saveFailed = false;
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    saveFailed = true;
                    ex.Entries.ToList().ForEach(entry => entry.OriginalValues.SetValues(entry.GetDatabaseValues()));
                }
            } while (saveFailed);
        }

        public void RollbackChanges()
        {
            // Set all entities in change tracker as 'unchanged state'
            ChangeTracker.Entries().ToList().ForEach(entry => entry.State = EntityState.Unchanged);
        }

        #endregion IQueryableContext members

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            RegisterAllMaps(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }

        #region Private methods

        private static void RegisterAllMaps(DbModelBuilder modelBuilder)
        {
            var typesToRegister = Assembly.GetExecutingAssembly()
                                          .GetTypes()
                                          .Where(type => !string.IsNullOrWhiteSpace(type.Namespace))
                                          .Where(
                                              type =>
                                              (type.BaseType != null && type.BaseType.IsGenericType &&
                                              type.BaseType.GetGenericTypeDefinition() ==
                                              typeof(EntityTypeConfiguration<>)) ||
                                              (type.BaseType != null && type.BaseType.IsGenericType &&
                                              type.BaseType.GetGenericTypeDefinition() ==
                                              typeof(ComplexTypeConfiguration<>))
                                              );
            foreach (dynamic configurationInstance in typesToRegister.Select(Activator.CreateInstance))
            {
                modelBuilder.Configurations.Add(configurationInstance);
            }
        }

        #endregion Private methods
    }
}