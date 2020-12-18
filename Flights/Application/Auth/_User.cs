namespace Flights.Application.Auth
{
    public class _User
    {
        public int? UserId { get; set; }
        public string User { get; set; }
        public string Role { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }
    }
}