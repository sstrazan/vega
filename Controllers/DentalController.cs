using Microsoft.AspNetCore.Mvc;
using Vega.Controllers.Resources;

namespace Vega.Controllers
{
    public class DentalController : Controller
    {

        [Route("/api/dental/users/register")]
        [HttpPost]
        public IActionResult RegisterUser([FromBody] UserResource userResource)
        {
            if (string.IsNullOrEmpty(userResource.FirstName))
                return BadRequest();
            else
                return Ok(userResource);
        }

    }
}