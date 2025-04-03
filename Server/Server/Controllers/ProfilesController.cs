using AutoMapper;
using Database.Models.DTOs.ProfileAndRelatedEntities.Education;
using Database.Models.DTOs.ProfileAndRelatedEntities.Experience;
using Database.Models.DTOs.ProfileAndRelatedEntities.Profile;
using Database.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Database.Utils;
using Microsoft.AspNetCore.Authorization;
using Database.Models.Domain;
using Profile = Database.Models.Domain.Profile;

namespace Server.Controllers
{
    [Route("api/profiles")]
    [ApiController]
    [Authorize]
    public class ProfileController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IProfileAndRelatedEntities _profileAndRelatedEntities;
        private readonly IMapper _mapper;

        public ProfileController(IUserRepository userRepository, IMapper mapper
            , IProfileAndRelatedEntities profileAndRelatedEntities)
        {
            _profileAndRelatedEntities = profileAndRelatedEntities;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Add a new Profile.
        /// </summary>
        /// <param name="newProfileDTO">The new Profile object to be added to the database.</param>
        /// <returns>Returns the Profile as <see cref="Profile"/> Object with the assigned ID if operation go will.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddProfile([FromBody] AddNewProfileDTO newProfileDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var UserId = UserService.ExtractUserIDFromToken(Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last());

                if (string.IsNullOrEmpty(UserId))
                    return Unauthorized("User ID is not found in the token.");

                var userExists = await _userRepository.GetUser(UserId);
                if (userExists == null)
                    return NotFound("User with the given UserId does not exist.");

                var newProfile = _mapper.Map<Profile>(newProfileDTO);
                newProfile.UserId = UserId;

                var createdProfile = await _profileAndRelatedEntities.AddProfile(newProfile);
                return CreatedAtAction(nameof(GetProfile), new { id = createdProfile.Id }, createdProfile);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error " + ex.Message);
            }
        }

        /// <summary>
        /// Update a profile.
        /// </summary>
        /// <param name="newProfileDTO">The Updated Profile object to be updated to the database.</param>
        /// <returns>Returns the updated Profile as <see cref="Profile"/> Object operation go will.</returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateProfile([FromBody] AddNewProfileDTO newProfileDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var UserId = UserService.ExtractUserIDFromToken(Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last());

                if (string.IsNullOrEmpty(UserId))
                    return Unauthorized("User ID is not found in the token.");

                var userExists = await _userRepository.GetUser(UserId);
                if (userExists == null)
                    return NotFound("User with the given UserId does not exist.");

                var UpdatedProfile = _mapper.Map<Profile>(newProfileDTO);
                UpdatedProfile.UserId = UserId;

                var createdProfile = await _profileAndRelatedEntities.UpdateProfile(UpdatedProfile);
                return CreatedAtAction(nameof(GetProfile), new { id = createdProfile.Id }, createdProfile);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error " + ex.Message);
            }
        }

        /// <summary>
        /// Add a new Experience for a specific profile.
        /// </summary>
        /// <param name="newExperienceDTO">The new Experience object to be added to the database.</param>
        /// <returns>Returns the Experience as <see cref="Experience"/> Object with the assigned ID if operation go will.</returns>
        [HttpPost("experience")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddExperience([FromBody] AddNewExperienceDTO newExperienceDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var UserId = UserService.ExtractUserIDFromToken(Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last());

                if (string.IsNullOrEmpty(UserId))
                    return Unauthorized("User ID is not found in the token.");

                var UserProfile = await _profileAndRelatedEntities.GetProfileByUserId(UserId);
                if (UserProfile == null)
                    return NotFound("User does not have profile.");

                newExperienceDTO.ProfileId = UserProfile.Id;

                var createdExperience = await _profileAndRelatedEntities.AddExperience
                    (_mapper.Map<Experience>(newExperienceDTO));
                return CreatedAtAction(nameof(GetExperiences), new { profileID = createdExperience.ProfileId}, createdExperience);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error " + ex.Message);
            }
        }

        /// <summary>
        /// Add a new Education for a specific profile.
        /// </summary>
        /// <param name="newEducationDTO">The new Education object to be added to the database.</param>
        /// <returns>Returns the Education as <see cref="Education"/> Object with the assigned ID if operation go will.</returns>
        [HttpPost("Education")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddEducation([FromBody] AddNewEducationDTO newEducationDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var UserId = UserService.ExtractUserIDFromToken(Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last());

                if (string.IsNullOrEmpty(UserId))
                    return Unauthorized("User ID is not found in the token.");

                var UserProfile = await _profileAndRelatedEntities.GetProfileByUserId(UserId);
                if (UserProfile == null)
                    return NotFound("User does not have profile.");

                newEducationDTO.ProfileId = UserProfile.Id;

                var createdEducation = await _profileAndRelatedEntities.AddEducation
                    (_mapper.Map<Education>(newEducationDTO));
                return CreatedAtAction(nameof(GetEducation), new { profileID = createdEducation.ProfileId}, createdEducation);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error " + ex.Message);
            }
        }


        /// <summary>
        /// Retrieves all profiles from the database.
        /// </summary>
        /// <returns>Returns a list of all profiles as <see cref="GetProfileDTO"/> Objects.</returns>
        [HttpGet]
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


                return Ok(_mapper.Map<List<GetProfileDTO>>(profilesList));
            }
            catch (Exception er)
            {
                return StatusCode(500, $"Internal server error: {er.Message}");
            }
        }

        /// <summary>
        /// Retrieves all Experiences for a specific Profile from the database.
        /// </summary>
        /// <returns>Returns a list of all experiences as <see cref="GetExperienceDTO"/> Objects.</returns>
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


                return Ok(_mapper.Map<List<GetExperienceDTO>>(experiencesList));
            }
            catch (Exception er)
            {
                return StatusCode(500, $"Internal server error: {er.Message}");
            }
        }


        /// <summary>
        /// Retrieves all Education for a specific Profile from the database.
        /// </summary>
        /// <returns>Returns a list of all Education as <see cref="GetEducationDTO"/> Objects.</returns>
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


                return Ok(_mapper.Map<List<GetEducationDTO>>(educationList));
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
        /// <returns>Returns the profile as <see cref="GetProfileDTO"/> Object if found, or a 404 error if not.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetProfile([FromRoute] int id)
        {
            try
            {
                var profile = await _profileAndRelatedEntities.GetProfile(id);

                if (profile == null)
                    return NotFound($"Profile with ID {id} not found.");

                return Ok(_mapper.Map<GetProfileDTO>(profile));
            }
            catch (Exception er)
            {
                return StatusCode(500, $"Internal server error: {er.Message}");
            }
        }

        /// <summary>
        /// Retrieves a profile by their UserID.
        /// </summary>
        /// <returns>Returns the profile as <see cref="GetProfileDTO"/> Object if found, or a 404 error if not.</returns>
        [HttpGet("me")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetMyProfile()
        {
            try
            {
                var UserId = UserService.ExtractUserIDFromToken(Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last());

                if (string.IsNullOrEmpty(UserId))
                    return Unauthorized("User ID is not found in the token.");

                var profile = await _profileAndRelatedEntities.GetProfileByUserId(UserId);

                if (profile == null)
                    return NotFound($"User Do not have a profile.");

                return Ok(_mapper.Map<GetProfileDTO>(profile));
            }
            catch (Exception er)
            {
                return StatusCode(500, $"Internal server error: {er.Message}");
            }
        }
    }
}
