
namespace EAuctionSeller.MessageBroker
{
    public interface IRabbitMqProducer
    {
        void Receive();
        void Publish(string message);
    }
}
