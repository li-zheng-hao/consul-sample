using Consul;

namespace ConsulDemo;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddConsul(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {

        serviceCollection.AddSingleton<ConsulClient>(sp =>
        {
            var consulClient = new ConsulClient(x =>
            {
                x.Address = new Uri(
                    $"http://{configuration["consul:ip"]}:{configuration["consul:port"]}");
            });
           
         
            return consulClient;
        });
        serviceCollection.AddHostedService<ConsulRegisterService>();
        
        
        return serviceCollection;
    }

   
}