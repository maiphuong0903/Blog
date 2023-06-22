using System;
using System.Collections.Generic;

namespace Blog.Models;

public partial class Comment
{
    public int CommentId { get; set; }

    public string? CommentText { get; set; }

    public DateTime? CommentDate { get; set; }
    public string FullName { get; set; }
    public int? AccountId { get; set; }

    public int? PostId { get; set; }

    public int? ParentId { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual Post Post { get; set; } = null!;
}
