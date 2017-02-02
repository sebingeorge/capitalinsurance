using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capital.Domain
{
    public class RegisterViewModel
    {
        public int? UserId { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        public string UserName { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        [EmailAddress]
        public string Email { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        [System.ComponentModel.DataAnnotations.StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [System.ComponentModel.DataAnnotations.DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
        public string Password { get; set; }

        [System.ComponentModel.DataAnnotations.DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        public int UserRole { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        public int SalesMgId { get; set; }
        public List<Modules> Module { get; set; }
        public List<FormsVsUser> Forms { get; set; }
        public int Reporting { get; set; }
    }
}
