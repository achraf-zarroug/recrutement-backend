using System.ComponentModel.DataAnnotations;

    namespace backen_dotnet.Dtos.Account
    {
        public class RegisterDto
        {
            [Required]
            public string? Username { get; set; }
            [Required]
            [EmailAddress]
            public string? Email { get; set; }
            [Required]
            public string? Password { get; set; }
        public string? PhoneNumber { get; set; }


    }
}
