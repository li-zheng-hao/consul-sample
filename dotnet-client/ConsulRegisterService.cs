using Consul;

namespace ConsulDemo;

public class ConsulRegisterService:IHostedService
{
    private readonly ConsulClient _consulClient;
    private readonly IHostApplicationLifetime _lifetime;

    public ConsulRegisterService(ConsulClient consulClient,IHostApplicationLifetime lifetime)
    {
        _consulClient = consulClient;
        _lifetime = lifetime;
    }
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var httpCheck = new AgentServiceCheck()
        {
            DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5), //服务启动多久后注册
            Interval = TimeSpan.FromSeconds(5), //健康检查时间间隔，或者称为心跳间隔
            HTTP = $"http://172.10.3.77:13000/ping", //健康检查地址
            Timeout = TimeSpan.FromSeconds(2)
        };

        // Register service with consul
        var registration = new AgentServiceRegistration()
        {
            Checks = new[] { httpCheck },
            ID = "172.10.3.77",
            Name = "demo-service",
            Address = "172.10.3.77",
            Port = 13000,
            Tags = new[] { "demo-service", },
            EnableTagOverride = true,
            Meta = new Dictionary<string, string>() { { "a", "a" } }
        };

        await _consulClient.Agent.ServiceRegister(registration, cancellationToken); //服务启动时注册，内部实现其实就是使用 Consul API 进行注册（HttpClient发起）
        
        _lifetime.ApplicationStopping.Register(() =>
        {
            _consulClient.Agent.ServiceDeregister("172.10.3.77", cancellationToken).GetAwaiter().GetResult(); //服务停止时取消注册
        });
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}