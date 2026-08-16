using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilentMoon.Domain.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Slug { get; set; }
        public string Title { get; set; }   
        public string Type { get; set; }
        public string IconUrl { get; set; }

    }
}
