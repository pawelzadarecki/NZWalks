using AutoMapper;

namespace NZWalks.Api.Profiles
{
    public class RegionsProfile: Profile
    {
        public RegionsProfile()
        {
            CreateMap<Models.Domian.Region, Models.DTO.Region>().
                ReverseMap();
        }
    }
}
