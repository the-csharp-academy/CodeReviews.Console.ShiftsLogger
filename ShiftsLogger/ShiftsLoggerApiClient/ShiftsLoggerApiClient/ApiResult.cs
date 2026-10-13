using System.Net;

namespace ShiftsLoggerApiClient
{
    public class ApiResult
    {
        public IEnumerable<ShiftsLog>? Shifts { get; set; }
        public HttpStatusCode? Status { get; set; }
        public bool? Success { get; set; }
    }
}