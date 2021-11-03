﻿using MQTTnet.Client.Options;
using MQTTnet.Client;
using MQTTnet.Diagnostics.Logger;
using MQTTnet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace RealtimeMap.API.MQQT
{
    public class HrtPositionsSubscription : IDisposable
    {
        public static async Task<HrtPositionsSubscription> Start(
            string sharedSubscriptionGroupName,
            Func<HrtPositionUpdate, Task> onPositionUpdate)
        {
            var mqttClient = CreateMqttClient();

            var mqttClientOptions = new MqttClientOptionsBuilder()
                .WithClientId(null) // do not keep state on the broker
                .WithCleanSession()
                .WithNoKeepAlive()
                .WithTls(new MqttClientOptionsBuilderTlsParameters
                {
                    UseTls = true,
                    SslProtocol = SslProtocols.Tls12
                })
                .WithTcpServer("mqtt.hsl.fi", 8883)
                .Build();

            mqttClient.UseConnectedHandler(async args =>
            {
                Console.WriteLine("### CONNECTED WITH MQTT SERVER ###");

                // we subscribe to a group subscription, so messages are distributed between cluster nodes
                await mqttClient.SubscribeAsync($"$share/{sharedSubscriptionGroupName}//hfp/v2/journey/ongoing/vp/bus/#");

                Console.WriteLine("### SUBSCRIBED ###");
            });

            mqttClient.UseDisconnectedHandler(async args =>
            {
                Console.WriteLine("### DISCONNECTED FROM MQTT SERVER ###");

                if (args.Exception is not null) Console.WriteLine(args.Exception);

                try
                {
                    await mqttClient.ConnectAsync(mqttClientOptions);
                }
                catch
                {
                    Console.WriteLine("### RECONNECTING FAILED ###");
                }
            });

            mqttClient.UseApplicationMessageReceivedHandler(async e =>
            {
                try
                {
                    var hrtPositionUpdate = HrtPositionUpdate.ParseFromMqttMessage(e.ApplicationMessage);

                    if (hrtPositionUpdate.HasValidPosition)
                    {
                        await onPositionUpdate(hrtPositionUpdate);
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                    throw;
                }
            });

            await mqttClient.ConnectAsync(mqttClientOptions);

            return new HrtPositionsSubscription(mqttClient);
        }

        private static IMqttClient CreateMqttClient()
        {
            var logger = CreateMqttNetLogger();
            var factory = new MqttFactory(logger);

            return factory.CreateMqttClient();
        }

        private static MqttNetEventLogger CreateMqttNetLogger()
        {
            var logger = new MqttNetEventLogger();

            logger.LogMessagePublished += (sender, e) =>
            {
                if (e.LogMessage.Level >= MqttNetLogLevel.Warning) Console.WriteLine(e.LogMessage.Message);

                if (e.LogMessage.Level == MqttNetLogLevel.Error) Console.WriteLine(e.LogMessage.Exception);
            };

            return logger;
        }

        private readonly IMqttClient _mqttClient;

        private HrtPositionsSubscription(IMqttClient mqttClient)
        {
            _mqttClient = mqttClient;
        }

        public void Dispose()
        {
            _mqttClient?.Dispose();
        }
    }
}
