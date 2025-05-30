namespace UserAPI.Entities.AdressEntity
{
    public class UpdateAdressDTO
    {
        public string Country { get; set; }

        public int Zipcode { get; set; }

        public string City { get; set; }

        public string Street { get; set; }
        public int UserId { get; set; }
    }
}
