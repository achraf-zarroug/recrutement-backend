namespace backen_dotnet.Dtos.Account
{
    public class NewUserDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public DateTime DateCreated { get; set; }
        public string Token { get; set; }

    }
}
