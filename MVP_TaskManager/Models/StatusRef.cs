using System;
using System.Collections.Generic;

namespace MVP_TaskManager.Models;

public partial class StatusRef
{
    public int IdStatus { get; set; }

    public string? NameStatus { get; set; }

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
