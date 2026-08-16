using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilentMoon.Application.DTOs.Topic
{
    public class TopicResponse
    {
        public int Id { get; set; }

        public string Slug { get; set; }

        public string Title { get; set; }

        public string IconKey { get; set; }
        public string ImgUrl { get; set; }
        public string ColorHex { get; set; }
    }
}
