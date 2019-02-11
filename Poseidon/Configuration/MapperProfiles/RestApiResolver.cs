using AutoMapper;
using Microsoft.AspNetCore.Http;
using Poseidon.Dtos;
using Poseidon.Helpers;
using Poseidon.Models;
using System.Linq;

namespace Poseidon.Configuration.MapperProfiles
{
    public class RestApiResolver : 
        IValueResolver<Alarm, AlarmDto, string>,
        IMemberValueResolver<IPaginatedResource, IPaginatedDto, string, string>
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public RestApiResolver(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public string Resolve(Alarm source, AlarmDto destination, string destMember, ResolutionContext context)
        {
            return source.Ack ? null : httpContextAccessor.HttpContext.GetAcknowledgmentAlarmUri(source.Id);
        }

        public string Resolve(IPaginatedResource source, IPaginatedDto destination, string destMember, ResolutionContext context)
        {
            string pageNumber = httpContextAccessor.HttpContext.Request.Query
                .FirstOrDefault(e => string.Compare(e.Key, "pageNumber", true) == 0).Value;
            if (string.IsNullOrEmpty(pageNumber)) pageNumber = "1";

            return httpContextAccessor.HttpContext.GetNextPageUrl(int.Parse(pageNumber) < source.PageCount);
        }

        public string Resolve(IPaginatedResource source, IPaginatedDto destination, string sourceMember, string destMember, ResolutionContext context)
        {
            string pageNumber = httpContextAccessor.HttpContext.Request.Query
                .FirstOrDefault(e => string.Compare(e.Key, "pageNumber", true) == 0).Value;
            if (string.IsNullOrEmpty(pageNumber)) pageNumber = "1";

            string url = null;

            if (sourceMember.Equals(nameof(PaginatedDto<object>.NextPageUrl)))
            {
                url = httpContextAccessor.HttpContext.GetNextPageUrl(int.Parse(pageNumber) < source.PageCount);
            }
            else if (sourceMember.Equals(nameof(PaginatedDto<object>.PreviousPageUrl)))
            {
                url = httpContextAccessor.HttpContext.GetPreviousPageUrl(int.Parse(pageNumber) > 1);
            }

            return url;
        }
    }
}
