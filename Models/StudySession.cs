namespace FocusFuel.Models;

/// One completed focus session.
public class StudySession
{
    public int Id { get; set; }
    public int UserId { get; set; }

    public DateTime Start { get; set; }
    public int Minutes { get; set; }

    public User? User { get; set; }
}
