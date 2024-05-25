using System;
using System.Collections.Generic;

namespace Recipes.Models;

public partial class About
{
    public decimal Id { get; set; }

    public string? AboutImage { get; set; }

    public string? AboutParagraph { get; set; }
}
