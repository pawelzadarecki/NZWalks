using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.Api.Repository;

namespace NZWalks.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class WalkDifficultyController : Controller
    {
        private readonly IWalkDifficultyRepository walkDifficultyRepository;
        private readonly IMapper mapper;

        public WalkDifficultyController(IWalkDifficultyRepository walkDifficultyRepository, IMapper mapper)
        {
            this.walkDifficultyRepository = walkDifficultyRepository;
            this.mapper = mapper;
        }

        [HttpGet]

        public async Task<IActionResult> GetAllWlkDifficultyAsync()
        {
            //fetch data from database
            var walkDifficultyDomain = await walkDifficultyRepository.GetAllAsync();

            //check if null
            if(walkDifficultyDomain == null)
            {
                return NotFound();
            }

            //convert to DTO
            var walkDifficultyDTO = mapper.Map<List<Models.DTO.WalkDifficulty>>(walkDifficultyDomain);

            //return respond
            return Ok(walkDifficultyDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkDifficulty")]
        public async Task<IActionResult> GetWalkDifficulty(Guid id)
        {
            //get from database by id
            var walkDifficultyDomain = await walkDifficultyRepository.GetAsync(id);

            // check if null
            if(walkDifficultyDomain == null)
            {
                return NotFound();
            }

            //Convert to DTO
            var walkDifficultyDTO = new Models.DTO.WalkDifficulty()
            {
                ID = walkDifficultyDomain.ID,
                Code = walkDifficultyDomain.Code,
            };

            //return respodn
            return Ok(walkDifficultyDTO);
        }

        [HttpPost]

        public async Task<IActionResult> AddWalkDifficultyAsync(Models.DTO.AddWalkDifficultyRequest addWalkDifficultyRequest)
        {
            //Validate incoming request
            if(!(ValidateAddWalkDifficultyAsync(addWalkDifficultyRequest)))
            {
                return BadRequest(ModelState);
            }

            //Convert DTO to Domain
            var walkDifficultyDomain = new Models.Domian.WalkDifficulty()
            {
                Code = addWalkDifficultyRequest.Code,
            };

            //Add Domain to database
            walkDifficultyDomain = await walkDifficultyRepository.AddAsync(walkDifficultyDomain);

            //Convert back to DTO
            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficultyDomain);

            //Return respond
            return CreatedAtAction(nameof(GetWalkDifficulty), new { Id = walkDifficultyDTO.ID }, walkDifficultyDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]

        public async Task<IActionResult> UpdateWalkDifficultyAsync([FromBody]Models.DTO.UpdateWalkDifficultyRequest updateWalkDifficultyRequest, [FromRoute]Guid id)
        {
            //Validate incomming request
            if (!(ValidateUpdateWalkDifficultyAsync(updateWalkDifficultyRequest)))
            {
                return BadRequest(ModelState);
            }

            //Convert to Domain
            var walkDifficultyDomain = new Models.Domian.WalkDifficulty()
            {
                Code = updateWalkDifficultyRequest.Code,
            };

            //Update database
            walkDifficultyDomain = await walkDifficultyRepository.UpdateAsync(walkDifficultyDomain, id);


            //check if null
            if (walkDifficultyDomain == null) return NotFound();

            //Convert Domain to DTO
            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficultyDomain);

            //return respond
            return Ok(walkDifficultyDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]

        public async Task<IActionResult> DeleteWalkDifficulty(Guid id)
        {
            //delete from database
            var walkDifficultyDomain = await walkDifficultyRepository.DeleteAsync(id);

            //Convert domain to DTO
            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficultyDomain);

            //Return respond
            return Ok(walkDifficultyDTO);
        }


        #region private methods

        private bool ValidateAddWalkDifficultyAsync(Models.DTO.AddWalkDifficultyRequest addWalkDifficultyRequest)
        {
            if(addWalkDifficultyRequest == null)
            {
                ModelState.AddModelError(nameof(addWalkDifficultyRequest), 
                    $"{nameof(addWalkDifficultyRequest)} can't be null");
                return false;
            }

            if (string.IsNullOrWhiteSpace(addWalkDifficultyRequest.Code))
            {
                ModelState.AddModelError(nameof(addWalkDifficultyRequest.Code),
                    $"{nameof(addWalkDifficultyRequest.Code)} can't be null or white space");
            }

            if(ModelState.ErrorCount > 0)
            {
                return false;
            }

            return true;
        }

        private bool ValidateUpdateWalkDifficultyAsync(Models.DTO.UpdateWalkDifficultyRequest updateWalkDifficultyRequest)
        {
            if (updateWalkDifficultyRequest == null)
            {
                ModelState.AddModelError(nameof(updateWalkDifficultyRequest),
                    $"{nameof(updateWalkDifficultyRequest)} can't be null");
                return false;
            }

            if (string.IsNullOrWhiteSpace(updateWalkDifficultyRequest.Code))
            {
                ModelState.AddModelError(nameof(updateWalkDifficultyRequest.Code),
                    $"{nameof(updateWalkDifficultyRequest.Code)} can't be null or white space");
            }

            if (ModelState.ErrorCount > 0)
            {
                return false;
            }

            return true;
        }

        #endregion
    }
}
