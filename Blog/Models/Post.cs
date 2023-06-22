using System;
using System.Collections.Generic;

namespace Blog.Models;

public partial class Post
{
    public int PostId { get; set; }

    public string? PostName { get; set; }

    public string? ShortContent { get; set; }

    public string? Contents { get; set; }

    public string? Picture { get; set; }

    public DateTime? CreateDate { get; set; }

    public string? Author { get; set; }

    public int AccountId { get; set; }

    public int CatId { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual Category Cat { get; set; } = null!;

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
}
