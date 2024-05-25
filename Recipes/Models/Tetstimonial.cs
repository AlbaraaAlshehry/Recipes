using System;
using System.Collections.Generic;

namespace Recipes.Models;

public partial class Tetstimonial
{
    public decimal Id { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? TestimonialText { get; set; }

    public decimal? UserId { get; set; }

    public virtual User? User { get; set; }
}
