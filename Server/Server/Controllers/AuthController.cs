using Database.Models.Domain;
using Database.Models.DTOs.User;
using Database.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenRepository _tokenRepository;

        public AuthController(UserManager<User> userManager, ITokenRepository tokenRepository)
        {
            _userManager = userManager;
            _tokenRepository = tokenRepository;
        }

        /// <summary>
        /// Add a new User.
        /// </summary>
        /// <param name="addNewUserDTO">The new user object to be added to the database.</param>
        /// <returns>Returns a token.</returns>
        [HttpPost]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] AddNewUserDTO addNewUserDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var identityUser = new User
                {
                    Email = addNewUserDTO.Email,
                    UserName = addNewUserDTO.Name,
                    DateCreated = DateTime.UtcNow
                };

                var identityResult = await _userManager.CreateAsync(identityUser, addNewUserDTO.Password);

                if (identityResult.Succeeded)
                    return Ok(new { Token = _tokenRepository.CreateJWTToken(identityUser) });
                else
                    return BadRequest(new { Errors = identityResult.Errors });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Register: {ex.Message}");

                return StatusCode(500, new { Message = "An unexpected error occurred. Please try again later." });
            }

        }


        /// <summary>
        /// Login a User.
        /// </summary>
        /// <param name="loginDTO">The user object to be logined to the database.</param>
        /// <returns>Returns a token.</returns>
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var user = await _userManager.FindByEmailAsync(loginDTO.Email);

                if (user != null)
                {
                    var checkPassword = await _userManager.CheckPasswordAsync(user, loginDTO.Password);

                    if (checkPassword)
                        return Ok(new { Token = _tokenRepository.CreateJWTToken(user) });
                }

                return NotFound("Invalid credtionals");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Login: {ex.Message}");

                return StatusCode(500, new { Message = "An unexpected error occurred. Please try again later." });
            }
        }
    }
}
