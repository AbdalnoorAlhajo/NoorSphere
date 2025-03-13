using Database;
using Database.Models.Domain;
using Database.Repositories.Interfaces;
using Database.Utils;
using Microsoft.AspNetCore.Mvc;
using Database.Mappings;
using AutoMapper;
using Database.Models.DTOs.User;

namespace Server.Controllers;


[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UsersController(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }


    /// <summary>
    /// Registers a new user by adding them to the database.
    /// </summary>
    /// <param name="newUserDTO">The new user object to be added to the database.</param>
    /// <returns>Returns the user as <see cref="User"/> Object with the assigned ID if operation go will.</returns>
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddUser([FromBody] AddNewUserDTO newUserDTO)
    {
        try
        {           
            if(ModelState.IsValid)
            {
                var existingUser = await _userRepository.GetUser(newUserDTO.Email);
                if (existingUser != null)
                    return BadRequest("Email is already takens.");

                var newUser = _mapper.Map<User>(newUserDTO);

                // Hash the password before saving it to ensure it is stored securely in the database. 
                // This prevents storing plain-text passwords and adds an extra layer of security.
                newUser.PasswordHash = UserService.HashPassword(newUserDTO.Password);

                var createdUser = await _userRepository.AddUser(newUser);
                return CreatedAtAction(nameof(FindUser), new { id = createdUser.Id }, createdUser);
            }
            else
                return BadRequest(ModelState);
        }
        catch (Exception er)
        {
            return StatusCode(500, $"Internal server error: {er.Message}");
        }
    }

    /// <summary>
    /// Retrieves a user by their ID.
    /// </summary>
    /// <param name="id">The ID of the user to retrieve.</param>
    /// <returns>Returns the user as <see cref="GetUserDTO"/> Object if found, or a 404 error if not.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> FindUser([FromRoute] int id)
    {
        try
        {
            var user = await _userRepository.GetUser(id);

            if (user == null)
                return NotFound($"User with ID {id} not found.");

            return Ok(_mapper.Map<GetUserDTO>(user));
        }
        catch (Exception er)
        {
            return StatusCode(500, $"Internal server error: {er.Message}");
        }
    }

    /// <summary>
    /// Retrieves all users from the database.
    /// </summary>
    /// <returns>Returns a list of all users as <see cref="GetUserDTO"/> Objects.</returns>
    [HttpGet("all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetUsers()
    {
        try
        {
            var usersList = await _userRepository.GetAllUsers();

            if (usersList == null)
                return NotFound($"Users are not found.");

            return Ok(_mapper.Map<List<GetUserDTO>>(usersList));
        }
        catch (Exception er)
        {
            return StatusCode(500, $"Internal server error: {er.Message}");
        }
    }


    /// <summary>
    /// Logs in a user by verifying their email and password.
    /// </summary>
    /// <param name="email">The user's email.</param>
    /// <param name="password">The user's password.</param>
    /// <returns>Returns the user details if the credentials match as a <see cref="GetUserDTO"/> Object.</returns>
    [HttpGet("/login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetUser([FromQuery] string email, [FromQuery] string password)
    {
        try
        {
            var user = await _userRepository.GetUserByCredentials(email, password);

            if (user == null)
                return NotFound($"User with the provided credentials was not found.");

            return Ok(_mapper.Map<GetUserDTO>(user));
        }
        catch (Exception er)
        {
            return StatusCode(500, $"Internal server error: {er.Message}");
        }
    }

}



