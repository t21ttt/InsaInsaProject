//using System.ComponentModel.DataAnnotations;

//namespace LibraryManagementSystem.Models
//{
//    public class ResetPasswordViewModel
//    {
//        [Required(ErrorMessage = "Email is required")]
//        [EmailAddress(ErrorMessage = "Invalid email address")]
//        public string Email { get; set; }

//        [Required(ErrorMessage = "Password is required")]
//        [DataType(DataType.Password)]
//        public string Password { get; set; }

//        [DataType(DataType.Password)]
//        [Display(Name = "Confirm password")]
//        [Compare("Password", ErrorMessage = "Password and Confirm Password must match")]
//        public string ConfirmPassword { get; set; }

//        public string Token { get; set; }
//    }
//}
