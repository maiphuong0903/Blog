using System;
using System.Collections.Generic;

namespace Blog.Models;

public partial class Category
{
    public int CatId { get; set; }

    public string? CatName { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
}
