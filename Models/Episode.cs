using System;
using System.Collections.Generic;

namespace OTTMyPlatform.Models;

public partial class Episode
{
    public int EpisodeId { get; set; }

    public int? EpisodeTimeDuration { get; set; }

    public int? ShowId { get; set; }
}
