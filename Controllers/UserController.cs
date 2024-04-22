using Microsoft.AspNetCore.Mvc;
using mpp_app_backend.Models;
using mpp_app_backend.Interfaces;
using mpp_app_backend.Exceptions;
using Microsoft.AspNetCore.Cors;
using mpp_app_backend.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace mpp_app_backend.Controllers
{
    [EnableCors("AllowFrontendOrigin")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly UserServices _userServices;

        public UserController(IUserRepository repo, UserServices serv)
        {
            _userRepository = repo;
            _userServices = serv;
        }

        // GET: api/<UserController>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<User>))]
        public IActionResult GetUsers()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var users = _userRepository.GetUsers();
            return Ok(users);
        }

        // GET api/<UserController>/{id}
        [HttpGet("{id}")]
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
                var user = _userRepository.GetUserById(id);

                return Ok(user);
            }
            catch (UserNotFoundException e)
            {
                Console.WriteLine(e.Message);
                return NotFound();
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest();
            }
        }

        // POST api/<UserController>
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(User))]
        public IActionResult AddUser([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            user.ID = Guid.NewGuid().ToString();
            _userRepository.AddUser(user);

            return CreatedAtAction("GetUserById", new { id = user.ID }, user);
        }

        // PUT api/<UserController>/{id}
        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult UpdateUser(string id, [FromBody] User user)
        {   
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _userRepository.UpdateUser(id, user.Username, user.Email, user.Password, user.Avatar, user.Birthdate, user.RegisteredAt);
                return Ok();
            }
            catch (UserNotFoundException e)
            {
                Console.WriteLine(e.Message);
                return NotFound();
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest();
            }
        }

        // DELETE api/<UserController>/{id}
        [HttpDelete("{id}")]
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
                _userRepository.DeleteUser(id);
                return Ok();
            }
            catch (UserNotFoundException e)
            {
                Console.WriteLine(e.Message);
                return NotFound();
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest();
            }
        }

        // GET api/<UserController>/getUsersPerYear
        [HttpGet("getUsersPerYear")]
        [ProducesResponseType(200, Type = typeof(IDictionary<int, int>))]
        public IActionResult GetUsersPerYear()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var usersPerYear = _userServices.GetNumberOfUsersByRegistrationYear();
            return Ok(usersPerYear);
        }
    }
}
