using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Net;

namespace MagicVilla_VillaAPI.Models
{
    public class APIResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
        public object response { get; set; }
    }
}
