using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [DisplayName("Email")]
        public string email { get; set; }
        [Required]
        [DisplayName("Password")]
        public string password { get; set; }
    }
}
