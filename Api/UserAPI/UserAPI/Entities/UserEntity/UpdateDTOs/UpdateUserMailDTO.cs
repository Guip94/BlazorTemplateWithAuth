namespace UserAPI.Entities.UserEntity.UpdateDTOs
{
    public class UpdateUserMailDTO
    {
        public int Id { get; set; }

        public string Mail { get; set; } = default!;

        public string Pwd { get; } = default!;
    }
}
