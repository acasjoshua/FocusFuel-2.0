namespace FocusFuel.Models;

/// Join entity: which user unlocked which achievement, and when.
public class UserAchievement
{
    public int UserId { get; set; }
    public int AchievementId { get; set; }
    public DateTime UnlockedAt { get; set; } = DateTime.Now;

    public User? User { get; set; }
    public Achievement? Achievement { get; set; }
}
