using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AlfaPeople.KingKhalidFoundation.Web.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email", ResourceType = typeof(App_GlobalResources.General))]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email", ResourceType = typeof(App_GlobalResources.General))]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Username", ResourceType = typeof(App_GlobalResources.General))]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(App_GlobalResources.General))]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Display(Name = "UserRoles")]
        public string UserRoles { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email", ResourceType = typeof(App_GlobalResources.General))]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(App_GlobalResources.General))]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[#$^+=!*()@%&]).{8,}$", ErrorMessageResourceName = "PassMsgs2", ErrorMessageResourceType = typeof(App_GlobalResources.General))]
        [StringLength(100, ErrorMessageResourceName = "PassMsg1", ErrorMessageResourceType = typeof(App_GlobalResources.General), MinimumLength = 8)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "ConfPass", ResourceType = typeof(App_GlobalResources.General))]
        [Compare("Password", ErrorMessageResourceName = "ConfirmMsg", ErrorMessageResourceType = typeof(App_GlobalResources.General))]
        public string ConfirmPassword { get; set; }
    }

    public class RegisterEmployee
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email", ResourceType = typeof(App_GlobalResources.General))]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Username", ResourceType = typeof(App_GlobalResources.General))]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(App_GlobalResources.General))]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[#$^+=!*()@%&]).{8,}$", ErrorMessageResourceName = "PassMsgs2", ErrorMessageResourceType = typeof(App_GlobalResources.General))]
        [StringLength(100, ErrorMessageResourceName = "PassMsg1", ErrorMessageResourceType = typeof(App_GlobalResources.General), MinimumLength = 8)]
        public string Password { get; set; }

        public bool IsActive { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string Code { get; set; }

        //[Required]
        //[EmailAddress]
        //[Display(Name = "Email", ResourceType = typeof(App_GlobalResources.General))]
        //public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(App_GlobalResources.General))]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[#$^+=!*()@%&]).{8,}$", ErrorMessageResourceName = "PassMsgs2", ErrorMessageResourceType = typeof(App_GlobalResources.General))]
        [StringLength(100, ErrorMessageResourceName = "PassMsg1", ErrorMessageResourceType = typeof(App_GlobalResources.General), MinimumLength = 8)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "ConfPass", ResourceType = typeof(App_GlobalResources.General))]
        [Compare("Password", ErrorMessageResourceName = "ConfirmMsg", ErrorMessageResourceType = typeof(App_GlobalResources.General))]
        public string ConfirmPassword { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email", ResourceType = typeof(App_GlobalResources.General))]
        public string Email { get; set; }
    }

    public class CustomForgotPasswordVM
    {
        public string CorpName { get; set; }

        public string imgSrc { get; set; }

        [Required]
        [MaxLength(20)]
        [StringLength(20)]
        public string CorpRegNum { get; set; }
    }
}