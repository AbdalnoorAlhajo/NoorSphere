using Database.Models.Domain;
using Database.Models.DTOs.Post;
using Database.Models.DTOs.PostAndRelatedEntities.Comment;
using Database.Models.DTOs.ProfileAndRelatedEntities.Education;
using Database.Models.DTOs.ProfileAndRelatedEntities.Experience;
using Database.Models.DTOs.ProfileAndRelatedEntities.Profile;
using Database.Models.DTOs.User;

namespace Database.Mappings
{
    public class AutoMapperProfiles : AutoMapper.Profile
    {
        public AutoMapperProfiles()
        {
            // User
            CreateMap<User, GetUserDTO>().ReverseMap();
            CreateMap<AddNewUserDTO, User>();

            // Post
            CreateMap<AddNewPostDTO, Post>();
            CreateMap<Post, GetPostDTO>().ReverseMap();

            // Comment
            CreateMap<AddNewCommentDTO, Comment>();
            CreateMap<Comment, GetCommentDTO>().ReverseMap();

            // Profile
            CreateMap<AddNewProfileDTO, Profile>();
            CreateMap<Profile, GetProfileDTO>().ReverseMap();

            // Education
            CreateMap<AddNewEducationDTO, Education>();
            CreateMap<Education, GetEducationDTO>().ReverseMap();

            // Experience
            CreateMap<AddNewExperienceDTO, Experience>();
            CreateMap<Experience, GetExperienceDTO>().ReverseMap();
        }
    }
}
