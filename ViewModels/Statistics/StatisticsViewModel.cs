using Models.Users;

namespace ViewModels.Statistics;

public class StatisticsViewModel
{
    public DateTime? StartInderval { get; set; } = null;
    public DateTime? EndInterval { get; set; } = null;
    public string Filter { get; set; } = string.Empty;

    public Dictionary<ApplicationUser, (int SolvedTotal, int Assigned, int SolvedInInterval)>? Statistics { get; set; }
}
