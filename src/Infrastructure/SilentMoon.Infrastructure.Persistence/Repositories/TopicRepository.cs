using Dapper;
using SilentMoon.Application.DTOs.Topic;
using SilentMoon.Application.Interfaces.Repositories;
using SilentMoon.Domain.Entities;
using SilentMoon.Infrastructure.Persistence.Contexts;
using SilentMoon.Infrastructure.Persistence.Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilentMoon.Infrastructure.Persistence.Repositories
{
    public class TopicRepository : GenericRepository<Topic>, ITopicRepository
    {
        IDapper dapper;

        public TopicRepository(IDapper dapper,AppDbContext dbContext):base(dbContext)
        {
            this.dapper = dapper;
        }

        public async Task<IEnumerable<TopicResponse>> GetSelectedTopicsAsync(string userId)
        {
            const string sql = @"
        SELECT
            t.Id,
            t.Title,
            t.Slug,
            t.IconKey,
            t.ColorHex
        FROM VW_TOPICS t
        INNER JOIN UserTopics ut
            ON ut.TopicId = t.Id
        WHERE ut.UserId = :USER_ID";

            var parameters = new DynamicParameters();
            parameters.Add("USER_ID", userId);

            return await dapper.GetAllAsync<TopicResponse>(sql, parameters);
        }

        public async Task<IEnumerable<TopicResponse>> GetAllTopicsAsync()
        {
            const string sql = @"
        SELECT
            Id,
            Title,
            Slug,
            IconKey,
            ColorHex
        FROM VW_TOPICS
        ORDER BY Id";

            return await dapper.GetAllAsync<TopicResponse>(sql);
        }
    }
}
