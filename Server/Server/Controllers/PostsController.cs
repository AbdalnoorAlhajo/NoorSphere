using AutoMapper;
using Database;
using Database.Models.Domain;
using Database.Models.DTOs.Post;
using Database.Models.DTOs.PostAndRelatedEntities.Comment;
using Database.Models.DTOs.PostAndRelatedEntities.Post;
using Database.Repositories.Interfaces;
using Database.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Server.Controllers;


[ApiController]
[Route("api/posts")]
[Authorize]
public class PostsController : ControllerBase
{
    private readonly IPostAndRelatedEntitiesRepository _postAndRelatedEntities;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public PostsController(IUserRepository userRepository, IMapper mapper
        , IPostAndRelatedEntitiesRepository postAndRelatedEntities)
    {
        _postAndRelatedEntities = postAndRelatedEntities;
        _userRepository = userRepository;
        _mapper = mapper;
    }


    /// <summary>
    /// Add a new post.
    /// </summary>
    /// <param name="newPostDTO">The new post object to be added to the database.</param>
    /// <returns>Returns the post as <see cref="Post"/> Object with the assigned ID if operation go will.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddPost([FromBody] AddNewPostDTO newPostDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var UserId = UserService.ExtractUserIDFromToken(Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last());

            if (string.IsNullOrEmpty(UserId))
                return Unauthorized("User ID is not found in the token.");

            var user = await _userRepository.GetUser(UserId);
            if (user == null)
                return NotFound($"User with ID {UserId} not found.");

            var newPost = _mapper.Map<Post>(newPostDTO);
            newPost.UserId = UserId;
            newPostDTO.Name = user.UserName;

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
    /// <returns>Returns a list of all posts as <see cref="GetPostDTO"/> Objects.</returns>
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

            return Ok(_mapper.Map<List<GetPostDTO>>(postsList));
        }
        catch (Exception er)
        {
            return StatusCode(500, $"Internal server error: {er.Message}");
        }
    }

    /// <summary>
    /// Add a new Comment for a specifec post.
    /// </summary>
    /// <param name="newCommentDTO">The new Comment object.</param>
    /// <returns>Returns the Comment as <see cref="Comment"/> Object with the assigned ID if operation go will.</returns>
    [HttpPost("comment")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddComment([FromBody] AddNewCommentDTO newCommentDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var UserId = UserService.ExtractUserIDFromToken(Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last());

            if (string.IsNullOrEmpty(UserId))
                return Unauthorized("User ID is not found in the token.");

            var user = await _userRepository.GetUser(UserId);
            if (user == null)
                return NotFound($"User with ID {UserId} not found.");

            var post = await _postAndRelatedEntities.GetPost(newCommentDTO.PostId);
            if (post == null)
                return NotFound($"Post with ID {newCommentDTO.PostId} not found.");

            var newComment = _mapper.Map<Comment>(newCommentDTO);
            newComment.Name = user.UserName;
            newComment.UserId = UserId;

            var createdComment = await _postAndRelatedEntities.AddComment(newComment);
            return Ok(createdComment);
        }

        catch (Exception er)
        {
            return StatusCode(500, $"Internal server error: {er.Message}");
        }

    }

    /// <summary>
    /// Like a specifec post.
    /// </summary>
    /// <param name="PostId">The post to be liked.</param>
    /// <returns>Returns <see cref="Like"/> Object with the assigned ID if operation go will.</returns>
    [HttpPost("like/{PostId}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Addlike([FromRoute] int PostId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var UserId = UserService.ExtractUserIDFromToken(Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last());

            if (string.IsNullOrEmpty(UserId))
                return Unauthorized("User ID is not found in the token.");

            var user = await _userRepository.GetUser(UserId);
            if (user == null)
                return NotFound($"User with ID {UserId} not found.");

            var post = await _postAndRelatedEntities.GetPost(PostId);
            if (post == null)
                return NotFound($"Post with ID {PostId} not found.");

            var createdLike = await _postAndRelatedEntities.AddLike(new Like { Id = 0, PostId = PostId, UserId = UserId});
            return Ok(createdLike);
        }

        catch (Exception er)
        {
            return StatusCode(500, $"Internal server error: {er.Message}");
        }

    }

    /// <summary>
    /// Retrieves all comments for a specifec post.
    /// </summary>
    /// <returns>Returns a list of all comments as <see cref="GetCommentDTO"/> Objects.</returns>
    [HttpGet("comments/{PostId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetComments([FromRoute ]int PostId)
    {
        try
        {
            var commentsList = await _postAndRelatedEntities.GetAllComments(PostId);

            if (commentsList.Count < 1)
                return NotFound($"Comments are not found.");

            return Ok(_mapper.Map<List<GetCommentDTO>>(commentsList));
        }
        catch (Exception er)
        {
            return StatusCode(500, $"Internal server error: {er.Message}");
        }
    }

}