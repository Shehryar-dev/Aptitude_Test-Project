using System;
using System.Collections.Generic;

namespace Aptitude_Test.Models;

public partial class Finalresult
{
    public int FId { get; set; }

    public int FTotalscoreGk { get; set; } = 0;

    public int FTotalscoreMaths { get; set; } = 0;

    public int FTotalscoreComputer { get; set; } = 0;

    public decimal FPercentage { get; set; }

    public string FUserstatus { get; set; } = null!;

    public DateTime? FTestdate { get; set; } = DateTime.Now;

    public int? FJaId { get; set; }

    public int? FUserId { get; set; }

    public int? FTotalmarksGk { get; set; } 

    public int? FTotalmarksMaths { get; set; } 

    public int? FTotalmarksComputer { get; set; } 

    public virtual JobApplication? FJa { get; set; }

    public virtual ComputerTest? FTotalmarksComputerNavigation { get; set; }

    public virtual Gktest? FTotalmarksGkNavigation { get; set; }

    public virtual MathTest? FTotalmarksMathsNavigation { get; set; }

    public virtual User? FUser { get; set; }
}
