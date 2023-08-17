using System;
using System.Collections.Generic;

namespace OTTMyPlatform.Models;

public partial class Tvshow
{
    public int ShowId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public string? imagePath { get; set; }
}
