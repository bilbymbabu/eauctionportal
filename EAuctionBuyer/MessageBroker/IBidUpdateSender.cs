using EAuctionBuyer.Data;
using EAuctionBuyer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EAuctionBuyer.MessageBroker
{
    public interface IBidUpdateSender
    {
         void SendBid(string message);
    }
}