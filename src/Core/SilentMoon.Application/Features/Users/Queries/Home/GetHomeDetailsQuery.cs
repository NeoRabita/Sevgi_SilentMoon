using Application.Abstractions.Messaging;
using SilentMoon.Application.DTOs.Home;
using SilentMoon.SharedKernel.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SilentMoon.Application.Features.Users.Queries.Home
{
    public class GetHomeDetailsQuery:IQuery<HomeSectionDto>
    {
    }

    public class GetHomeDetailsQueryHandler : IQueryHandler<GetHomeDetailsQuery, HomeSectionDto>
    {
        public Task<Result<HomeSectionDto>> Handle(GetHomeDetailsQuery query, CancellationToken ct)
        {
           throw new NotImplementedException();
        }
    }
}
