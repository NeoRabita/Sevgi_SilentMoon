using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using SilentMoon.Application.Interfaces.Messaging;
using SilentMoon.Infrastructure.Messaging.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SilentMoon.Infrastructure.Messaging.Producers
{
    public class RabbitMqProducer : IMessagePublisher
    {
        private readonly RabbitMqSettings _settings;

        public RabbitMqProducer(IOptions<RabbitMqSettings> options)
        {
            _settings = options.Value;
        }

        public async Task PublishAsync<T>(string queueName, T message)
        {
            var factory = new ConnectionFactory
            {
                HostName = _settings.Host,
                Port = _settings.Port,
                UserName = _settings.Username,
                Password = _settings.Password
            };

            await using var connection = await factory.CreateConnectionAsync();
            await using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false);

            var body = Encoding.UTF8.GetBytes(
                JsonSerializer.Serialize(message));

            await channel.BasicPublishAsync(
                exchange: string.Empty,
                routingKey: queueName,
                body: body);
        }

    }
}
