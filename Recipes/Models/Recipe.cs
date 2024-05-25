using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recipes.Models;

public partial class Recipe
{
    public decimal Id { get; set; }

    public string? RecipeName { get; set; }

    public string? Ingrediients { get; set; }

    public string? Preparation { get; set; }

    public string? Descreption { get; set; }

    public decimal? Price { get; set; }

    public string? ImagePath { get; set; }
    [NotMapped]
    public IFormFile ImageFile { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedDate { get; set; }

    public decimal? CategoryId { get; set; }

    public decimal? UserId { get; set; }

    public virtual Category? Category { get; set; }

    public virtual User? User { get; set; }

    public virtual ICollection<UserRecipe> UserRecipes { get; set; } = new List<UserRecipe>();
}
