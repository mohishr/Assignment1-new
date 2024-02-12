namespace Assignment1.UI.Services
{
    public class ServiceResponse<T>
    {
        public T Data { get; set; }
        public string Message { get; set; }
        public string StatusCode { get; set; }
    }
}
