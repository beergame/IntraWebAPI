using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IntraWebApi.Models
{
    public class Article
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        public byte[] Picture { get; set; }

    }
}
