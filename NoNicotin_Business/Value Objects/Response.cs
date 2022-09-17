using Microsoft.AspNetCore.Identity;

namespace NoNicotineAPI.Models
{
    public class Response<T>
    {

        public bool Succeeded { get; set; }

        public string? Message { get; set; }

        public T? Data { get; set; }
    }
}
