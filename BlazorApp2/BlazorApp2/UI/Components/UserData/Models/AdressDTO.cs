using System.ComponentModel.DataAnnotations;

namespace BlazorApp2.UI.Components.UserData.Models
{
    public class AdressDTO
    {
        [Required(ErrorMessage = "Country is required")]
        public string Country { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }

        [Required(ErrorMessage = "Zipcode is required")]
        public int Zipcode { get; set; }

        [Required(ErrorMessage = "Street is required")]
        public string Street { get; set; }

    }
}
