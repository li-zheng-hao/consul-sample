# consul-sample

# 关于Consul

## 配置ACL令牌

使用`consul.exe agent -config-file=server1.json `启动服务后，输入`consul.exe acl bootstrap`获取初始化的管理员令牌，然后把配置的`default_policy`改为`deny`

## 集群配置

参考`consul-server`下的json配置文件，启动命令：`consul.exe agent -config-file=server1.json`

# 客户端连接

## ASP.NET Core

参考目录`dotnet-client`

## golang

参考目录`go-client`


# 灰度发布(金丝雀发布)

两种方案：

1. [官方方案](https://www.hashicorp.com/blog/canary-deployments-with-consul-service-mesh)

不太推荐，配置文件需要使用cli或http手写，不支持ui页面修改，比较麻烦

2. [基于tag的方案](https://blog.csdn.net/qq_43692950/article/details/125226460)

简单，单独弄一个环境部署灰度发布的服务（例如弄一个1c2g的服务器，环境变量加上ENV:GRAY）, 所有服务在Consul注册时先判断当前是否为灰度环境，是的话tag加一个`{"env":"gray"}`的标签，然后把一部分用户设置为灰度用户，这些用户在访问服务时时优先调用灰度服务，这样就可以用测试账号先提前测试灰度服务，在测试通过后发布到正式的生产环境



# 参考资料

1. [client mode configuration](https://devopscube.com/hsetup-configure-consul-agent-client-mode/)
2. [consul单机集群配置](https://blog.csdn.net/yzf279533105/article/details/130268875)
3. [Consul官方配置文件文档](https://developer.hashicorp.com/consul/docs/agent/config/config-files)