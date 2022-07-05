using Azure.Messaging.ServiceBus;
using EAuctionSeller.Data;
using EAuctionSeller.Extensions;
using EAuctionSeller.Model;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EAuctionSeller.MessageBroker
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
        public async void SendBid(Product SenderMessage)
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
