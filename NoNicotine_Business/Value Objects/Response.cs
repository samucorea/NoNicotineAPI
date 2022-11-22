using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace NoNicotineAPI.Models
{
    public class Response<T>
    {
        [JsonIgnore]
        public bool Succeeded { get; set; }

        public string Message { get; set; } = string.Empty;
        [JsonIgnore]
        public T? Data { get; set; }
    }
}
