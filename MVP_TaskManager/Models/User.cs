using System;
using System.Collections.Generic;

namespace MVP_TaskManager.Models;

public partial class User
{
    public int Id { get; set; }

    public string? Username { get; set; }

    public string? Login { get; set; }

    public string? Password { get; set; }

    public DateOnly? RegDate { get; set; }

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
