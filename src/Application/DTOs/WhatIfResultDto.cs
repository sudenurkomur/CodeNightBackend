namespace CodeNight.Application.DTOs;

public class WhatIfResultDto
{
    public WhatIfScenario Baseline { get; set; } = new();
    public WhatIfScenario WhatIf { get; set; } = new();
    public string Explanation { get; set; } = string.Empty;
}

public class WhatIfScenario
{
    public List<Guid> TriggeredChallenges { get; set; } = new();
    public Guid? SelectedChallenge { get; set; }
    public List<Guid> SuppressedChallenges { get; set; } = new();
}
