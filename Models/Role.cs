using System;
using System.Collections.Generic;

namespace OTTMyPlatform.Models;

public partial class Role
{
    public int RoleId { get; set; }

    public string? RoleType { get; set; }

    public virtual ICollection<UserLoginDetail> UserLoginDetails { get; set; } = new List<UserLoginDetail>();
}
