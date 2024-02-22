using JBMDatabase.Enum;
using JBMDatabase.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace JBMDatabase
{
    public static class ConfigDbContextOptions
    {
        public static DbContextOptions<TContext> GetDbContextOptions<TContext>(this IServiceCollection services,
                                                                       DatabaseOptions database,
                                                                       string connectionString)
                                                                       where TContext : DbContext
        {
            DbContextOptions<TContext>? options = null;

            if (ConnectionStringValidation.IsValid(connectionString))
            {
                options = database switch
                {
                    DatabaseOptions.InMemoryDatabase => new DbContextOptionsBuilder<TContext>().UseInMemoryDatabase(connectionString).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking).Options,
                    DatabaseOptions.SqlServer => new DbContextOptionsBuilder<TContext>().UseSqlServer(connectionString).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking).Options,
                    DatabaseOptions.Sqlite => new DbContextOptionsBuilder<TContext>().UseSqlite(connectionString).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking).Options,
                    _ => throw new ArgumentException($"{nameof(DatabaseOptions)} not find", nameof(DatabaseOptions)),
                };
            }

            return options;
        }

        public static DbContextOptions<TContext> GetDbContextOptions<TContext>(DatabaseOptions database,
                                                                       string connectionString)
                                                                       where TContext : DbContext
        {
            return new ServiceCollection().GetDbContextOptions<TContext>(database, connectionString); ;
        }

        public static DbContextOptions<TContext> GetDbContextOptions<TContext>(this DbContextOptions<TContext> options,
                                                                                    DatabaseOptions database,
                                                                                    string connectionString)
                                                              where TContext : DbContext
        {
            options = new ServiceCollection().GetDbContextOptions<TContext>(database, connectionString);
            return options;
        }

    }
}
