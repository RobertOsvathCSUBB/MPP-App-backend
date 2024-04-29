using Microsoft.AspNetCore.Mvc;
using mpp_app_backend.Models;
using mpp_app_backend.Interfaces;
using mpp_app_backend.Exceptions;
using Microsoft.AspNetCore.Cors;
using mpp_app_backend.Services;
using Microsoft.EntityFrameworkCore;

namespace mpp_app_backend.Controllers
{
    [EnableCors("AllowFrontendOrigin")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ILoginActivityRepository _loginActivityRepository;
        private readonly UserServices _userServices;

        public UserController(IUserRepository userRepo, ILoginActivityRepository laRepo, UserServices serv)
        {
            _userRepository = userRepo;
            _loginActivityRepository = laRepo;
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

        // GET: api/<UserController>/totalUsersCount
        [HttpGet("totalUsersCount")]
        [ProducesResponseType(200, Type = typeof(int))]
        public IActionResult GetTotalUsersCount()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var totalUsersCount = _userRepository.GetUsers().Count;
            return Ok(totalUsersCount);
        }

        // GET: api/<UserController>/pages?page={page}&pageSize={pageSize}
        [HttpGet("pages")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<User>))]
        public IActionResult GetUsersPaginated(int page, int pageSize)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var users = _userRepository.GetUsersPaginated(page, pageSize);
            return Ok(users);
        }

        // GET: api/<UserController>/sorted
        [HttpGet]
        [Route("sorted")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<User>))]
        public IActionResult GetUsersSorted()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var users = _userRepository.GetUsersSorted();
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
                return NotFound();
            }
            catch (Exception e)
            {
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

            while (true)
            {
                try
                {
                    user.ID = Guid.NewGuid().ToString();
                    _userRepository.AddUser(user);
                    break;
                }
                catch (DbUpdateException e)
                {
                    continue;
                }
            }

            return CreatedAtAction("GetUserById", new { id = user.ID }, user);
        }

        // PUT api/<UserController>/{id}
        [HttpPut("{id}")]
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
                user.ID = id;
                _userRepository.UpdateUser(user);
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
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.Message);
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
