using System;
using System.Collections.Generic;

namespace MVP_TaskManager.Models;

public partial class Task
{
    public int IdTask { get; set; }

    public int? IdUser { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public int? IdStatus { get; set; }

    public DateOnly? DateCreate { get; set; }

    public virtual StatusRef? IdStatusNavigation { get; set; }

    public virtual User? IdUserNavigation { get; set; }
}
