using AutoMapper;

namespace NZWalks.Api.Profiles
{
    public class WalkProfile: Profile
    {
        public WalkProfile()
        {
            CreateMap<Models.Domian.Walk, Models.DTO.Walk>().
                ReverseMap();

            CreateMap<Models.Domian.WalkDifficulty, Models.DTO.WalkDifficulty>().
                ReverseMap();

        }
    }
}
