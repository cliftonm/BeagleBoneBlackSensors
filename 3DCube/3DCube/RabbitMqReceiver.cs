using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Text;

using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

using Clifton.Core.ExtensionMethods;

namespace BeagleboneSensors
{
	public class GyroEventArgs : EventArgs
	{
		public GyroData GyroData {get;protected set;}

		public GyroEventArgs(GyroData data)
		{
			GyroData = data;
		}
	}

	public class RabbitMqReceiver : RabbitMqIO
	{
		public string QueueName { get; set; }
		public event EventHandler<GyroEventArgs> GyroData;

		public RabbitMqReceiver()
		{
			QueueName = "bbbsensors";
		}

		public override void CreateConnection()
		{
			base.CreateConnection();
			DeclareQueue(channel, QueueName);
			EventingBasicConsumer consumer = new EventingBasicConsumer(channel);

			consumer.Received += (model, eventArgs) =>
			{
				byte[] body = eventArgs.Body;
				string message = Encoding.UTF8.GetString(body);
				System.Diagnostics.Debug.WriteLine(message);
				GyroData data = JsonConvert.DeserializeObject<GyroData>(message);
				GyroData.Fire(this, new GyroEventArgs(data));
			};

			channel.BasicConsume(QueueName, true, consumer);
		}

		private static void DeclareQueue(IModel channel, string queueName)
		{
			channel.QueueDeclare(queueName, false, false, false, null);
		}
	}
}
