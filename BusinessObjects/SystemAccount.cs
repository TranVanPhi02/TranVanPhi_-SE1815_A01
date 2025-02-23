using System;
using System.Collections.Generic;

namespace BusinessObjects;

public partial class SystemAccount
{
    public short AccountId { get; set; }

    public string? AccountName { get; set; }

    public string? AccountEmail { get; set; }

    public int? AccountRole { get; set; }

    public string? AccountPassword { get; set; }
    public bool IsDisabled { get; set; } = false;

    public virtual ICollection<NewsArticle> NewsArticles { get; set; } = new List<NewsArticle>();
}
