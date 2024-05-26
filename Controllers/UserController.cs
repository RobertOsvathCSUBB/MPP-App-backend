using Microsoft.AspNetCore.Mvc;
using mpp_app_backend.Models;
using mpp_app_backend.Interfaces;
using mpp_app_backend.Exceptions;
using Microsoft.AspNetCore.Cors;
using mpp_app_backend.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using mpp_app_backend.Context;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace mpp_app_backend.Controllers
{
    [EnableCors("AllowFrontendOrigin")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserService _userServices;
        private readonly AdminDataContext _adminContext;

        public UserController(IUserService serv, AdminDataContext adminContext)
        {
            _userServices = serv;
            _adminContext = adminContext;
        }

        // GET: api/<UserController>?email={email}&domain={domain}
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<User>))]
        public IActionResult GetUsers(string email, string domain)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var adminEmail = email + "@" + domain + ".com";
            var adminID = _adminContext.Users.FirstOrDefault(admin => admin.Email == adminEmail).Id;

            var users = _userServices.GetUsers(adminID);
            return Ok(users);
        }

        // GET: api/<UserController>/totalUsersCount?email={email}&domain={domain}
        [HttpGet("totalUsersCount"), Authorize]
        [ProducesResponseType(200, Type = typeof(int))]
        public IActionResult GetTotalUsersCount(string email, string domain)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var adminEmail = email + "@" + domain + ".com";
            var adminID = _adminContext.Users.FirstOrDefault(admin => admin.Email == adminEmail).Id;

            var totalUsersCount = _userServices.GetTotalUsersCount(adminID);
            return Ok(totalUsersCount);
        }

        // GET: api/<UserController>/pages?email={email}&domain={domain}&page={page}&pageSize={pageSize}
        [HttpGet("pages"), Authorize]
        [ProducesResponseType(200, Type = typeof(IEnumerable<User>))]
        public IActionResult GetUsersPaginated(string email, string domain, int page, int pageSize)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var adminEmail = email + "@" + domain + ".com";
            var adminID = _adminContext.Users.FirstOrDefault(admin => admin.Email == adminEmail).Id;

            var users = _userServices.GetUsersPaginated(adminID, page, pageSize);
            return Ok(users);
        }

        // GET: api/<UserController>/sorted
        /*[HttpGet]
        [Route("sorted"), Authorize]
        [ProducesResponseType(200, Type = typeof(IEnumerable<User>))]
        public IActionResult GetUsersSorted()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var users = _userServices.GetUsersSorted();
            return Ok(users);
        }*/

        // GET api/<UserController>/{id}
        [HttpGet("{id}"), Authorize]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(400)]
        public IActionResult GetUserById(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = _userServices.GetUserById(id);
                return Ok(user);
            }
            catch (UserNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // POST api/<UserController>?email={email}&domain={domain}
        [HttpPost, Authorize]
        [ProducesResponseType(201, Type = typeof(User))]
        public IActionResult AddUser(string email, string domain, [FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var adminEmail = email + "@" + domain + ".com";
            var adminID = _adminContext.Users.FirstOrDefault(admin => admin.Email == adminEmail).Id;

            _userServices.AddUser(adminID, user);
            return CreatedAtAction("GetUserById", new { id = user.ID }, user);
        }

        /*// POST api/<UserController>/addRange
        [HttpPost("addRange"), Authorize]
        [ProducesResponseType(201, Type = typeof(IEnumerable<User>))]
        public IActionResult AddUsers([FromBody] ICollection<User> users)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _userServices.AddUserRange(users);
            return CreatedAtAction("GetUsers", users);
        }*/

        // PUT api/<UserController>/{id}
        [HttpPut("{id}"), Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateUser(string id, [FromBody] User user)
        {   
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _userServices.UpdateUser(id, user);
                return Ok();
            }
            catch (UserNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // DELETE api/<UserController>/{id}
        [HttpDelete("{id}"), Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult DeleteUser(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _userServices.DeleteUser(id);
                return Ok();
            }
            catch (UserNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /*// GET api/<UserController>/getUsersPerYear
        [HttpGet("getUsersPerYear"), Authorize]
        [ProducesResponseType(200, Type = typeof(IDictionary<int, int>))]
        public IActionResult GetUsersPerYear()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var usersPerYear = _userServices.GetNumberOfUsersByRegistrationYear();
            return Ok(usersPerYear);
        }*/

        // POST api/<UserController>/{id}/loginActivity
        [HttpPost("{id}/loginActivity"), Authorize]
        [ProducesResponseType(201, Type = typeof(LoginActivity))]
        public IActionResult AddLoginActivity(string id, [FromBody] LoginActivity loginActivity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _userServices.AddLoginActivity(id, loginActivity);
            return CreatedAtAction("GetUserById", new { id = loginActivity.UserId }, loginActivity);
        }
    }
}
