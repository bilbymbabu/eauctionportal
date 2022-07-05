using EAuctionSeller.Data;
using EAuctionSeller.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EAuctionSeller.MessageBroker
{
    public interface IBidUpdateSender
    {
         void SendBid(Product message);
    }
}
