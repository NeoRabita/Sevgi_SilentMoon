using SilentMoon.Application.DTOs.Topic;
using SilentMoon.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilentMoon.Application.Interfaces.Repositories
{
    public interface ITopicRepository:IGenericRepository<Topic>
    {
        public Task<IEnumerable<TopicResponse>> GetSelectedTopicsAsync(string userId);
        public Task<IEnumerable<TopicResponse>> GetAllTopicsAsync();
    }
}
