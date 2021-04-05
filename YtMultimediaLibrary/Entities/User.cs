using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace YtMultimediaLibrary.Entities
{
    public class User
    {
        public User()
        {
            Channels = new List<Channel>();
        }

        [Key]
        public int UserId { get; set; }

        [Required][MaxLength(15)]
        public string UserName { get; set; }
        [Required][MaxLength(15)]
        public string EMail { get; set; }
        [Required]
        public string PasswordHashed { get; set; }

        public List<Channel> Channels { get; set; }
    }
}
