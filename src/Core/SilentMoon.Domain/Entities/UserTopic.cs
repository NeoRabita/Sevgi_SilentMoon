using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilentMoon.Domain.Entities
{
    public class UserTopic
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int TopicId { get; set; }

        public ApplicationUser User { get; set; }
        public Topic Topic { get; set; }

    }
}
