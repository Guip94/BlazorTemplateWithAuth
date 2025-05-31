using System.ComponentModel.DataAnnotations;

namespace BlazorApp2.AuthSystems.Models
{
    public class RegisterModel
    {
        [Required]
        public string Mail { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public  string Pwd { get; set; }


        [Required]
        public string Lastname { get; set; }
        [Required]
        public string Firstname { get; set; }


        public RegisterModel(string mail,  string pwd, string lastname, string firstname)
        {
            Mail = mail;
            Pwd = pwd;
            Lastname = lastname;
            Firstname = firstname;
        }
    }
}
