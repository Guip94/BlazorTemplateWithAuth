using System.ComponentModel.DataAnnotations;

namespace BlazorApp2.UI.Components.UserData.Models
{
    public class UserDTO
    {

        [Required(ErrorMessage = "First name is required")]
        [MinLength(1)]
        public string FirstName { get; set; }


        [Required(ErrorMessage = "Last name is required")]
        [MinLength(1)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Mail is required")]
        [DataType(DataType.EmailAddress)]
        public string Mail { get; set; }







    
    }
}
