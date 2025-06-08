using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace UserAPI.Entities.UserEntity.UpdateDTOs
{
    public class UpdateUserDTO
    {
        public int Id { get; set; }


        public string Lastname { get; set; } = default!;

        public string Firstname { get; set; } = default!;


    }
}
