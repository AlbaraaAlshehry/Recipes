using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recipes.Models;

public partial class User
{
    public decimal Id { get; set; }

    public string? Fname { get; set; }

    public string? Lname { get; set; }

    public string? Email { get; set; }

    public string? ImagePath { get; set; }
    [NotMapped]
    public IFormFile ImageFile { get; set; }
    public virtual ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();

    public virtual ICollection<Tetstimonial> Tetstimonials { get; set; } = new List<Tetstimonial>();

    public virtual ICollection<UserLogin> UserLogins { get; set; } = new List<UserLogin>();

    public virtual ICollection<UserRecipe> UserRecipes { get; set; } = new List<UserRecipe>();
}
