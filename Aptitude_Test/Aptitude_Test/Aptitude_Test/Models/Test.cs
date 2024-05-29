using System;
using System.Collections.Generic;

namespace Aptitude_Test.Models;

public partial class Test
{
    public int TestId { get; set; }

    public string TestSubject { get; set; } = null!;

    public int TestTotalMarks { get; set; }

    public int? TestCourseId { get; set; }

    public virtual Course? TestCourse { get; set; }

    public virtual ICollection<TestQuestion> TestQuestions { get; set; } = new List<TestQuestion>();
}
