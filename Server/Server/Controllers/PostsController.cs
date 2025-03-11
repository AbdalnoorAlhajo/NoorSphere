using Database;
using Database.Models;
using Database.Repositories.Interfaces;
using Database.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Server.Controllers;


[ApiController]
[Route("api/posts")]
public class PostsController : ControllerBase
{
    private readonly IPostAndRelatedEntitiesRepository _postAndRelatedEntities;
    private readonly IUserRepository _userRepository;

    public PostsController(IUserRepository userRepository, IPostAndRelatedEntitiesRepository postAndRelatedEntities)
    {
        _postAndRelatedEntities = postAndRelatedEntities;
        _userRepository = userRepository;
    }


    /// <summary>
    /// Add a new post.
    /// </summary>
    /// <param name="newPost">The new post object to be added to the database.</param>
    /// <returns>Returns the post as <see cref="Post"/> Object with the assigned ID if operation go will.</returns>
    [HttpPost("Post")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddPost([FromBody] Post newPost)
    {

        try
        {
            if (newPost == null)
                return BadRequest("Post data is missing.");

            if (string.IsNullOrEmpty(newPost.Name) || string.IsNullOrEmpty(newPost.Text))
                return BadRequest("Text and Name are required filed.");

            var user = await _userRepository.GetUser(newPost.UserId);
            if (user == null)
                return NotFound($"User with ID {newPost.UserId} not found.");

            newPost.Name = user.Name;

            var createdPost = await _postAndRelatedEntities.AddPost(newPost);
            return Ok(createdPost);
        }

        catch (Exception er)
        {
            return StatusCode(500, $"Internal server error: {er.Message}");
        }

    }


    /// <summary>
    /// Retrieves all posts from the database.
    /// </summary>
    /// <returns>Returns a list of all posts as <see cref="Post"/> Objects.</returns>
    [HttpGet("all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetPosts()
    {
        try
        {
            var postsList = await _postAndRelatedEntities.GetAllPosts();

            if (postsList.Count < 1)
                return NotFound($"Posts are not found.");


            return Ok(postsList);
        }
        catch (Exception er)
        {
            return StatusCode(500, $"Internal server error: {er.Message}");
        }
    }

    /// <summary>
    /// Add a new Comment for a specifec post.
    /// </summary>
    /// <param name="newComment">The new Comment object.</param>
    /// <returns>Returns the Comment as <see cref="Comment"/> Object with the assigned ID if operation go will.</returns>
    [HttpPost("comment")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddComment([FromBody] Comment newComment)
    {

        try
        {
            if (newComment == null)
                return BadRequest("Comment data is missing.");

            if (string.IsNullOrEmpty(newComment.Name) || string.IsNullOrEmpty(newComment.Text))
                return BadRequest("Text and Name are required filed.");

            var user = await _userRepository.GetUser(newComment.UserId);
            if (user == null)
                return NotFound($"User with ID {newComment.UserId} not found.");

            var post = await _postAndRelatedEntities.GetPost(newComment.PostId);
            if (post == null)
                return NotFound($"Post with ID {newComment.PostId} not found.");

            newComment.Name = user.Name;

            var createdComment = await _postAndRelatedEntities.AddComment(newComment);

            return Ok(createdComment);
        }

        catch (Exception er)
        {
            return StatusCode(500, $"Internal server error: {er.Message}");
        }

    }


    /// <summary>
    /// Retrieves all comments for a specifec post.
    /// </summary>
    /// <returns>Returns a list of all comments as <see cref="Comment"/> Objects.</returns>
    [HttpGet("comments")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetComments(int PostId)
    {
        try
        {
            var commentsList = await _postAndRelatedEntities.GetAllComments(PostId);

            if (commentsList.Count < 1)
                return NotFound($"Comments are not found.");


            return Ok(commentsList);
        }
        catch (Exception er)
        {
            return StatusCode(500, $"Internal server error: {er.Message}");
        }
    }


}