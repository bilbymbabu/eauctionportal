using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EAuctionSeller.Extensions
{
    public class AzureServiceBusConfiguration
    {
        public string ConnectionString { get; set; }

        public string QueueName { get; set; }
    }
}
