using AutoMapper;
using Database.Models.DTOs.ProfileAndRelatedEntities.Education;
using Database.Models.DTOs.ProfileAndRelatedEntities.Experience;
using Database.Models.DTOs.ProfileAndRelatedEntities.Profile;
using Database.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Server.Controllers
{
    [Route("api/profiles")]
    [ApiController]
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

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddProfile([FromBody] AddNewProfileDTO newProfileDTO)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    var userExists = await _userRepository.GetUser(newProfileDTO.UserId);
                    if (userExists == null)
                        return NotFound("User with the given UserId does not exist.");

                    var createdProfile = await _profileAndRelatedEntities.AddProfile
                        (_mapper.Map<Profile>(newProfileDTO));
                    return CreatedAtAction(nameof(GetProfile), new { id = createdProfile.Id }, createdProfile);
                }
                else
                    return BadRequest(ModelState);
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
        public async Task<IActionResult> AddExperience([FromBody] AddNewExperienceDTO newExperienceDTO)
        {
            try
            {  
                if(ModelState.IsValid)
                {
                    var profilerExists = await _profileAndRelatedEntities.GetProfile(newExperienceDTO.ProfileId);
                    if (profilerExists == null)
                        return NotFound("Profile with the given ProfileId does not exist.");

                    var createdExperience = await _profileAndRelatedEntities.AddExperience
                        (_mapper.Map<Experience>(newExperienceDTO));
                    return Ok(createdExperience);
                }
                else
                    return BadRequest(ModelState);
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
        public async Task<IActionResult> AddEducation([FromBody] AddNewEducationDTO newEducationDTO)
        {

            try
            {
               if( ModelState.IsValid)
                {
                    var profilerExists = await _profileAndRelatedEntities.GetProfile(newEducationDTO.ProfileId);
                    if (profilerExists == null)
                        return NotFound("Profile with the given ProfileId does not exist.");

                    var createdEducation = await _profileAndRelatedEntities.AddEducation
                        (_mapper.Map<Education>(newEducationDTO));
                    return Ok(createdEducation);
                }
                else
                    return BadRequest(ModelState);
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


    }
}
