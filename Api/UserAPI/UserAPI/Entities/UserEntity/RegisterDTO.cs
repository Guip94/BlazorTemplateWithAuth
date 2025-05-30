using System.ComponentModel.DataAnnotations;

namespace UserAPI.Entities.UserEntity
{
    public class RegisterDTO
    {
        [Required]
        [EmailAddress]
        public string Mail { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Pwd { get; set; }

        [Required]
        [MinLength(2)]
        public string Lastname { get; set; }
        [Required]
        [MinLength(2)]
        public string Firstname { get; set; }

    }
}
