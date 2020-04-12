using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using DLL.Model;
using DLL.Model.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Utility.Helpers;

namespace DLL.DbContext
{
    public class ApplicationDbContext : IdentityDbContext<AppUser, AppRole, long,
        IdentityUserClaim<long>, AppUserRole, IdentityUserLogin<long>,
        IdentityRoleClaim<long>, IdentityUserToken<long>>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApplicationDbContext(DbContextOptions options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Department> Departments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(ISoftDeletable).IsAssignableFrom(entity.ClrType) == true)
                {
                    entity.AddProperty(IsDeletedProperty, typeof(bool));

                    modelBuilder
                        .Entity(entity.ClrType)
                        .HasQueryFilter(GetIsDeletedRestriction(entity.ClrType));
                }
            }
            base.OnModelCreating(modelBuilder);
        }
        
        private const string IsDeletedProperty = "IsDeleted";
        private static readonly MethodInfo _propertyMethod = typeof(EF).GetMethod(nameof(EF.Property), BindingFlags.Static | BindingFlags.Public)?.MakeGenericMethod(typeof(bool));

        private static LambdaExpression GetIsDeletedRestriction(Type type)
        {
            var parm = Expression.Parameter(type, "it");
            var prop = Expression.Call(_propertyMethod, parm, Expression.Constant(IsDeletedProperty));
            var condition = Expression.MakeBinary(ExpressionType.Equal, prop, Expression.Constant(false));
            var lambda = Expression.Lambda(condition, parm);
            return lambda;
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSaving();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
        {
            OnBeforeSaving();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void OnBeforeSaving()
        {
            var entries = ChangeTracker.Entries();
            var userName = GetCurrentUserName();
            foreach (var entry in entries)
            {
                if (entry.Entity is ITrackable trackable)
                {
                    var now = DateTime.UtcNow;

                    switch (entry.State)
                    {
                        case EntityState.Added:
                            trackable.CreatedAt = now;
                            trackable.CreatedBy = userName;
                            trackable.UpdatedAt = now;
                            trackable.UpdatedBy = userName;
                            break;
                        case EntityState.Modified:
                            trackable.UpdatedAt = now;
                            trackable.UpdatedBy = userName;
                            break;
                        case EntityState.Deleted:
                            entry.Property(IsDeletedProperty).CurrentValue = true;
                            entry.State = EntityState.Modified;
                            break;
                    }
                }
            }
        }

        private string GetCurrentUserName()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null)
                return httpContext.User.Claims.Where(c => c.Type == CustomJwtClaimNames.UserName)
                    .Select(c => c.Value).SingleOrDefault();
            return null;
        }
    }
}