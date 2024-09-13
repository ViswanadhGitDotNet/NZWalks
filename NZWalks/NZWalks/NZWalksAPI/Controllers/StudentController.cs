using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NZWalksAPI.Controllers
{
    //https:localhost:portnumber/api/student
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        //GET:
        [HttpGet]
        public IActionResult GetAllStudents()
        {
            string[] studenetNames = new string[] { "john", "jane", "Mark", "Emily", "David" };

            return Ok(studenetNames);
        }

    }
}
