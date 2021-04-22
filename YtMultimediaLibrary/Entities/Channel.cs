using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace YtMultimediaLibrary.Entities
{
    public class Channel
    {
        [Key]
        public int ChannelId { get; set; }
        [Required]
        public string Link { get; set; }
        [Required]
        public string ChannelName { get; set; }
        [Required]
        public int UserId { get; set; }

        public List<User> Users { get; set; }
    }
}
