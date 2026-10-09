using System.ComponentModel.DataAnnotations;

namespace FocusFuel.Models;

/// A registered student account.
public class User
{
    public int Id { get; set; }

    [Required, MaxLength(100)] public string Name { get; set; } = "";
    /// The "Email or Student ID" the user logs in with.
    [Required, MaxLength(150)] public string LoginId { get; set; } = "";
    [Required] public string Password { get; set; } = "";   // TODO: store a hash once the database is added

    [Range(10, 600)] public int DailyGoal { get; set; } = 120;   // minutes per day
    public DateTime Joined { get; set; } = DateTime.Today;
    /// Focus sessions the user abandoned (used for Focus Rate).
    public int Interrupted { get; set; }

    public TimerSettings Timer { get; set; } = new();
    public List<StudySession> Sessions { get; set; } = new();
    public List<StudyTask> Tasks { get; set; } = new();
    public List<UserAchievement> Achievements { get; set; } = new();
}
