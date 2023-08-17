using System;
using System.Collections.Generic;

namespace OTTMyPlatform.Models;

public partial class UserShowWatchList
{
    public int UserWatchListId { get; set; }

    public int? ShowId { get; set; }

    public int? UserId { get; set; }

    public int? EpisodeId { get; set; }

    public int? Watched { get; set; }
}
