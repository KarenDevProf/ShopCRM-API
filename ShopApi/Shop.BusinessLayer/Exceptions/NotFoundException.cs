namespace Shop.BusinessLayer.Exceptions
{
    public class NotFoundException : ShopBaseException
    {
        public NotFoundException(string errorType) : base(errorType)
        {

        }
    }
}
