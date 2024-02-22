using JBMDatabase.ConnectionString;
using JBMDatabase.ConnectionString.Model;
using JBMDatabase.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JBMDatabase
{
    public static class CreateDatabase
    {
        #region Create and AddDbContext
        public static async Task<bool> EnsureCreateAsync<TContext>(this IServiceCollection services,
                                                                   string connectionString,
                                                                   DatabaseOptions database = DatabaseOptions.InMemoryDatabase,
                                                                   QueryTrackingBehavior trackingBehavior = QueryTrackingBehavior.TrackAll)
                                                                        where TContext : DbContext
        {
            return await EnsureCreateEncapsulationAsync<TContext>(services, connectionString, database, trackingBehavior);
        }

        public static async Task<TContext> EnsureCreateAndGetDbContextAsync<TContext>(this IServiceCollection services,
                                                                                       string connectionString,
                                                                                       DatabaseOptions database = DatabaseOptions.InMemoryDatabase,
                                                                                       QueryTrackingBehavior trackingBehavior = QueryTrackingBehavior.TrackAll)
                                                                                        where TContext : DbContext
        {
            return await EnsureCreateAndGetDbContextEncapsulationAsync<TContext>(services, connectionString, database, trackingBehavior);
        }

        public static TContext EnsureCreateAndGetDbContext<TContext>(this IServiceCollection services,
                                                                       string connectionString,
                                                                       DatabaseOptions database = DatabaseOptions.InMemoryDatabase,
                                                                       QueryTrackingBehavior trackingBehavior = QueryTrackingBehavior.TrackAll)
                                                                        where TContext : DbContext
        {
            return EnsureCreateAndGetDbContextEncapsulation<TContext>(services, connectionString, database, trackingBehavior);
        }

        public static async Task<bool> EnsureCreateAsync<TContext>(this IServiceCollection services,
                                                                   ConnectionStringModel connectionStringModel,
                                                                   DatabaseOptions database = DatabaseOptions.InMemoryDatabase,
                                                                   QueryTrackingBehavior trackingBehavior = QueryTrackingBehavior.TrackAll)
                                                                        where TContext : DbContext
        {
            string connectionString = connectionStringModel.CreateConnectionString();

            return await EnsureCreateEncapsulationAsync<TContext>(services, connectionString, database, trackingBehavior);
        }

        public static async Task<bool> EnsureCreateAsync<TContext>(this IServiceCollection services,
                                                                   IConfiguration configuration,
                                                                   string jsonSection,
                                                                   DatabaseOptions database = DatabaseOptions.InMemoryDatabase,
                                                                   QueryTrackingBehavior trackingBehavior = QueryTrackingBehavior.TrackAll)
                                                                       where TContext : DbContext
        {
            string connectionString = ConnectionStringSettings.CreateConnectionString(configuration, jsonSection);

            return await EnsureCreateEncapsulationAsync<TContext>(services, connectionString, database, trackingBehavior);
        }

        private static async Task<bool> EnsureCreateEncapsulationAsync<TContext>(IServiceCollection services,
                                                                                 string connectionString,
                                                                                 DatabaseOptions databaseOptions,
                                                                                 QueryTrackingBehavior trackingBehavior)
                                                                                where TContext : DbContext
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            switch (databaseOptions)
            {
                case DatabaseOptions.InMemoryDatabase:
                    services.AddDbContext<TContext>(options => options.UseInMemoryDatabase(nameof(TContext)).UseQueryTrackingBehavior(trackingBehavior));
                    break;
                case DatabaseOptions.SqlServer:
                    services.AddDbContext<TContext>(options => options.UseSqlServer(connectionString).UseQueryTrackingBehavior(trackingBehavior));
                    break;
                case DatabaseOptions.Sqlite:
                    services.AddDbContext<TContext>(options => options.UseSqlite(connectionString).UseQueryTrackingBehavior(trackingBehavior));
                    break;
                default:
                    throw new ArgumentException($"{nameof(DatabaseOptions)} not find", nameof(DatabaseOptions));
            }

            bool created = false;

            using (IServiceScope serviceScope = services.BuildServiceProvider().CreateScope())
            {
                using (DbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<TContext>())
                {
                    try
                    {
                        created = await dbContext.Database.EnsureCreatedAsync();
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
            }

            return created;
        }

        private static async Task<TContext> EnsureCreateAndGetDbContextEncapsulationAsync<TContext>(IServiceCollection services,
                                                                                                    string connectionString,
                                                                                                    DatabaseOptions databaseOptions,
                                                                                                    QueryTrackingBehavior trackingBehavior)
                                                                                                         where TContext : DbContext
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            switch (databaseOptions)
            {
                case DatabaseOptions.InMemoryDatabase:
                    services.AddDbContext<TContext>(options => options.UseInMemoryDatabase(nameof(TContext)).UseQueryTrackingBehavior(trackingBehavior));
                    break;
                case DatabaseOptions.SqlServer:
                    services.AddDbContext<TContext>(options => options.UseSqlServer(connectionString).UseQueryTrackingBehavior(trackingBehavior));
                    break;
                case DatabaseOptions.Sqlite:
                    services.AddDbContext<TContext>(options => options.UseSqlite(connectionString).UseQueryTrackingBehavior(trackingBehavior));
                    break;
                default:
                    throw new ArgumentException($"{nameof(DatabaseOptions)} not find", nameof(DatabaseOptions));
            }

            IServiceScope serviceScope = services.BuildServiceProvider().CreateScope();
            TContext dbContext = serviceScope.ServiceProvider.GetRequiredService<TContext>();

            try
            {
                await dbContext.Database.EnsureCreatedAsync();
            }
            catch (Exception ex)
            {
                throw;
            }

            return dbContext;
        }

        private static TContext EnsureCreateAndGetDbContextEncapsulation<TContext>(IServiceCollection services,
                                                                                   string connectionString,
                                                                                   DatabaseOptions databaseOptions,
                                                                                   QueryTrackingBehavior trackingBehavior)
                                                                                        where TContext : DbContext
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            switch (databaseOptions)
            {
                case DatabaseOptions.InMemoryDatabase:
                    services.AddDbContext<TContext>(options => options.UseInMemoryDatabase(nameof(TContext)).UseQueryTrackingBehavior(trackingBehavior));
                    break;
                case DatabaseOptions.SqlServer:
                    services.AddDbContext<TContext>(options => options.UseSqlServer(connectionString).UseQueryTrackingBehavior(trackingBehavior));
                    break;
                case DatabaseOptions.Sqlite:
                    services.AddDbContext<TContext>(options => options.UseSqlite(connectionString).UseQueryTrackingBehavior(trackingBehavior));
                    break;
                default:
                    throw new ArgumentException($"{nameof(DatabaseOptions)} not find", nameof(DatabaseOptions));
            }

            IServiceScope serviceScope = services.BuildServiceProvider().CreateScope();
            TContext dbContext = serviceScope.ServiceProvider.GetRequiredService<TContext>();

            try
            {
                dbContext.Database.EnsureCreated();
            }
            catch (Exception ex)
            {
                throw;
            }

            return dbContext;
        }

        #endregion

        #region AddDbContext
        public static void JustAddDbContext<TContext>(this IServiceCollection services,
                                                       string connectionString,
                                                       DatabaseOptions databaseOptions,
                                                       QueryTrackingBehavior trackingBehavior = QueryTrackingBehavior.TrackAll)
                                                       where TContext : DbContext
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            switch (databaseOptions)
            {
                case DatabaseOptions.InMemoryDatabase:
                    services.AddDbContext<TContext>(options => options.UseInMemoryDatabase(nameof(TContext)).UseQueryTrackingBehavior(trackingBehavior));
                    break;
                case DatabaseOptions.SqlServer:
                    services.AddDbContext<TContext>(options => options.UseSqlServer(connectionString).UseQueryTrackingBehavior(trackingBehavior));
                    break;
                case DatabaseOptions.Sqlite:
                    services.AddDbContext<TContext>(options => options.UseSqlite(connectionString).UseQueryTrackingBehavior(trackingBehavior));
                    break;
                default:
                    throw new ArgumentException($"{nameof(DatabaseOptions)} not find", nameof(DatabaseOptions));
            }
        }
        #endregion
    }
}
