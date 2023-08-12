using System;
using System.Collections.Generic;

namespace OTTMyPlatform.Models;

public partial class Episode
{
    public int EpisodeId { get; set; }

    public int? EpisodeNumber { get; set; }

    public int? EpisodeTimeDuration { get; set; }

    public int? ShowId { get; set; }

    public virtual Tvshow? Show { get; set; }

    public virtual ICollection<UserShowWatchList> UserShowWatchLists { get; set; } = new List<UserShowWatchList>();
}
