using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.Api.Repository;

namespace NZWalks.Api.Controllers
{
    [ApiController]
    [Route("controller")]
    public class WalkController : Controller
    {
        private readonly IWalkRepository walkRepository;

        public IMapper Mapper { get; }

        public WalkController(IWalkRepository walkRepository, IMapper mapper)
        {
            this.walkRepository = walkRepository;
            Mapper = mapper;
        }

        [HttpGet]

        public async Task<IActionResult> GetAllWalks()
        {
            //Fetch data from database - domain walks
            var walksDomain = await walkRepository.GetAllAsync();

            //Convert domain walks to DTO walks
            var walksDTO = Mapper.Map<List<Models.DTO.Walk>>(walksDomain);

            //Return response
            return Ok(walksDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkAsync")]
        public async Task<IActionResult> GetWalkAsync(Guid id)
        {
            //Fetch data from database - domain walks
            var walkDomain = await walkRepository.GetAsync(id);

            //Convert domain walks to DTO walks
            var walkDTO = Mapper.Map<Models.DTO.Walk>(walkDomain);

            //Return response
            return Ok(walkDTO);
        }

        [HttpPost]

        public async Task<IActionResult> AddAsync([FromBody] Models.DTO.AddWalkRequest addWalkRequest)
        {
            //Conver DTO do Domain
            var walkdDomain = new Models.Domian.Walk()
            {
                Name = addWalkRequest.Name,
                Length = addWalkRequest.Length,
                RegionId = addWalkRequest.RegionId,
                WalkDifficultyId = addWalkRequest.WalkDifficultyId,
            };

            //Pass data to database
            walkdDomain = await walkRepository.AddAsync(walkdDomain);

            // Conver the Domain object to DTO
             var walkDTO = Mapper.Map<Models.DTO.Walk>(walkdDomain);

            // Return response


            return CreatedAtAction(nameof(GetWalkAsync), new { Id = walkdDomain.Id }, walkDTO);

        }

        [HttpPut]
        [Route("{id:guid}")]

        public async Task<IActionResult> UpdateWalk([FromRoute]Guid id, [FromBody]Models.DTO.UpdateWalkRequest updateWalkRequest)
        {
            //Convert DTO to Domain
            var walk = new Models.Domian.Walk()
            {
                Name = updateWalkRequest.Name,
                Length = updateWalkRequest.Length,
            };

            //Update database
            walk = await walkRepository.UpdateAsync(id, walk);

            //check if null - NotFound
            if(walk == null)
            {
                return NotFound();
            }

            //Convert Domain to DTO
            var walkDTO = Mapper.Map<Models.DTO.Walk>(walk);

            //Return respond
            return Ok(walkDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]

        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            //delete walk
            var walk = await walkRepository.DeleteAsync(id);

            //check if null
            if(walk == null)
            {
                return NotFound();
            }

            //conver to DTO
            var walkDTO = Mapper.Map<Models.DTO.Walk>(walk);

            //return respond
            return Ok(walkDTO);
        }
    }
}