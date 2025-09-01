using AutoMapper;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [Route("api/cities/{cityId}/pointsofinterest")]
    [ApiController]
    public class PointsOfInterestController : ControllerBase
    {
        private readonly ILogger<PointsOfInterestController> _logger;
        private readonly IMailService _mailService;
        private readonly IMapper _mapper;
        private readonly ICityInfoRepository _cityInfoRepository;

        public PointsOfInterestController (
            ILogger<PointsOfInterestController> logger, 
            IMailService mailService, 
            ICityInfoRepository cityInfoRepository, 
            IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException (nameof (logger));
            _mailService = mailService ?? throw new ArgumentNullException(nameof (_mailService));
            _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PointOfInterestDto>>> GetPointsOfInterest(int cityId)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                _logger.LogInformation($"City with id {cityId} wasn't found when accessing points of interest.");
                return NotFound();
            }
            var pointsOfInterestForCity = await 
                _cityInfoRepository.GetPointOfInterestsForcityAsync(cityId);
            return Ok(_mapper.Map<IEnumerable<PointOfInterestDto>>(pointsOfInterestForCity));
            // Old code before repository pattern
            // if (city == null)
            // {
            //     return NotFound();
            // }
            // return Ok(city.PointsOfInterest);
        }

        [HttpGet("{pointofinterestid}", Name = "GetPointOfInterest")]
        public async Task<ActionResult<PointOfInterestDto>> GetPointOfInterest(int cityId, int pointOfInterestId)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                _logger.LogInformation($"City with id {cityId} wasn't found when accessing points of interest.");
                return NotFound();
            }
            var pointOfInterest = await 
                _cityInfoRepository.GetPointOfInterestForCityAsync(cityId, pointOfInterestId);
            if (pointOfInterest == null)
            {
                _logger.LogInformation($"Point of interest with id {pointOfInterestId} wasn't found when accessing points of interest for city with id {cityId}.");
                return NotFound();
            }

            return Ok(_mapper.Map<PointOfInterestDto>(pointOfInterest));
            // Old code before repository pattern
            //var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
            //if (city == null)
            //{
            //    return NotFound();
            //}

            //var pointOfInterest = city.PointsOfInterest.FirstOrDefault(poi => poi.Id == pointOfInterestId);
            //if (pointOfInterest == null)
            //{
            //    return NotFound();
            //}

            //return Ok(pointOfInterest);
        }

        //[HttpPost]
        //public ActionResult<PointOfInterestDto> CreatePointOfInterest(int cityId, PointOfInterestForCreationDto pointOfInterest)
        //{
        //    var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
        //    if (city == null)
        //    {
        //        return NotFound();
        //    }

        //    var maxPointOfInterestId = _citiesDataStore.Cities.SelectMany(c => c.PointsOfInterest).Max(p => p.Id);

        //    var finalPointOfInterest = new PointOfInterestDto()
        //    {
        //        Id = ++maxPointOfInterestId,
        //        Name = pointOfInterest.Name,
        //        Description = pointOfInterest.Description,
        //    };

        //    city.PointsOfInterest.Add(finalPointOfInterest);

        //    return CreatedAtRoute("GetPointOfInterest",
        //        new
        //        {
        //            cityId = cityId,
        //            pointOfInterestId = finalPointOfInterest.Id
        //        },
        //        finalPointOfInterest);
        //}

        //[HttpPut("{pointofinterestid}")]
        //public ActionResult UpdatePointOfInterest(int cityid, int pointOfInterestId, PointOfInterestForUpdateDto pointOfInterest)
        //{
        //    var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityid);
        //    if (city == null)
        //    {
        //        return NotFound();
        //    }

        //    var poi = city.PointsOfInterest
        //        .FirstOrDefault(p => p.Id == pointOfInterestId);
        //    if (poi == null)
        //    {
        //        return NotFound();
        //    }

        //    poi.Name = pointOfInterest.Name;
        //    poi.Description = pointOfInterest.Description;

        //    return NoContent();
        //}
        //[HttpPatch("{pointofinterestid}")]
        //public ActionResult PartiallyUpdatePointOfInterest(
        //    int cityId, int pointOfInterestId,
        //    JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument)
        //{
        //    var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
        //    if (city == null)
        //    {
        //        return NotFound();
        //    }
        //    var poi = city.PointsOfInterest.FirstOrDefault(p => p.Id == pointOfInterestId);
        //    if (poi == null)
        //    {
        //        return NotFound();
        //    }
        //    var poiPatch =
        //        new PointOfInterestForUpdateDto()
        //        {
        //            Name = poi.Name,
        //            Description = poi.Description
        //        };
        //    patchDocument.ApplyTo(poiPatch, ModelState);

        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (!TryValidateModel(poiPatch))
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    poi.Name = poiPatch.Name;
        //    poi.Description = poiPatch.Description;

        //    return NoContent();

        //}

        //[HttpDelete("{pointOfInterestId}")]
        //public ActionResult DeletePointOfInterest(int cityId, int pointOfInterestId)
        //{
        //    var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
        //    if (city == null)
        //    {
        //        return NotFound();
        //    }
        //    var poi = city.PointsOfInterest.FirstOrDefault(p => p.Id == pointOfInterestId);
        //    if (poi == null)
        //    {
        //        return NotFound();
        //    }

        //    city.PointsOfInterest.Remove(poi);

        //    _mailService.Send("Point of interest delted.", $"Point of interest {poi.Name} with id {poi.Id} was deleted.");
        //    return NoContent();
        //}
    }
}
