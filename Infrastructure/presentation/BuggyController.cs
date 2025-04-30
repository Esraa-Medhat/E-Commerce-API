using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace presentation
{
    [ApiController]
    [Route("api/[Controller]")]
    public class BuggyController:ControllerBase
    {
        [HttpGet("notfound")]
       public IActionResult GetNotFoundRequest()
        {
            return NotFound();//404
        }
        [HttpGet("servererror")]
        public IActionResult GetServerErrorRequest()
        {
            throw new Exception();
            return Ok();
        }
        [HttpGet("badrequest")]
        public IActionResult GetBadRequest()
        {
            return BadRequest();//400
        }
        [HttpGet("badrequest/{id}")]
        public IActionResult GetBadRequest(int id)//validation error
        {
            return BadRequest();//400
        }
        [HttpGet("unauthorized")]
        public IActionResult GetUnauthorizedRequest()
        {
            return Unauthorized();//401
        }


    }
}
