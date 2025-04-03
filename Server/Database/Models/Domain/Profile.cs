using Database.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models.Domain;
public class Profile
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("User")]
    public string UserId { get; set; }

    public string? Company { get; set; }
    public string? Website { get; set; }
    public string? Country { get; set; }
    public string? Location { get; set; }

    [Required]
    public string Status { get; set; }

    [Required]
    public string Skills { get; set; }

    public string? Bio { get; set; }
    public List<Experience> Experiences { get; set; }
    public List<Education> Educations { get; set; }
    public SocialLinks SocialLinks { get; set; }
    public DateTime Date { get; set; } = DateTime.UtcNow;
}

public class Experience
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Title { get; set; }

    [Required]
    public string Company { get; set; }

    public string? Location { get; set; }

    [Required]
    public DateTime From { get; set; }

    public DateTime? To { get; set; }

    public bool Current { get; set; } = false;

    public string? Description { get; set; }

    [ForeignKey("Profile")]
    public int ProfileId { get; set; }
}

public class Education
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string School { get; set; }

    [Required]
    public string Degree { get; set; }

    [Required]
    public string FieldOfStudy { get; set; }

    [Required]
    public DateTime From { get; set; }

    public DateTime? To { get; set; }

    public bool Current { get; set; } = false;

    public string? Description { get; set; }

    [ForeignKey("Profile")]
    public int ProfileId { get; set; }
}

public class SocialLinks
{
    [Key]
    public int Id { get; set; }
    public string? Youtube { get; set; }
    public string? Twitter { get; set; }
    public string? Facebook { get; set; }
    public string? LinkedIn { get; set; }
    public string? Instagram { get; set; }
    public string? GitHub { get; set; }

    [ForeignKey("Profile")]
    public int ProfileId { get; set; }
}

