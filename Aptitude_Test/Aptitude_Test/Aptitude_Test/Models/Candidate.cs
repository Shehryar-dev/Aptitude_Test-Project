using System;
using System.Collections.Generic;

namespace Aptitude_Test.Models;

public partial class Candidate
{
    public int CandidateId { get; set; }

    public string FullName { get; set; } = null!;

    public string? ImageUrl { get; set; }

    public string? CantDescription { get; set; }

    public string? Location { get; set; }

    public DateTime? CreatedDate { get; set; }
}
