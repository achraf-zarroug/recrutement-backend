using System.ComponentModel.DataAnnotations;

namespace backen_dotnet.Dtos.Account
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)] // Cela masquera le champ de mot de passe dans Swagger

        public string Password { get; set; }
    }
}
