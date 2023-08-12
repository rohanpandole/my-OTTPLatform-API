using System;
using System.Collections.Generic;

namespace OTTMyPlatform.Models;

public partial class User
{
    public int UserId { get; set; }

    public string? UserName { get; set; }

    public virtual ICollection<UserShowWatchList> UserShowWatchLists { get; set; } = new List<UserShowWatchList>();
}
