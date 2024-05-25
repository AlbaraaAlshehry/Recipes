using System;
using System.Collections.Generic;

namespace Recipes.Models;

public partial class UserRecipe
{
    public decimal Id { get; set; }

    public decimal? UserId { get; set; }

    public decimal? RecipeId { get; set; }

    public DateTime? ParchaseDate { get; set; }

    public virtual Recipe? Recipe { get; set; }

    public virtual User? User { get; set; }
}
