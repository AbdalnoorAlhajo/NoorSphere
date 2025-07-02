using AutoMapper;
using Database;
using Database.Models.Domain;
using Database.Models.DTOs.Post;
using Database.Models.DTOs.PostAndRelatedEntities.Post;
using Database.Repositories.Interfaces;
using Database.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Database.Repositories.Implementaions.SQLPostAndRelatedEntitiesRepository;

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
            var UserId = UserService.ExtractUserIDFromToken(Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last());

            var postsList = await _postAndRelatedEntities.GetAllPosts(UserId);

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
    /// Retrieves a specific post by its ID.
    /// </summary>
    /// <param name="postId">The ID of the post to retrieve.</param>
    /// <returns>Returns a <see cref="GetPostDTO"/> object if found.</returns>
    [HttpGet("{postId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetPostById(int postId)
    {
        try
        {
            var userId = UserService.ExtractUserIDFromToken(Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last());

            var post = await _postAndRelatedEntities.GetPost(postId, userId);

            if (post == null)
                return NotFound($"Post with ID {postId} was not found.");

            return Ok(post);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }


    /// <summary>
    /// Retrieves posts made by users that the current user follows.
    /// </summary>
    /// <returns>Returns a list of posts by followed users as <see cref="GetPostDTO"/> objects.</returns>
    [HttpGet("following")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetPostsByFollowedUsers()
    {
        try
        {
            var userId = UserService.ExtractUserIDFromToken(Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last());

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User ID is not found in the token.");

            var posts = await _postAndRelatedEntities.GetPostsByFollowedUsers(userId);

            if (posts == null || posts.Count == 0)
                return NotFound("No posts found from followed users.");

            return Ok(posts);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
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

            var post = await _postAndRelatedEntities.GetPost(PostId, UserId);
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

    //This is about to be deleted
    /// <summary>
    /// Retrieves all comments for a specifec post.
    /// </summary>
    /// <returns>Returns a list of all comments as <see cref = "GetPostDTO" /> Objects.</ returns >
    [HttpGet("comments/{PostId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetComments([FromRoute] int PostId)
    {
        try
        {
            var userId = UserService.ExtractUserIDFromToken(Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last());


            var commentsList = await _postAndRelatedEntities.GetAllComments(PostId, userId);

            if (commentsList.Count < 1)
                return NotFound($"Comments are not found.");

            return Ok(commentsList);
        }
        catch (Exception er)
        {
            return StatusCode(500, $"Internal server error: {er.Message}");
        }
    }

    /// <summary>
    /// Retrieves trending Topics from posts (NoorSphere, Social Media, Software Engineer).
    /// </summary>
    /// <returns>Returns a sorted list of trending topics as <see cref="TrendingTopicDTO"/> objects.</returns>
    [HttpGet("trending")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetTrendingTopics()
    {
        try
        {
            var trendingTopics = await _postAndRelatedEntities.GetTrendingTopics();

            return Ok(trendingTopics);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }


    /// <summary>
    /// Retrieves all posts that contain the specified search text.
    /// </summary>
    /// <param name="text">The text to search for in the post content.</param>
    /// <returns>Returns a list of matched posts as <see cref="GetPostDTO"/> objects.</returns>
    [HttpGet("search")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetPostsByText([FromQuery] string text)
    {
        try
        {
            var userId = UserService.ExtractUserIDFromToken(Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last());


            if (string.IsNullOrWhiteSpace(text))
                return BadRequest("Search text must not be empty.");

            var matchedPosts = await _postAndRelatedEntities.GetPostsByText(text, userId);

            if (matchedPosts.Count < 1)
                return NotFound($"No posts found containing the text: '{text}'");

            return Ok(matchedPosts);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }


    /// <summary>
    /// Retrieves all posts created by the specified user.
    /// </summary>
    /// <param name="userId">The ID of the user whose posts should be retrieved.</param>
    /// <returns>Returns a list of posts by the user as <see cref="GetPostDTO"/> objects.</returns>
    [HttpGet("user")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetPostsByUserId([FromQuery] string userId)
    {
        try
        {
            var currentUserId = UserService.ExtractUserIDFromToken(Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last());

            if (string.IsNullOrEmpty(userId))
                return BadRequest("Invalid user ID.");

            var userPosts = await _postAndRelatedEntities.GetPostsForSpecificUser(userId, currentUserId);

            if (userPosts == null || userPosts.Count < 1)
                return NotFound($"No posts found for user with ID: {userId}");

            return Ok(userPosts);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    /// <summary>
    /// Retrieves posts liked by the specified user.
    /// </summary>
    /// <param name="userId">The ID of the user whose liked posts to retrieve.</param>
    /// <returns>Returns a list of liked posts as <see cref="GetPostDTO"/> objects.</returns>
    [HttpGet("liked")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetPostsLikedByUser([FromQuery] string userId)
    {
        try
        {
            var currentUserId = UserService.ExtractUserIDFromToken(Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last());


            if (string.IsNullOrWhiteSpace(userId))
                return BadRequest("User ID must be provided.");

            var likedPosts = await _postAndRelatedEntities.GetPostsLikedByUser(userId, currentUserId);

            if (likedPosts == null || likedPosts.Count == 0)
                return NotFound($"No liked posts found for user ID: {userId}");

            return Ok(likedPosts);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }


}