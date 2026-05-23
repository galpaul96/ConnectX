using ConnectX.Domain;

namespace ConnectX.Domain.Entities;

public class TestQuestion : Audit
{
    public Guid ExternalId { get; set; }
    public Guid OnlineTestId { get; set; }
    public OnlineTest? OnlineTest { get; set; }

    public TestQuestionType QuestionType { get; set; }
    public string Prompt { get; set; } = string.Empty;
    public string? Explanation { get; set; }
    public int SortOrder { get; set; }
    public decimal Points { get; set; }

    public List<TestOption> Options { get; set; } = [];
}

