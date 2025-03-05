using Database;
using Database.Models;
using Database.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Server.Controllers;


[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    /// <summary>
    /// Registers a new user by adding them to the database.
    /// </summary>
    /// <param name="newUser">The new user object to be added to the database.</param>
    /// <returns>Returns the user as <see cref="User"/> Object with the assigned ID if operation go will.</returns>
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddUser([FromBody] User newUser)
    {
        try
        {
            if (newUser == null)
                return BadRequest("User data is missing.");
            

            using var dbUser = new NoorSphere();

            var existingUser = await dbUser.users.FirstOrDefaultAsync(u => u.Email == newUser.Email);
            if (existingUser != null)
                return BadRequest("Email is already takens.");

            // Hash the password before saving it to ensure it is stored securely in the database. 
            // This prevents storing plain-text passwords and adds an extra layer of security.
            newUser.PasswordHash = UserService.HashPassword(newUser.PasswordHash);

            dbUser.users.Add(newUser);
            await dbUser.SaveChangesAsync();

            return CreatedAtAction(nameof(FindUser), new { id = newUser.Id }, newUser);
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
    /// <returns>Returns the user as <see cref="User"/> Object if found, or a 404 error if not.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> FindUser([FromRoute] int id)
    {
        try
        {
            using var dbUser = new Database.NoorSphere();

            // Attempt to find the user by ID
            var user = await dbUser.users.FindAsync(id);

            if (user == null)
                return NotFound($"User with ID {id} not found.");

            return Ok(user);
        }
        catch (Exception er)
        {
            return StatusCode(500, $"Internal server error: {er.Message}");
        }
    }

    /// <summary>
    /// Retrieves all users from the database.
    /// </summary>
    /// <returns>Returns a list of all users as <see cref="User"/> Objects.</returns>
    [HttpGet("all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetUsers()
    {
        try
        {
            using var dbUser = new Database.NoorSphere();

            var usersList = await dbUser.users.ToListAsync();

            if (usersList == null)
                return NotFound($"Users are not found.");


            return Ok(usersList);
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
    /// <returns>Returns the user details if the credentials match as a <see cref="User"/> Object.</returns>
    [HttpGet("/login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetUser([FromQuery] string email, [FromQuery] string password)
    {
        try
        {
            using var dbUser = new Database.NoorSphere();

            // Attempt to find a user in the database with the provided email and password.
            // Since passwords are stored as hashed values in the database, 
            // we must hash the input password before comparing it with the stored hash to ensure a secure comparison.
            var user = await dbUser.users.Where
                (b => b.PasswordHash == UserService.HashPassword(password)
                && b.Email == email).ToListAsync();

            if (user.Count == 0)
                return NotFound($"User with the provided credentials was not found.");

            return Ok(user);
        }
        catch (Exception er)
        {
            return StatusCode(500, $"Internal server error: {er.Message}");
        }
    }

}



