using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SilentMoon.Application.DTOs.Email;
using SilentMoon.Application.Interfaces.Messaging;
using SilentMoon.Application.Interfaces.Services;
using SilentMoon.Infrastructure.Messaging.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SilentMoon.Infrastructure.Messaging.Consumers
{
    public class RabbitMqConsumer : BackgroundService
    {
        private readonly RabbitMqSettings _settings;
        private readonly IServiceScopeFactory _scopeFactory;

        public RabbitMqConsumer(
            IOptions<RabbitMqSettings> options, IServiceScopeFactory scopeFactory)
        {
            _settings = options.Value;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory
            {
                HostName = _settings.Host,
                Port = _settings.Port,
                UserName = _settings.Username,
                Password = _settings.Password
            };
            using var _scope = _scopeFactory.CreateScope();
            var _emailService = _scope.ServiceProvider.GetService<IEmailService>();
            var connection = await factory.CreateConnectionAsync(stoppingToken);
            var channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);

            await channel.QueueDeclareAsync(
                queue: "email-queue",
                durable: true,
                exclusive: false,
                autoDelete: false,
                cancellationToken: stoppingToken);

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += async (_, ea) =>
            {
                var json = Encoding.UTF8.GetString(ea.Body.ToArray());

                var request = JsonSerializer.Deserialize<EmailRequest>(json);

                if (request is not null)
                {
                    await _emailService.SendAsync(request);
                }

                await channel.BasicAckAsync(
                    deliveryTag: ea.DeliveryTag,
                    multiple: false,
                    cancellationToken: stoppingToken);
            };

            await channel.BasicConsumeAsync(
                queue: "email-queue",
                autoAck: false,
                consumer: consumer,
                cancellationToken: stoppingToken);

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
    }
}
