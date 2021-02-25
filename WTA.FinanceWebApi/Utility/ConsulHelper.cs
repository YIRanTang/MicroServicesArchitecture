using Consul;
using Microsoft.Extensions.Configuration;
using System;

namespace WTA.FinanceWebApi.Utility
{
    public static class ConsulHelper
    {
        public static void ConsulRegist(this IConfiguration configuration)
        {
            ConsulClient client = new ConsulClient(c =>
            {
                c.Address = new Uri("http://127.0.0.1:8500/");
                c.Datacenter = "dc1";
            });
            string ip = configuration["ip"];
            int port = int.Parse(configuration["port"]);//命令行参数必须传入
            //int weight = string.IsNullOrWhiteSpace(configuration["weight"]) ? 1 : int.Parse(configuration["weight"]);//命令行参数必须传入
            client.Agent.ServiceRegister(new AgentServiceRegistration()
            {
                ID = "service" + Guid.NewGuid(),//唯一的
                Name = "dataservice",//组名称-Group
                Address = ip,//其实应该写ip地址
                Port = port,//不同实例
                //Tags = new string[] { weight.ToString() },//标签
                Check = new AgentServiceCheck()//配置心跳检查的
                {
                    Interval = TimeSpan.FromSeconds(12),
                    HTTP = $"http://{ip}:{port}/Api/Health/Index",
                    Timeout = TimeSpan.FromSeconds(5),
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5)
                }
            });
            Console.WriteLine($"http://{ip}:{port}完成注册");
        }
    }
}
