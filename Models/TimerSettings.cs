using System.ComponentModel.DataAnnotations;

namespace FocusFuel.Models;

/// Per-user timer lengths in minutes (one-to-one with User).
public class TimerSettings
{
    public int Id { get; set; }
    public int UserId { get; set; }

    [Range(1, 180)] public int FocusMinutes { get; set; } = 25;
    [Range(1, 180)] public int ShortBreakMinutes { get; set; } = 5;
    [Range(1, 180)] public int LongBreakMinutes { get; set; } = 15;

    public User? User { get; set; }

    /// Access by TimerMode index: 0 = focus, 1 = short break, 2 = long break.
    public int this[int mode]
    {
        get => mode switch { 0 => FocusMinutes, 1 => ShortBreakMinutes, _ => LongBreakMinutes };
        set
        {
            switch (mode)
            {
                case 0: FocusMinutes = value; break;
                case 1: ShortBreakMinutes = value; break;
                default: LongBreakMinutes = value; break;
            }
        }
    }
}
