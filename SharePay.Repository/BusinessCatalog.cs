
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharePay.Repository;

namespace SharePay.Repository
{
    public static class BusinessCatalog
    {
        public static void RegisterRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserGroupRepository, UserGroupRepository>();
            services.AddScoped<IGroupRepository, GroupRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
        }
    }
}
