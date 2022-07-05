
namespace EAuctionBuyer.MessageBroker
{
    public interface IRabbitMqListener
    {
        void Receive();
        void Publish(string message);
    }
}
