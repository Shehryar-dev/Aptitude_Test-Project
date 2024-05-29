using System;
using System.Collections.Generic;

namespace Aptitude_Test.Models;

public partial class TestQuestion
{
    public int QuesId { get; set; }

    public string Questions { get; set; } = null!;

    public string? QuesOpt1 { get; set; }

    public string? QuesOpt2 { get; set; }

    public string? QuesOpt3 { get; set; }

    public string? QuesOpt4 { get; set; }

    public string QuesAns { get; set; } = null!;

    public int QuesMarks { get; set; }

    public int? QuesTestId { get; set; }

    public virtual Test? QuesTest { get; set; }
}
