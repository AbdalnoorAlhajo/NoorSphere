using Database;
using Database.Models.Domain;
using Database.Repositories.Interfaces;
using Database.Utils;
using Microsoft.AspNetCore.Mvc;
using Database.Mappings;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Database.Models.DTOs.UserAndRelatedEntities.User;
using Database.Models.DTOs.UserAndRelatedEntities.Follow;

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
    /// Deletes the current authenticated user.
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

    /// <summary>
    /// Add follow to specific user.
    /// </summary>
    /// <returns>Returns a follow as <see cref="GetFollowDTO"/> with asigned id.</returns>
    [HttpPost("follows")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddFollow([FromBody] AddNewFollowDTO addNewFollowDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        try
        {
            var followedUser = await _userRepository.GetUser(addNewFollowDTO.FollowedUserId);

            if(followedUser == null)
                return NotFound($"Followed user with ID {addNewFollowDTO.FollowedUserId} not found.");

            var followerUser = await _userRepository.GetUser(addNewFollowDTO.FollowerUserId);
            if (followerUser == null)
                return NotFound($"Follower user with ID {addNewFollowDTO.FollowerUserId} not found.");

            if(await _userRepository.IsFollowingExist(addNewFollowDTO.FollowerUserId, addNewFollowDTO.FollowedUserId))
                return BadRequest($"User with ID {addNewFollowDTO.FollowerUserId} is already following the user with ID {addNewFollowDTO.FollowedUserId}.");

            return Ok(_mapper.Map<Follow>(await _userRepository.AddFollow(addNewFollowDTO)));
        }
        catch (Exception er)
        {
            return StatusCode(500, $"Internal server error: {er.Message}");
        }
    }

    /// <summary>
    /// Get all follows to specific user.
    /// </summary>
    /// <returns>Returns follows as <see cref="GetFollowDTO"/> list.</returns>
    [HttpGet("follows/me")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetFollows()
    {
        try
        {
            var UserId = UserService.ExtractUserIDFromToken(Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last());

            if (string.IsNullOrEmpty(UserId))
                return Unauthorized("User ID is not found in the token.");

            var user = await _userRepository.GetUser(UserId);
            if (user == null)
                return NotFound("User with the given UserId does not exist.");

            var UserFollows = await _userRepository.GetAllFollows(user.Id);

            return UserFollows != null ? Ok(_mapper.Map<List<GetFollowDTO>>(UserFollows)) : NotFound($"User with ID {user.Id} does not have follows.");

        }
        catch (Exception er)
        {
            return StatusCode(500, $"Internal server error: {er.Message}");
        }
    }


    /// <summary>
    /// Retrieves the number of followers and following for the current user.
    /// </summary>
    /// <returns>Returns <see cref="FollowsAndFollwoingDTO"/> with follower and following counts.</returns>
    [HttpGet("FollowersAndFollwoing")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetFollowsAndFollowers(string userId)
    {
        try
        {
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User ID is not found in the token.");

            var user = await _userRepository.GetUser(userId);
            if (user == null)
                return NotFound("User does not exist.");

            var stats = await _userRepository.GetFollowsAndFollowers(userId);

            return Ok(stats);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }


}



