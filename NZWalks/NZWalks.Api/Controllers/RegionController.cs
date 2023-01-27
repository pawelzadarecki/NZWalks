using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NZWalks.Api.Repository;

namespace NZWalks.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class RegionController : Controller
    {
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionController(IRegionRepository regionRepository, IMapper mapper)
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "reader")]

        public async Task<IActionResult> GetAllRegionsAsync()
        {
            var regions = await regionRepository.GetAllAsync();

            //var regionsDTO = new List<Models.DTO.Region>();
            //regions.ToList().ForEach(region =>
            //{
            //    var regionDTO = new Models.DTO.Region()
            //    {
            //        Id = region.Id,
            //        Name = region.Name,
            //        Area = region.Area,
            //        lat = region.lat,
            //        Long = region.Long,
            //        Population = region.Population,
            //    };

            //    regionsDTO.Add(regionDTO);
            //});

            var regionsDTO = mapper.Map<List<Models.DTO.Region>>(regions);

            return Ok(regionsDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetRegionAsync")]
        [Authorize(Roles = "reader")]

        public async Task<IActionResult> GetRegionAsync(Guid id)
        {
            var region = regionRepository.Get(id);

            if (region == null)
            {
                return NotFound();
            }

            var regionDTO = mapper.Map<Models.DTO.Region>(region);

            return Ok(regionDTO);
        }

        [HttpPost]
        [Authorize(Roles ="writer")]

        public async Task<IActionResult> AddRegionAsync(Models.DTO.AddRegionRequest addRegionRequest)
        {
            //Validate DTO 
            //if(!ValidateAddRegionAsync(addRegionRequest))
            //{
            //    return BadRequest(ModelState);
            //} 

            // Repository(DTO) to Domain Model
            var region = new Models.Domian.Region()
            {
                Code = addRegionRequest.Code,
                Name = addRegionRequest.Name,
                Area = addRegionRequest.Area,
                lat = addRegionRequest.lat,
                Long = addRegionRequest.Long,
                Population = addRegionRequest.Population,
            };

            //Pass details to Repository
            region = await regionRepository.AddAsync(region);

            //Convert back to DTO
            var regionDTO = new Models.DTO.Region()
            {
                Id = region.Id,
                Code = region.Code,
                Name = region.Name,
                Area = region.Area,
                lat = region.lat,
                Long = region.Long,
                Population = region.Population,
            };

            return CreatedAtAction(nameof(GetRegionAsync), new { Id = regionDTO.Id }, regionDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        [Authorize(Roles = "writer")]

        public async Task<IActionResult> DeleteRegion(Guid id)
        {
            // Delete region from database
            var region = await regionRepository.DeleteAsync(id);

            //Check if null 
            if (region == null)
            {
                return NotFound();
            }

            //Conver to DTO
            var regionDTO = new Models.DTO.Region()
            {
                Id = region.Id,
                Code = region.Code,
                Name = region.Name,
                Area = region.Area,
                lat = region.lat,
                Long = region.Long,
                Population = region.Population,
            };

            // return ok result
            return Ok(regionDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]
        [Authorize(Roles = "writer")]

        public async Task<IActionResult> UpdateRegion([FromRoute] Guid id, 
            [FromBody] Models.DTO.UpdateRegionRequest updateRegionRequest)
        {
            //Validate data request
            //if (!ValidateUpdateRegion(updateRegionRequest)) return BadRequest(ModelState);

            var region = new Models.Domian.Region()
            {

                Code = updateRegionRequest.Code,
                Name = updateRegionRequest.Name,
                Area = updateRegionRequest.Area,
                lat = updateRegionRequest.lat,
                Long = updateRegionRequest.Long,
                Population = updateRegionRequest.Population,
            };

            // Save Domain Region to Db
            region = await regionRepository.UpdateAsync(id, region);

            //if null NotFound
            if (region == null)
            {
                return NotFound();
            }

            //Convert back to DTO
            var RegionDTO = new Models.DTO.Region()
            {
                Id = region.Id,
                Code = region.Code,
                Name = region.Name,
                Area = region.Area,
                lat = region.lat,
                Long = region.Long,
                Population = region.Population,
            };

            //Return ok response
            return Ok(RegionDTO);
        }

        #region Private methods

        private bool ValidateAddRegionAsync(Models.DTO.AddRegionRequest addRegionRequest)
        {
            if(string.IsNullOrWhiteSpace(addRegionRequest.Code))
            {
                ModelState.AddModelError(nameof(addRegionRequest.Code), 
                    $"{nameof(addRegionRequest.Code)} Can't be null or white space");
            }

            if (string.IsNullOrWhiteSpace(addRegionRequest.Name))
            {
                ModelState.AddModelError(nameof(addRegionRequest.Name),
                    $"{nameof(addRegionRequest.Name)} Can't be null or white space");
            }
            if (addRegionRequest.Area <= 0)
            {
                ModelState.AddModelError(nameof(addRegionRequest.Area),
                    $"{nameof(addRegionRequest.Area)} Can't be 0 or less");
            }
            if (addRegionRequest.lat <= 0)
            {
                ModelState.AddModelError(nameof(addRegionRequest.lat),
                    $"{nameof(addRegionRequest.lat)} Can't be 0 or less");
            }
            if (addRegionRequest.Long <= 0)
            {
                ModelState.AddModelError(nameof(addRegionRequest.Long),
                    $"{nameof(addRegionRequest.Long)} Can't be 0 or less");
            }
            if (addRegionRequest.Population < 0)
            {
                ModelState.AddModelError(nameof(addRegionRequest.Population),
                    $"{nameof(addRegionRequest.Population)} Can't be less than 0");
            }

            if (ModelState.ErrorCount > 0) return false;

            return true;
        }

        private bool ValidateUpdateRegion(Models.DTO.UpdateRegionRequest updateRegionRequest)


        {
            if (string.IsNullOrWhiteSpace(updateRegionRequest.Code))
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Code),
                    $"{nameof(updateRegionRequest.Code)} Can't be null or white space");
            }
            if (string.IsNullOrWhiteSpace(updateRegionRequest.Name))
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Name),
                    $"{nameof(updateRegionRequest.Name)} Can't be null or white space");
            }
            if (updateRegionRequest.Area <= 0)
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Area),
                    $"{nameof(updateRegionRequest.Area)} Can't be 0 or less");
            }
            if (updateRegionRequest.lat <= 0)
            {
                ModelState.AddModelError(nameof(updateRegionRequest.lat),
                    $"{nameof(updateRegionRequest.lat)} Can't be 0 or less");
            }
            if (updateRegionRequest.Long <= 0)
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Long),
                    $"{nameof(updateRegionRequest.Long)} Can't be 0 or less");
            }
            if (updateRegionRequest.Population < 0)
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Population),
                    $"{nameof(updateRegionRequest.Population)} Can't be less than 0");
            }

            if (ModelState.ErrorCount > 0) return false;

            return true;
        }

        #endregion

    }
}
