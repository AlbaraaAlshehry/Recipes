using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recipes.Models;

public partial class Home
{
    public decimal Id { get; set; }

    public string? HeroImage { get; set; }

    public string? HomeParagraph { get; set; }

    public string? RecipesImage { get; set; }

    public string? AboutImage { get; set; }
    [NotMapped]
    public IFormFile ImageFile { get; set; }

    public string? AboutParagraph { get; set; }

    public string? FooterAddress { get; set; }
}
