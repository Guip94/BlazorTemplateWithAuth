using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace UserAPI.Entities.UserEntity
{
    public class UpdateUserDTO
    {
        public int Id { get; set; }
        [PasswordPropertyText]
        public string Pwd { get; set; }

        [EmailAddress]
        public string Mail { get; set; }

        public string Lastname { get; set; } = default!;

        public string Firstname { get; set; } = default!;


    }
}
