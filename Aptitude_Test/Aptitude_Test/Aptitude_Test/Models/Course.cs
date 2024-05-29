using System;
using System.Collections.Generic;

namespace Aptitude_Test.Models;

public partial class Course
{
    public int CourseId { get; set; }

    public string CourseName { get; set; } = null!;

    public virtual ICollection<Test> Tests { get; set; } = new List<Test>();
}
