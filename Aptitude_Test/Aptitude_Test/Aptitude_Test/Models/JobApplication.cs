using System;
using System.Collections.Generic;

namespace Aptitude_Test.Models;

public partial class JobApplication
{
    public int JId { get; set; }

    public string JFullname { get; set; } = null!;

    public string JEmail { get; set; } = null!;

    public string JPhone { get; set; } = null!;

    public string UserAddress { get; set; } = null!;

    public string JResume { get; set; } = null!;

    public string JMessage { get; set; } = null!;

    public DateTime? JSubmission { get; set; } = DateTime.Now;

    public int? JUserId { get; set; }

    public virtual ICollection<Finalresult> Finalresults { get; set; } = new List<Finalresult>();

    public virtual User? JUser { get; set; }
}
