using System;
using System.Collections.Generic;

namespace Aptitude_Test.Models;

public partial class Job
{
    public int JobId { get; set; }

    public string? JobTitle { get; set; }

    public string? CompanyName { get; set; }

    public string? JobLocation { get; set; }

    public string? EmploymentType { get; set; }
}
