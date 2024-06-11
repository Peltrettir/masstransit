using RabbitMQ.Client;
using System.Text;

var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.QueueDeclare("Hello", false, false, false, null);
const string message = "Hello, World!";
var body = Encoding.UTF8.GetBytes(message);

channel.BasicPublish(string.Empty, "Hello", null, body);

Console.WriteLine($"Sent message \"{message}\"");
Console.ReadLine();