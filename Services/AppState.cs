namespace FocusFuel.Services;

public record StudySession(DateTime Start, int Minutes);
public enum TimerMode { Focus, ShortBreak, LongBreak }

public class UserAccount
{
    public string Name { get; set; } = "";
    public string Id { get; set; } = "";
    public string Password { get; set; } = "";
    public int DailyGoal { get; set; } = 120;
    public DateTime Joined { get; set; } = DateTime.Today;
    public int Interrupted { get; set; }
    public List<StudySession> Sessions { get; } = new();
}

/// In-memory app state (accounts, sessions, timer). Swap for a database later.
public class AppState : IDisposable
{
    static readonly List<UserAccount> Users = new();
    public UserAccount? User { get; private set; }
    public event Action? OnChange;
    void Notify() => OnChange?.Invoke();

    // ---------- auth ----------
    public string? Register(string name, string id, string pw, int goal)
    {
        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(id) || pw.Length < 6)
            return "Fill in every field. Password needs 6+ characters.";
        lock (Users)
        {
            if (Users.Any(u => u.Id.Equals(id, StringComparison.OrdinalIgnoreCase)))
                return "An account with that email or ID already exists.";
            User = new UserAccount { Name = name.Trim(), Id = id.Trim(), Password = pw, DailyGoal = Math.Clamp(goal, 10, 600) };
            Users.Add(User);
        }
        return null;
    }

    public string? Login(string id, string pw)
    {
        lock (Users)
            User = Users.FirstOrDefault(u => u.Id.Equals(id?.Trim(), StringComparison.OrdinalIgnoreCase) && u.Password == pw);
        return User == null ? "Wrong email/ID or password." : null;
    }

    public void Logout() { StopTimer(); User = null; }

    // ---------- stats ----------
    public int MinutesOn(DateTime d) => User?.Sessions.Where(s => s.Start.Date == d.Date).Sum(s => s.Minutes) ?? 0;
    public int TodayMinutes => MinutesOn(DateTime.Today);
    public int TotalMinutes => User?.Sessions.Sum(s => s.Minutes) ?? 0;
    public int SessionCount => User?.Sessions.Count ?? 0;
    public int SessionsToday => User?.Sessions.Count(s => s.Start.Date == DateTime.Today) ?? 0;
    public int AvgSession => SessionCount == 0 ? 0 : TotalMinutes / SessionCount;
    public int FocusRate { get { var t = SessionCount + (User?.Interrupted ?? 0); return t == 0 ? 100 : SessionCount * 100 / t; } }
    public int[] LastDays(int n) => Enumerable.Range(0, n).Select(i => MinutesOn(DateTime.Today.AddDays(i - n + 1))).ToArray();
    public DateTime WeekStart => DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
    public int WeekMinutes => Enumerable.Range(0, 7).Sum(i => MinutesOn(WeekStart.AddDays(i)));
    public int MonthMinutes => Enumerable.Range(0, DateTime.Today.Day).Sum(i => MinutesOn(DateTime.Today.AddDays(-i)));

    public int Streak
    {
        get
        {
            var d = DateTime.Today; if (MinutesOn(d) == 0) d = d.AddDays(-1);
            var n = 0; while (MinutesOn(d) > 0) { n++; d = d.AddDays(-1); }
            return n;
        }
    }

    public int SessionsBetween(int fromHour, int toHour) =>
        User?.Sessions.Count(s => s.Start.Hour >= fromHour && s.Start.Hour < toHour) ?? 0;

    public IEnumerable<(string Name, string Desc, bool Unlocked)> Achievements => new[]
    {
        ("First Steps", "Complete your first focus session", SessionCount >= 1),
        ("Hour Hero", "Study 60 minutes in total", TotalMinutes >= 60),
        ("On a Roll", "Reach a 3-day streak", Streak >= 3),
        ("Goal Getter", "Hit your daily goal", User != null && TodayMinutes >= User.DailyGoal),
    };

    // ---------- timer ----------
    public int[] Minutes { get; } = { 25, 5, 15 };
    public TimerMode Mode { get; private set; } = TimerMode.Focus;
    public int Remaining { get; private set; } = 25 * 60;
    public bool Running => _timer != null;
    public int Total => Minutes[(int)Mode] * 60;
    System.Threading.Timer? _timer;

    public void SetMode(TimerMode m) { StopTimer(); Mode = m; Remaining = Total; Notify(); }
    public void SetMinutes(int idx, int value) { Minutes[idx] = Math.Clamp(value, 1, 180); if ((int)Mode == idx && !Running) Remaining = Total; Notify(); }

    public void Toggle()
    {
        if (Running) StopTimer(); else _timer = new System.Threading.Timer(_ => Tick(), null, 1000, 1000);
        Notify();
    }

    public void Reset()
    {
        if (Mode == TimerMode.Focus && Running && User != null) User.Interrupted++;
        StopTimer(); Remaining = Total; Notify();
    }

    void Tick()
    {
        Remaining--;
        if (Remaining <= 0)
        {
            var mins = Minutes[(int)Mode];
            if (Mode == TimerMode.Focus) User?.Sessions.Add(new StudySession(DateTime.Now.AddMinutes(-mins), mins));
            StopTimer(); Remaining = Total;
        }
        Notify();
    }

    void StopTimer() { _timer?.Dispose(); _timer = null; }
    public void Dispose() => StopTimer();
}
