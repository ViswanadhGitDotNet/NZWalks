using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalksAPI.CustomActionFilters;
using NZWalksAPI.Data;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO;
using NZWalksAPI.Repositories;
using System.Text.Json;

namespace NZWalksAPI.Controllers
{
    //https:localhost:1234/api/Regions
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;
        private readonly ILogger<RegionsController> logger;

        public RegionsController(NZWalksDbContext dbContext,IRegionRepository regionRepository,IMapper mapper,ILogger<RegionsController> logger)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
            this.logger = logger;
        }
        //GET ALL REGIONS
        //GET: https:localhost:1234/api/Regions
        [HttpGet]
       /* [Authorize(Roles ="Reader")]*/
        public async Task<IActionResult> GetAll()
        {
            try
            {
                //throw new Exception("This is a custom Exception");
                //logger.LogInformation("GetAllRegions Action Method was invoked")
                //Get Data From Database- Domain models
                var regionsDomain = await regionRepository.GetAllAsync();

                //logger.LogInformation($"Finished GetAllRegions request with data: {JsonSerializer.Serialize(regionsDomain)}");
                //Map Domain Models to DTOs

                /*  var regionsDto = new List<RegionDto>();
                  foreach (var regionDomain in regionsDomain)
                  {
                      regionsDto.Add(new RegionDto()
                      {
                          Id = regionDomain.Id,
                          Code = regionDomain.Code,
                          Name = regionDomain.Name,
                          RegionImageUrl = regionDomain.RegionImageUrl,
                      });
                  }*/

                //Map Domain Models to DTOs
                var regionsDto = mapper.Map<List<RegionDto>>(regionsDomain);

                //Return DTOs
                return Ok(regionsDto);
            }
            catch (Exception ex)
            {
                logger.LogError(ex,ex.Message);
                throw;
            }
        }


        //GET SINGLE REGION (Get Region By ID)
        //GET: https://localhost:portnumber/api/regions/{id}

        [HttpGet]
        [Route("{id:Guid}")]
        //[Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            //var region=dbContext.Regions.Find(id);

            //Get Region Domain Model From DataBase
            var regionDomain = await regionRepository.GetByIdASync(id);
            if (regionDomain == null)
            {
                return NotFound();
            }
            //Map/Convert Region Domain Model to Region DTO
/*
            var regionsDto = new RegionDto
            {
                Id = regionDomain.Id,
                Code = regionDomain.Code,
                Name = regionDomain.Name,
                RegionImageUrl = regionDomain.RegionImageUrl,
            };*/
              
           var regionsDto= mapper.Map<RegionDto>(regionDomain);
            return Ok(regionsDto);
        }

        //Post To Create New Region
        //POST: https://localhost:portnumber/api/regions

        [HttpPost]
        [ValidateModel]
      /*  [Authorize(Roles = "Writer")]*/
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
                //Map or convert DTO to Domain Model
                /* var regionDomainModel = new Region
                 {
                     Code = addRegionRequestDto.Code,
                     Name = addRegionRequestDto.Name,
                     RegionImageUrl = addRegionRequestDto.RegionImageUrl,
                 };*/
                var regionDomainModel = mapper.Map<Region>(addRegionRequestDto);
                //Use Domain Model to create Region
                regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);

                //Map Domaim Model back to DTO
                /* var regionDto = new RegionDto
                 {
                     Id = regionDomainModel.Id,
                     Code = regionDomainModel.Code,
                     Name = regionDomainModel.Name,
                     RegionImageUrl = regionDomainModel.RegionImageUrl,
                 };
                 */
                var regionDto = mapper.Map<RegionDto>(regionDomainModel);

                return CreatedAtAction(nameof(GetById), new { id = regionDomainModel.Id }, regionDto);

    }


        //Update the Region
        //PUT: https://localhost:portnumber/api/region/{id}

        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
     /*   [Authorize(Roles = "Writer")]*/

        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
               /* var regionDomainModel = new Region
                 {
                     Code = updateRegionRequestDto.Code,
                     Name = updateRegionRequestDto.Name,
                     RegionImageUrl = updateRegionRequestDto.RegionImageUrl,
                 };
                 */
                var regionDomainModel = mapper.Map<Region>(updateRegionRequestDto);
                //Check if region exists
                regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel);

                if (regionDomainModel == null)
                {
                    return NotFound();
                }

                //Convert DomainModel to DTO

                /*var regionDto = new RegionDto
                {
                    Id = regionDomainModel.Id,
                    Code = regionDomainModel.Code,
                    Name = regionDomainModel.Name,
                    RegionImageUrl = regionDomainModel.RegionImageUrl,
                };*/
                var regionDto = mapper.Map<RegionDto>(regionDomainModel);
                return Ok(regionDto);
        }


        //Delete a Region
        //DELETE: https://localhost:portnumber/api/regions/{id}

        [HttpDelete]
        [Route("{id:Guid}")]
       /* [Authorize(Roles = "Writer,Reader")]*/
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var regionDomainModel=await regionRepository.DeleteAsync(id);
            if(regionDomainModel==null)
            {
                return NotFound();
            }

            //Map DomainModel to DTO

            /*  var regionDto = new RegionDto
              {
                  Id = regionDomainModel.Id,
                  Code = regionDomainModel.Code,
                  Name = regionDomainModel.Name,
                  RegionImageUrl = regionDomainModel.RegionImageUrl,
              };*/
            var regionDto = mapper.Map<RegionDto>(regionDomainModel);
            //return the deleted region back
            return Ok(regionDto);
        }
    }
}
