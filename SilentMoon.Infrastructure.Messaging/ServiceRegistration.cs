using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SilentMoon.Infrastructure.Messaging.Settings;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SilentMoon.Application.Interfaces.Messaging;
using SilentMoon.Infrastructure.Messaging.Producers;
using SilentMoon.Infrastructure.Messaging.Consumers;

namespace SilentMoon.Infrastructure.Messaging
{
    public static class ServiceRegistration
    {
        public static void AddMessagingRegistration(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<RabbitMqSettings>(
                configuration.GetSection("RabbitMq"));


            services.AddSingleton<IMessagePublisher, RabbitMqProducer>();
            services.AddHostedService<RabbitMqConsumer>();
        }
    }
}
