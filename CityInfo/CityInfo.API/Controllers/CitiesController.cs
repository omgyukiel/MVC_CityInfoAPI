using CityInfo.API.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace CityInfo.API.Controllers
{
    [ApiController]
    [Route("api/cities")]
    public class CitiesController: ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<CityDto>> GetCities()
        {
            return Ok(CitiesDataStore.Instance.Cities);
        }

        [HttpGet("{id}")]
        public ActionResult<CityDto> GetCity(int id)
        {
            // find city
            var citytoReturn = CitiesDataStore.Instance.Cities
                .FirstOrDefault(c => c.Id == id);

            if (citytoReturn == null)
            {
                return NotFound();
            }
            return Ok(citytoReturn);

        }
    }
}
