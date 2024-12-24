
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharePay.Business.Implementations;
using SharePay.Business.Interfaces;
using SharePay.Repository;

namespace SharePay.Business
{
    public static class BusinessCatalog
    {
        public static void RegisterBusinesses(this IServiceCollection services)
        {
            services.AddScoped<IUserAccountBusiness, UserAccountBusiness>();
            services.AddScoped<IUserGroupBusiness, UserGroupBusiness>();
            services.AddScoped<IGroupBusiness, GroupBusiness>();
            services.AddScoped<IUserBusiness, UserBusiness>();
        }
    }
}
