using System;
using System.Collections.Generic;

namespace Aptitude_Test.Models;

public partial class User
{
    public int UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string UserEmail { get; set; } = null!;

    public string UserPassword { get; set; } = null!;

    public string? UserImage { get; set; }

    public int? UserRoleId { get; set; }

    public virtual ICollection<Finalresult> Finalresults { get; set; } = new List<Finalresult>();

    public virtual ICollection<JobApplication> JobApplications { get; set; } = new List<JobApplication>();

    public virtual Role? UserRole { get; set; }
}
