using RabbitMQ.Client;
using System.Runtime.InteropServices.JavaScript;
using System.Text;

ConnectionFactory factory= new();

string user = "guest";
string password = "guest";
string vhost = "/";
string hostName = "127.0.0.1";
int port = 5672;
factory.UserName = user;
factory.Password = password;
factory.VirtualHost = vhost;
factory.HostName = hostName;
factory.Port = port;

factory.Uri = new Uri($"amqp://{user}:{password}@{hostName}:{port}/{vhost}");
factory.ClientProvidedName = "Rabbit Sender App";


IConnection cnn = factory.CreateConnection();

IModel channel = cnn.CreateModel();

string exchangeName = "DemoExchange";
string routingKey = "demo-routing-key";
string queueName = "DemoQueue";

channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
channel.QueueDeclare(queueName, false, false, false, null);
channel.QueueBind(queueName, exchangeName, routingKey);

// Send message to message broker
for (int i = 0; i < 60; i++)
{
    string message = $"Message #{i}!";
    byte[] messageBodyBytes = Encoding.UTF8.GetBytes(message);
    channel.BasicPublish(exchangeName, routingKey, null, messageBodyBytes);
    Thread.Sleep(1000);
}

channel.Close();
cnn.Close();
