using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BlazorApp2.AuthSystems.Models
{
    public class LoginModel
    {
        private string _mail;
        private string _pwd;
        [Required]
        public string Mail { get { return _mail; } set { _mail = value; } }

        [Required]
        [DataType(DataType.Password)]
        public string Pwd { get { return _pwd; } set { _pwd = value; } }


        public LoginModel()
        {

        }
        public LoginModel(string mail, string pwd)
        {
            Mail = mail;
            Pwd = pwd;
        }
    }
}
