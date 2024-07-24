using Microsoft.AspNetCore.Mvc;
using DAL;
using BL;
using BL.Interfaces;
using DAL.DTO;
using BL.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Project.middleWare;
using Image_Encryption.midlleware;
using Microsoft.AspNetCore.Authorization;

namespace Image_Encryption.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService _IUserService;

        private readonly JwtTokenMiddleware _JwtTokenMiddleware;

        public UserController(IUserService userService, JwtTokenMiddleware JwtTokenMiddleware)
        {
            _IUserService = userService;
            _JwtTokenMiddleware = JwtTokenMiddleware;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromQuery] int id, [FromQuery] string password)
        {
            try
            {
                var user = await _IUserService.Loggin(id, password);
                if (user == null)
                    return Unauthorized();

                var token = _JwtTokenMiddleware.GenerateToken(id.ToString());
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                throw;
            }
            
        }
    


        [HttpPost]
        public async Task<ActionResult> AddUser([FromBody] UserDto newUser)
        {
            try
            {
                var res = await _IUserService.AddUser(newUser);
                if (res)
                    return Ok(res);
                return BadRequest(res);
            }
            catch (Exception ex)
            {
                // Log the exception (ex)
                return StatusCode(500, "An error occurred while adding the user.");
            }
        }

        [Authorize]
        [HttpPut]
        public async Task<ActionResult> UpdateUser([FromBody] UserDto user)
        {
            try
            {
                var res = await _IUserService.UpdateUser(user);
                if (res)
                    return Ok(res);
                return BadRequest(res);
            }
            catch (Exception ex)
            {
                // Log the exception (ex)
                return StatusCode(500, "An error occurred while updating the user.");
            }
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUserById(int id)
        {
            try
            {
                var user = await _IUserService.GetUserById(id);
                if (user == null)
                    return NotFound("User not found.");
                return Ok(user);
            }
            catch (Exception ex)
            {
                // Log the exception (ex)
                return StatusCode(500, "An error occurred while retrieving the user.");
            }
        }

        [Authorize]
        [HttpGet("GetAllUsers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ICollection<UserDto>>> GetAllUsers()
        {
            try
            {
                var users = await _IUserService.GetAllUsers();
                if (users == null || users.Count == 0)
                {
                    return NotFound("No users found.");
                }
                return Ok(users);
            }
            catch (Exception ex)
            {
                // Log the exception (ex)
                return StatusCode(500, "An error occurred while retrieving users.");
            }
        }

        [Authorize]
        [HttpPut("ResetPassword")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> ResetPassword([FromQuery] string password1, [FromQuery] string password2, [FromQuery] int id, [FromQuery] string validation = " ")
        {
            try
            {
                if (password1 != password2)
                {
                    return BadRequest("Passwords do not match.");
                }

                // Validate user ID and perform the password reset

                // Perform the password reset operation
                bool result = await _IUserService.ResetPassword(password1, password2, id, validation);

                if (result)
                {
                    return Ok("Password has been reset successfully.");
                }
                else
                {
                    return StatusCode(500, "An error occurred while resetting the password.");
                }
            }
            catch (Exception ex)
            {
                // Log the exception (ex)
                return StatusCode(500, "An error occurred while resetting the password.");
            }
        }

    }
}
