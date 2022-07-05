using Azure.Messaging.ServiceBus;
using EAuctionBuyer.Data;
using EAuctionBuyer.Extensions;
using EAuctionBuyer.Model;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EAuctionBuyer.MessageBroker
{
    public class BidUpdateSenderServiceBus : IBidUpdateSender
    {
        private readonly string _connectionString;
        private readonly string _queueName;
        public BidUpdateSenderServiceBus(IOptions<AzureServiceBusConfiguration> serviceBusOptions)
        {
            _connectionString = serviceBusOptions.Value.ConnectionString;
            _queueName = serviceBusOptions.Value.QueueName;
        }
        public async void SendBid(string SenderMessage)
        {
            await using (var client = new ServiceBusClient(_connectionString))
            {
                var sender = client.CreateSender(_queueName);

                var json = JsonConvert.SerializeObject(SenderMessage);
                var message = new ServiceBusMessage(json);

                await sender.SendMessageAsync(message);
            }
        }
    }
}
