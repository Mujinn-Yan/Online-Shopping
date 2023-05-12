using AutoMapper;
using Microsoft.AspNetCore.Identity;
using OnlineStoreForWoman.DTOs;
using OnlineStoreForWoman.Models;
using System.Collections.Generic;
using System.Linq;

namespace DMClinic.Web.MapperProfiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            SourceMemberNamingConvention = new LowerUnderscoreNamingConvention();
            DestinationMemberNamingConvention = new PascalCaseNamingConvention();
            CreateMap<CartDto,Cart>();
           // CreateMap<Opportunity, Opportunity>();
           // CreateMap<OpportunityCreateOrUpdateDto, Opportunity>();
           // CreateMap<OpportunityCreateOrUpdateDto, Opportunity>().ReverseMap();
           // //CreateMap<MarkInteractionUpdateDto, Opportunity>()
           // //    .ForMember(x => x.Id, opt => opt.MapFrom(y => y.OpportunityID))
           // //    .ForMember(x => x.Site, opt => opt.Ignore());
           // CreateMap<MarkInteractionUpdateDto, OpportunityInteraction>();
           // CreateMap<MarkInteractionUpdateDto, Opportunity>();
           //     //.ForMember(x => x.Id, opt => opt.MapFrom(y => y.OpportunityId));
           // CreateMap<OpportunityInteractionAttachementDto, OpportunityInteractionAttachement>();
           // CreateMap<RoleMenuItem, MenuItemDto>();
           // CreateMap<RoleTableColumn, TableColumnDto>();
           // CreateMap<SponsorCreateDto, Sponsor>();
           // CreateMap<ContactResearchOrganizationDto, ContactResearchOrganization>();
           // CreateMap<SourceCreateDto, Source>();
           // CreateMap<ProgramCreateDto, SponsorProgram>().ReverseMap();
           // CreateMap<OpportunityTagDto, OpportunityTag>();
           //// CreateMap<IQueryable<OpportunityTag>, IQueryable<OpportunityTagDto>>();
           // //CreateMap<IEnumerable<OpportunityTag>, IEnumerable<OpportunityTagDto>>();
           // CreateMap<OpportunityTagDto, OpportunityTag>().ReverseMap();
           // CreateMap<TaggedUser, TaggedUserDto>().ReverseMap();
           // CreateMap<TagStatus, TagStatusDto>().ReverseMap();
           // CreateMap<TagActivityDto, TagActivity>().ReverseMap();
           // CreateMap<PriorityDto, Priority>().ReverseMap();
           // CreateMap<UserDto, IdentityUser>().ReverseMap();

           // CreateMap<LeadsAttachementDto, OpportunityLeadAttachment>();
        }
    }
}
