using System;
using System.Collections.Generic;

namespace OTTMyPlatform.Models;

public partial class UserLoginDetail
{
    public int Id { get; set; }

    public string? UserName { get; set; }

    public string? Password { get; set; }

    public string? Email { get; set; }

    public int? PhoneNumber { get; set; }

    public int? RoleId { get; set; }

    public virtual Role? Role { get; set; }
}
