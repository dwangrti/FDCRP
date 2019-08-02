using System.ComponentModel.DataAnnotations;

namespace ASJ.ViewModels.Security
{ 
    public class ForgotPasswordViewModel
    {

        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }

    }
}