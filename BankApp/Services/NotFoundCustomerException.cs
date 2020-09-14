using System;

namespace BankApp.Services
{
    [Serializable()]
    public class NotFoundCustomerException : System.Exception
    {
        public NotFoundCustomerException() : base() { }
        public NotFoundCustomerException(string message) : base(message) { }
        public NotFoundCustomerException(string message, System.Exception inner) : base(message, inner) { }

        
        protected NotFoundCustomerException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
