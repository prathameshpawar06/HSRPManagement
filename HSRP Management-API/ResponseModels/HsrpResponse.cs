namespace HSRP_Management_API.ResponseModels
{
    public class HsrpResponse
    {
        public string Message { get; set; }
        public bool Success { get; set; }
        public object Data { get; set; }
        public string Tocken { get; set; }
        public HsrpResponse(string message, bool success, string tocken = null, object data = null)
        {
            Message = message;
            Success = success;
            Data = data;
            Tocken = tocken;
        }
    }
}
