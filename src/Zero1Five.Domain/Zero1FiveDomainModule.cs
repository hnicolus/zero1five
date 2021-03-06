using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Zero1Five.MultiTenancy;
using Volo.Abp.AuditLogging;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Emailing;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.IdentityServer;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement.Identity;
using Volo.Abp.PermissionManagement.IdentityServer;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;
using Volo.Abp.BlobStoring.Azure;
using Zero1Five.AzureStorage;
using Volo.Abp.BlobStoring;
using Zero1Five.AzureStorage.Gig;
using Zero1Five.AzureStorage.Products;

namespace Zero1Five
{
    [DependsOn(
        typeof(Zero1FiveDomainSharedModule),
        typeof(AbpAuditLoggingDomainModule),
        typeof(AbpBackgroundJobsDomainModule),
        typeof(AbpFeatureManagementDomainModule),
        typeof(AbpIdentityDomainModule),
        typeof(AbpPermissionManagementDomainIdentityModule),
        typeof(AbpIdentityServerDomainModule),
        typeof(AbpPermissionManagementDomainIdentityServerModule),
        typeof(AbpSettingManagementDomainModule),
        typeof(AbpTenantManagementDomainModule),
        typeof(AbpEmailingModule)
    )]
    [DependsOn(typeof(AbpBlobStoringAzureModule))]
    public class Zero1FiveDomainModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpMultiTenancyOptions>(options => { options.IsEnabled = MultiTenancyConsts.IsEnabled; });

            var configuration = context.Services.GetConfiguration();
            ConfigureAzureStorageAccountOptions(context, configuration);
            ConfigureAbpBlobStoringOptions(configuration);
#if DEBUG
            context.Services.Replace(ServiceDescriptor.Singleton<IEmailSender, NullEmailSender>());
#endif
        }

        private void ConfigureAzureStorageAccountOptions(ServiceConfigurationContext context,IConfiguration configuration)
        {
            Configure<AzureStorageAccountOptions>(options =>
            {
                var azureStorageConnectionString = configuration["AzureStorageAccountSettings:ConnectionString"];
                var azureStorageAccountUrl = configuration["AzureStorageAccountSettings:AccountUrl"];

                options.ConnectionString = azureStorageConnectionString;
                options.AccountUrl = azureStorageAccountUrl;
            });
        }

        private void ConfigureAbpBlobStoringOptions(IConfiguration configuration)
        {
            Configure<AbpBlobStoringOptions>(options =>
            {
                var azureStorageConnectionString = configuration["AzureStorageAccountSettings:ConnectionString"];
               
                options.Containers.Configure<GigPictureContainer>(container =>
                {
                    container.UseAzure(azure =>
                    {
                        azure.ConnectionString = azureStorageConnectionString;
                        azure.CreateContainerIfNotExists = true;
                    });
                });
                options.Containers.Configure<ProductPictureContainer>(container =>
                {
                    container.UseAzure(azure =>
                    {
                        azure.ConnectionString = azureStorageConnectionString;
                        azure.CreateContainerIfNotExists = true;
                    });
                });
            });
        }
    }
}