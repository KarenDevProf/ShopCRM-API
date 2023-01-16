namespace Shop.API.Models
{
    public class ResponseObjectModel<T> where T : class, new()
    {
        public ResponseObjectModel()
        {
            HasError = false;
            Data = new T();
        }

        public bool HasError { get; set; }
        public string Message { get; set; }
        public string ErrorCode { get; set; }
        public T Data { get; set; }


    }
}
