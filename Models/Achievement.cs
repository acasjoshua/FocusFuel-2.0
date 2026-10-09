using System.ComponentModel.DataAnnotations;

namespace FocusFuel.Models;

/// A badge that can be unlocked (e.g. "First Steps", "Hour Hero").
public class Achievement
{
    public int Id { get; set; }

    [Required, MaxLength(40)] public string Code { get; set; } = "";
    [Required, MaxLength(60)] public string Name { get; set; } = "";
    [MaxLength(200)] public string Description { get; set; } = "";

    public List<UserAchievement> UserAchievements { get; set; } = new();
}
