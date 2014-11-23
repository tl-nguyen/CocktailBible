using System;
using System.Collections.Generic;
using System.Text;

using SQLite;

namespace CocktailBible.Models
{
    [Table("LocalRecipes")]
    public class LocalRecipe
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Unique, MaxLength(150)]
        public string Name { get; set; }
    }
}
