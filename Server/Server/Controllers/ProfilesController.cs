using Database.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Server.Controllers
{
    [Route("api/profiles")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IProfileAndRelatedEntities _profileAndRelatedEntities;

        public ProfileController(IUserRepository userRepository
            , IProfileAndRelatedEntities profileAndRelatedEntities)
        {
            _profileAndRelatedEntities = profileAndRelatedEntities;
            _userRepository = userRepository;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddProfile([FromBody] Profile newProfile)
        {

            try
            {
                 if (newProfile == null)
                    return BadRequest("Profile data is required.");
             
                 if (string.IsNullOrEmpty(newProfile.Status))
                    return BadRequest("Status and Skills are required.");

                var userExists = await _userRepository.GetUser(newProfile.UserId);
                if (userExists == null)
                    return NotFound("User with the given UserId does not exist.");

                var createdProfile = await _profileAndRelatedEntities.AddProfile(newProfile);
                return CreatedAtAction(nameof(GetProfile), new { id = createdProfile.Id }, createdProfile);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error " + ex.Message);
            }
        }

        [HttpPost("experience")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddExperience([FromBody] Experience newExperience)
        {

            try
            {
                if (newExperience == null)
                    return BadRequest("Experience data is required.");

                if (string.IsNullOrEmpty(newExperience.Title))
                    return BadRequest("Title is required.");

                var profilerExists = await _profileAndRelatedEntities.GetProfile(newExperience.ProfileId);
                if (profilerExists == null)
                    return NotFound("Profile with the given ProfileId does not exist.");

                var createdExperience = await _profileAndRelatedEntities.AddExperience(newExperience);
                return Ok(createdExperience);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error " + ex.Message);
            }
        }


        [HttpPost("Education")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddEducation([FromBody] Education newEducation)
        {

            try
            {
                if (newEducation == null)
                    return BadRequest("Education data is required.");

                if (string.IsNullOrEmpty(newEducation.Degree))
                    return BadRequest("Degree is required.");

                var profilerExists = await _profileAndRelatedEntities.GetProfile(newEducation.ProfileId);
                if (profilerExists == null)
                    return NotFound("Profile with the given ProfileId does not exist.");

                var createdEducation = await _profileAndRelatedEntities.AddEducation(newEducation);
                return Ok(createdEducation);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error " + ex.Message);
            }
        }


        /// <summary>
        /// Retrieves all profiles from the database.
        /// </summary>
        /// <returns>Returns a list of all profiles as <see cref="Profile"/> Objects.</returns>
        [HttpGet("Profiles")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetProfiles()
        {
            try
            {
                var profilesList = await _profileAndRelatedEntities.GetAllProfiles();

                if (profilesList.Count < 1)
                    return NotFound($"Profiles are not found.");


                return Ok(profilesList);
            }
            catch (Exception er)
            {
                return StatusCode(500, $"Internal server error: {er.Message}");
            }
        }

        /// <summary>
        /// Retrieves all Experiences for a specific Profile from the database.
        /// </summary>
        /// <returns>Returns a list of all experiences as <see cref="Experience"/> Objects.</returns>
        [HttpGet("Experiences")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetExperiences(int profileID)
        {
            try
            {
                var experiencesList = await _profileAndRelatedEntities.GetAllExperiences(profileID);

                if (experiencesList.Count < 1)
                    return NotFound($"profile with ID({profileID}) has no Experience.");


                return Ok(experiencesList);
            }
            catch (Exception er)
            {
                return StatusCode(500, $"Internal server error: {er.Message}");
            }
        }


        /// <summary>
        /// Retrieves all Education for a specific Profile from the database.
        /// </summary>
        /// <returns>Returns a list of all Education as <see cref="Education"/> Objects.</returns>
        [HttpGet("Education")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetEducation(int profileID)
        {
            try
            {
                var educationList = await _profileAndRelatedEntities.GetAllEducation(profileID);

                if (educationList.Count < 1)
                    return NotFound($"profile with ID({profileID}) has no Experience.");


                return Ok(educationList);
            }
            catch (Exception er)
            {
                return StatusCode(500, $"Internal server error: {er.Message}");
            }
        }


        /// <summary>
        /// Retrieves a profile by their ID.
        /// </summary>
        /// <param name="id">The ID of the profile to retrieve.</param>
        /// <returns>Returns the profile as <see cref="Profile"/> Object if found, or a 404 error if not.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetProfile([FromRoute] int id)
        {
            try
            {
                // Attempt to find the profile by ID
                var profile = await _profileAndRelatedEntities.GetProfile(id);

                if (profile == null)
                    return NotFound($"Profile with ID {id} not found.");

                return Ok(profile);
            }
            catch (Exception er)
            {
                return StatusCode(500, $"Internal server error: {er.Message}");
            }
        }


    }
}
