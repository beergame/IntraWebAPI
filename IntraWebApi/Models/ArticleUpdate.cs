using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IntraWebApi.Models
{
    public class ArticleUpdate
    {
        [Required]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public byte[] Picture { get; set; }
    }
}
