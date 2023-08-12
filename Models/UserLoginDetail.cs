using System;
using System.Collections.Generic;

namespace OTTMyPlatform.Models;

public partial class UserLoginDetail
{
    public string? UserName { get; set; }

    public string? Password { get; set; }

    public string? Email { get; set; }

    public int? PhoneNumber { get; set; }
}
