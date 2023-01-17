using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.Api.Repository;

namespace NZWalks.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalkController : Controller
    {
        private readonly IWalkRepository walkRepository;
        private readonly IRegionRepository regionRepository;
        private readonly IWalkDifficultyRepository walkDifficultyRepository;

        public IMapper Mapper { get; }

        public WalkController(IWalkRepository walkRepository, IMapper mapper, IRegionRepository regionRepository, IWalkDifficultyRepository walkDifficultyRepository)
        {
            this.walkRepository = walkRepository;
            Mapper = mapper;
            this.regionRepository = regionRepository;
            this.walkDifficultyRepository = walkDifficultyRepository;
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
            //Validate incomming request
            if(!(await ValidateAddAsync(addWalkRequest)))
            {
                return BadRequest(ModelState);
            }

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
            if(!(ValidateUpdateWalk(updateWalkRequest)))
            {
                return BadRequest(ModelState);
            }

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

        #region Private methods

        private async Task<bool> ValidateAddAsync(Models.DTO.AddWalkRequest addWalkRequest)
        {
            if(addWalkRequest == null)
            {
                ModelState.AddModelError(nameof(addWalkRequest),
                $"{nameof(addWalkRequest)} can't be null");

                return false;
            }

            if(string.IsNullOrWhiteSpace(addWalkRequest.Name))
            {
                ModelState.AddModelError(nameof(addWalkRequest.Name),
                    $"{nameof(addWalkRequest.Name)} can't be null or white space");
            }

            if (addWalkRequest.Length <= 0)
            {
                ModelState.AddModelError(nameof(addWalkRequest.Length),
                    $"{nameof(addWalkRequest.Length)} can't be null or white space");
            }

            var region = await regionRepository.Get(addWalkRequest.RegionId);
            if (region == null)
            {
                ModelState.AddModelError(nameof(addWalkRequest.RegionId),
                    $"{nameof(addWalkRequest.RegionId)} Region not found in database");
            }

            var walkDifficulty = await walkDifficultyRepository.GetAsync(addWalkRequest.WalkDifficultyId);
            if (walkDifficulty == null)
            {
                ModelState.AddModelError(nameof(addWalkRequest.WalkDifficultyId),
                    $"{nameof(addWalkRequest.WalkDifficultyId)} Region not found in database");
            }

            if (ModelState.ErrorCount > 0) return false;

            return true;
        }

        private bool ValidateUpdateWalk(Models.DTO.UpdateWalkRequest updateWalkRequest)
        {
           if(updateWalkRequest == null)
           {
                ModelState.AddModelError(nameof(updateWalkRequest), 
                    $"{nameof(updateWalkRequest)} can't be null");
                return false;
           }

           if(string.IsNullOrWhiteSpace(updateWalkRequest.Name))
           {
                ModelState.AddModelError(nameof(updateWalkRequest.Name),
                    $"{nameof(updateWalkRequest.Name)} can't be null or whitespace");
           }

            if (updateWalkRequest.Length <= 0)
            {
                ModelState.AddModelError(nameof(updateWalkRequest.Length),
                    $"{nameof(updateWalkRequest.Length)} can't be null or less");
            }

            if (ModelState.ErrorCount > 0) return false;

            return true;
        }

        #endregion
    }
}