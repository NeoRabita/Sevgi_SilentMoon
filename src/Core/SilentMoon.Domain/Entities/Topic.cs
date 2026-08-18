using SilentMoon.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilentMoon.Domain.Entities
{
    public class Topic:BaseEntity<int>
    {
        public string Slug { get; set; }
        public string Title { get; set; }
        public string IconKey { get; set; }
        public string ColorHex { get; set; }
        public ICollection<UserTopic> UserTopics { get; set; } = new List<UserTopic>();
        public string TranslationId { get; set; }
        public Translation Translation { get; set; }

    }
}
