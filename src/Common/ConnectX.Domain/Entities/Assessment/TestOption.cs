using ConnectX.Domain;

namespace ConnectX.Domain.Entities;

public class TestOption : Audit
{
    public Guid ExternalId { get; set; }
    public Guid TestQuestionId { get; set; }
    public TestQuestion? TestQuestion { get; set; }

    public string Text { get; set; } = string.Empty;
    public int SortOrder { get; set; }
    public bool IsCorrect { get; set; }
    public string? Feedback { get; set; }
}

