using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;

using RabbitMQ.Client;

namespace BeagleboneSensors
{
	public abstract class RabbitMqIO
	{
		protected List<IPAddress> localHostIPs;
		protected IConnection connection;
		protected IModel channel;

		public RabbitMqIO()
		{
			localHostIPs = Helpers.GetLocalHostIPs();
		}

		public virtual void CreateConnection()
		{
			ConnectionFactory factory = new ConnectionFactory();
			factory.UserName = ConfigurationManager.AppSettings["username"];
			factory.Password = ConfigurationManager.AppSettings["password"];
			factory.VirtualHost = "/";
			factory.Protocol = Protocols.DefaultProtocol;
			factory.HostName = localHostIPs[0].ToString();   // "192.168.0.2";
			factory.Port = 5672; //  AmqpTcpEndpoint.UseDefaultPort;

			connection = factory.CreateConnection();
			channel = connection.CreateModel();
		}

		public virtual void Shutdown()
		{
			channel.Dispose();
			connection.Dispose();
		}
	}
}
