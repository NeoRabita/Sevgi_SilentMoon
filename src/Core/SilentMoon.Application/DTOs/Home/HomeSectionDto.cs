using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilentMoon.Application.DTOs.Home
{
    public class HomeSectionDto
    {
        public string Title { get; set; }
        public ICollection<CourseItemDto> CourseItems { get; set; }
    }
}
