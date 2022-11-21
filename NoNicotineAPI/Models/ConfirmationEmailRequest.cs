namespace NoNicotineAPI.Models
{
    public class ConfirmationEmailRequest
    {
        public string  Token { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
    }
}
