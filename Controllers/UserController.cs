﻿using Microsoft.AspNetCore.Mvc;
using mpp_app_backend.Models;
using mpp_app_backend.Interfaces;
using mpp_app_backend.Exceptions;
using Microsoft.AspNetCore.Cors;
using mpp_app_backend.Services;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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
                Console.WriteLine(e.Message);
                return NotFound();
            }
            catch (Exception e)
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
                    Console.WriteLine(e.Message);
                    continue;
                }
            }

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
                user.ID = id;
                _userRepository.UpdateUser(user);
                return Ok();
            }
            catch (UserNotFoundException e)
            {
                Console.WriteLine(e.Message);
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
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
                Console.WriteLine(e.Message);
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
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
