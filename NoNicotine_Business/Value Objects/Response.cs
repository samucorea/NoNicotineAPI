using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

namespace NoNicotineAPI.Models
{
    public class Response<T>
    {

        public bool Succeeded { get; set; }

        public string? Message { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public T? Data { get; set; }
    }
}
