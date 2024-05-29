using System;
using System.Collections.Generic;

namespace Aptitude_Test.Models;

public partial class Gktest
{
    public int Id { get; set; }
    public string? Question { get; set; }
    public string? OptionA { get; set; }
    public string? OptionB { get; set; }
    public string? OptionC { get; set; }
    public string? OptionD { get; set; }
    public string? CorrectAnswer { get; set; }

    public virtual ICollection<Finalresult> Finalresults { get; set; } = new List<Finalresult>();
}

