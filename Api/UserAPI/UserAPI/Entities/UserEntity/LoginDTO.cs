using System.ComponentModel.DataAnnotations;

namespace UserAPI.Entities.UserEntity
{
    public class LoginDTO
    {
        private string _mail;
        private string _pwd;

        [Required]
        public string Mail { get { return _mail; } set { _mail = value; } }

        [Required]
        public string Pwd { get { return _pwd; } set { _pwd = value; } }
    }
}
