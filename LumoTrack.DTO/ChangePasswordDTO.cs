using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LumoTrack.DTO
{
    public class ChangePasswordDTO
    {
        public string UserID { get; set; }
        
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage ="Campo requerido")]
        [StringLength(100, ErrorMessage = "La contraseña debe de cumplir con el formato indicado")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Las contraseñas ingresadas no coinciden")]
        public string ConfirmPassword { get; set; }
    }
}
