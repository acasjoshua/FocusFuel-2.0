using System.ComponentModel.DataAnnotations;

namespace FocusFuel.Models;

/// A to-do item on the Tasks page.
public class StudyTask
{
    public int Id { get; set; }
    public int UserId { get; set; }

    [Required, MaxLength(200)] public string Title { get; set; } = "";
    public bool Done { get; set; }
    public DateTime Created { get; set; } = DateTime.Now;
    public DateTime? CompletedAt { get; set; }

    public User? User { get; set; }
}
