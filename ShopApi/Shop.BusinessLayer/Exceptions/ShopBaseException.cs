using System;

namespace Shop.BusinessLayer.Exceptions
{
    public abstract class ShopBaseException : Exception
    {
        public string ErrorMessageResourceKey => this.GetType().Name.Replace("Exception", "");
        public string ErrorType { get; set; }

        protected ShopBaseException()
        {
        }

        protected ShopBaseException(string errorType)
        {
            ErrorType = errorType;
        }
    }
}
