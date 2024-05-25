using System;
using System.Collections.Generic;

namespace Recipes.Models;

public partial class Payment
{
    public decimal Id { get; set; }

    public decimal? CardNumber { get; set; }

    public decimal? Ccv { get; set; }

    public DateTime? ExDate { get; set; }
}
