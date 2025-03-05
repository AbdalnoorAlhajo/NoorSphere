using Database;
using Database.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Server.Controllers
{
    [Route("api/profiles")]
    [ApiController]
    public class ProfileController : ControllerBase
    {

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

                using var dbProfile = new NoorSphere();

                // Check if the UserId exists in the Users table
                var userExists = dbProfile.users.FirstOrDefault(u => u.Id == newProfile.UserId);
                if (userExists == null)
                    return NotFound("User with the given UserId does not exist.");

                dbProfile.profiles.Add(newProfile);
                await dbProfile.SaveChangesAsync();

                return CreatedAtAction(nameof(FindProfile), new { id = newProfile.Id }, newProfile);
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

                using var dbProfile = new NoorSphere();

                var profilerExists = dbProfile.profiles.FirstOrDefault(u => u.Id == newExperience.ProfileId);
                if (profilerExists == null)
                    return NotFound("Profile with the given ProfileId does not exist.");

                dbProfile.Experiences.Add(newExperience);
                await dbProfile.SaveChangesAsync();

                return Ok(newExperience);
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

                using var dbProfile = new NoorSphere();

                var userExists = dbProfile.profiles.FirstOrDefault(u => u.Id == newEducation.ProfileId);
                if (userExists == null)
                    return NotFound("Profile with the given ProfileId does not exist.");

                dbProfile.education.Add(newEducation);
                await dbProfile.SaveChangesAsync();

                return Ok(newEducation);
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
                using var dbProfile = new NoorSphere();

                var profilesList = await dbProfile.profiles.ToListAsync();

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
                using var dbProfile = new NoorSphere();

                var experiencesList = await dbProfile.Experiences.Where(b => b.ProfileId == profileID).ToListAsync();

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
                using var dbProfile = new NoorSphere();

                var educationList = await dbProfile.education.Where(b => b.ProfileId == profileID).ToListAsync();

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
        public async Task<IActionResult> FindProfile([FromRoute] int id)
        {
            try
            {
                using var dbProfile = new NoorSphere();

                // Attempt to find the profile by ID
                var profile = await dbProfile.profiles.FindAsync(id);

                if (profile == null)
                    return NotFound($"Profile with ID {id} not found.");

                return Ok(profile);
            }
            catch (Exception er)
            {
                return StatusCode(500, $"Internal server error: {er.Message}");
            }
        }


        //[HttpGet("me")]
        //[Authorize]
        //public async Task<IActionResult> GetProfile()
        //{
        //    try
        //    {
        //        var profile = await _profileService.GetProfileByUserId(User.Identity.Name);
        //        if (profile == null)
        //            return BadRequest("There is no profile for this user");

        //        return Ok(profile);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error fetching user profile.");
        //        return StatusCode(500, "Internal server error");
        //    }
        //}

        //[HttpGet]
        //[Authorize]
        //public async Task<IActionResult> GetAllProfiles()
        //{
        //    try
        //    {
        //        var profiles = await _profileService.GetAllProfiles();
        //        return Ok(profiles);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error fetching profiles.");
        //        return StatusCode(500, "Internal server error");
        //    }
        //}

        //[HttpGet("user/{userId}")]
        //[Authorize]
        //public async Task<IActionResult> GetProfileByUserId(string userId)
        //{
        //    try
        //    {
        //        var profile = await _profileService.GetProfileByUserId(userId);
        //        if (profile == null)
        //            return BadRequest("There is no profile for the given user");

        //        return Ok(profile);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error fetching user profile.");
        //        return StatusCode(500, "Internal server error");
        //    }
        //}

        //[HttpDelete]
        //[Authorize]
        //public async Task<IActionResult> DeleteProfile()
        //{
        //    try
        //    {
        //        await _profileService.DeleteProfile(User.Identity.Name);
        //        await _postService.DeletePostsByUser(User.Identity.Name);
        //        await _userService.DeleteUser(User.Identity.Name);

        //        return Ok(new { message = "User information is deleted successfully" });
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error deleting profile.");
        //        return StatusCode(500, "Internal server error");
        //    }
        //}

        //[HttpPost("upload")]
        //[Authorize]
        //public async Task<IActionResult> UploadProfileImage(IFormFile file)
        //{
        //    if (file == null)
        //        return BadRequest("No file uploaded.");

        //    try
        //    {
        //        await _profileService.UploadProfileImage(file, User.Identity.Name);
        //        return Ok(new { message = "File uploaded successfully" });
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error uploading profile image.");
        //        return StatusCode(500, "Internal server error");
        //    }
        //}

        //[HttpPut("experience")]
        //[Authorize]
        //public async Task<IActionResult> AddExperience([FromBody] ExperienceDto experienceDto)
        //{
        //    if (experienceDto == null || string.IsNullOrEmpty(experienceDto.Title) || string.IsNullOrEmpty(experienceDto.Company))
        //        return BadRequest("Experience Title and Company are required.");

        //    try
        //    {
        //        var profile = await _profileService.AddExperience(experienceDto, User.Identity.Name);
        //        return Ok(profile);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error adding experience.");
        //        return StatusCode(500, "Internal server error");
        //    }
        //}

        //[HttpDelete("experience/{experienceId}")]
        //[Authorize]
        //public async Task<IActionResult> DeleteExperience(string experienceId)
        //{
        //    try
        //    {
        //        var profile = await _profileService.DeleteExperience(experienceId, User.Identity.Name);
        //        return Ok(profile);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error deleting experience.");
        //        return StatusCode(500, "Internal server error");
        //    }
        //}

        //[HttpPut("education")]
        //[Authorize]
        //public async Task<IActionResult> AddEducation([FromBody] EducationDto educationDto)
        //{
        //    if (educationDto == null || string.IsNullOrEmpty(educationDto.School) || string.IsNullOrEmpty(educationDto.Degree))
        //        return BadRequest("Education School and Degree are required.");

        //    try
        //    {
        //        var profile = await _profileService.AddEducation(educationDto, User.Identity.Name);
        //        return Ok(profile);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error adding education.");
        //        return StatusCode(500, "Internal server error");
        //    }
        //}

        //[HttpDelete("education/{educationId}")]
        //[Authorize]
        //public async Task<IActionResult> DeleteEducation(string educationId)
        //{
        //    try
        //    {
        //        var profile = await _profileService.DeleteEducation(educationId, User.Identity.Name);
        //        return Ok(profile);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error deleting education.");
        //        return StatusCode(500, "Internal server error");
        //    }
        //}
    }
}
