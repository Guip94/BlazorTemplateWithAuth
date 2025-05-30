namespace UserAPI.Entities.UserEntity
{
    public class UserDTO
    {
        public int Id { get; set; }
        public required string Mail { get; set; }
        public required string Lastname { get; set; }
        public required string Firstname { get; set; }
        public string AccessToken { get; set; } = default!;

        public string RefreshToken { get; set; } = default!;

    }
}
