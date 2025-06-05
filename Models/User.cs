
using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Motel_birkenhein.Models
{
        public class User : IdentityUser
        {
            public string? FirstName { get; set; } = "";
            public string? LastName { get; set; } = "";
                
        }
    
}
