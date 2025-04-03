using Database;
using Database.Models.Domain;
using Database.Repositories.Interfaces;
using Database.Utils;
using Microsoft.AspNetCore.Mvc;
using Database.Mappings;
using AutoMapper;
using Database.Models.DTOs.User;
using Microsoft.AspNetCore.Authorization;

namespace Server.Controllers;


[ApiController]
[Route("api/users")]
[Authorize]
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
    /// <returns>Returns <see cref="StatusCodes.Status201Created"/> if deleted successfully.</returns>
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteUser()
    {
        
        try
        {
            var UserId = UserService.ExtractUserIDFromToken(Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last());

            if (string.IsNullOrEmpty(UserId))
                return Unauthorized("User ID is not found in the token.");

            var userExists = await _userRepository.GetUser(UserId);
            if (userExists == null)
                return NotFound("User with the given UserId does not exist.");

            return await _userRepository.DeleteUser(userExists) ? Ok("User deleted successfully.") : BadRequest("User deletion failed.");
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
    public async Task<IActionResult> FindUser([FromRoute] string id)
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

}



