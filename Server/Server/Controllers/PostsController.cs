using AutoMapper;
using Database;
using Database.Models.Domain;
using Database.Models.DTOs.Post;
using Database.Models.DTOs.PostAndRelatedEntities.Comment;
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
    /// <param name="newPost">The new post object to be added to the database.</param>
    /// <returns>Returns the post as <see cref="Post"/> Object with the assigned ID if operation go will.</returns>
    [HttpPost("Post")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddPost([FromBody] AddNewPostDTO newPostDTO)
    {
        try
        {
            if(ModelState.IsValid)
            {

                var user = await _userRepository.GetUser(newPostDTO.UserId);
                if (user == null)
                    return NotFound($"User with ID {newPostDTO.UserId} not found.");

                newPostDTO.Name = user.Name;

                var newPost = _mapper.Map<Post>(newPostDTO);

                var createdPost = await _postAndRelatedEntities.AddPost(newPost);
                return Ok(createdPost);
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
        try
        {
            if(ModelState.IsValid)
            {
                var user = await _userRepository.GetUser(newCommentDTO.UserId);
                if (user == null)
                    return NotFound($"User with ID {newCommentDTO.UserId} not found.");

                var post = await _postAndRelatedEntities.GetPost(newCommentDTO.PostId);
                if (post == null)
                    return NotFound($"Post with ID {newCommentDTO.PostId} not found.");

                newCommentDTO.Name = user.Name;

                var createdComment = await _postAndRelatedEntities.AddComment
                    (_mapper.Map<Comment>(newCommentDTO));

                return Ok(createdComment);
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
    /// Retrieves all comments for a specifec post.
    /// </summary>
    /// <returns>Returns a list of all comments as <see cref="GetCommentDTO"/> Objects.</returns>
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

            return Ok(_mapper.Map<List<GetCommentDTO>>(commentsList));
        }
        catch (Exception er)
        {
            return StatusCode(500, $"Internal server error: {er.Message}");
        }
    }

}