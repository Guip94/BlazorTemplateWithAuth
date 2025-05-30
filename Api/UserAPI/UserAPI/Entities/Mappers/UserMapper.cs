using Domain.Entity;
using  UserAPI.Entities;
using UserAPI.Entities.UserEntity;

namespace UserAPI.Entities.Mappers
{
    public static class UserMapper
    {

        public static UserDTO ToUserDto(this User entity)
        {
            return new UserDTO
            {
                Id = entity.Id,
                Lastname = entity.Lastname,
                Firstname = entity.Firstname,
                Mail = entity.Mail
            };
           
        }
        public static UserDTO ToUserDtoWithToken(this User entity)
        {
            return new UserDTO
            {
                Id = entity.Id,
                Lastname = entity.Lastname,
                Firstname = entity.Firstname,
                Mail = entity.Mail,
                RefreshToken = entity.RefreshToken
            };
        }

    }
}
