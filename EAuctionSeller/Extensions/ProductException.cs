using System;
namespace EAuctionSeller.Extensions
{
    public class ProductException : Exception
    {
        public ProductException(string message)
            : base(message)
        {
        }

    }
    public class ProductNotFounException : Exception
    {
        public ProductNotFounException(string message)
            : base(message)
        {
        }
    }
}
