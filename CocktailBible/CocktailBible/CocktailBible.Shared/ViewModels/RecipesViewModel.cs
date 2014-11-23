using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using System.Linq;

using CocktailBible.Models;
using Parse;
using SQLite;
using Windows.Storage;
using CocktailBible.Utils;

namespace CocktailBible.ViewModels
{
    public class RecipesViewModel
    {
        private ObservableCollection<Recipe> recipes;

        public RecipesViewModel()
        {
            if (!App.IsDataLoaded)
            {
                LoadData();
            }
        }

        public IEnumerable<Recipe> Recipes
        {
            get
            {
                if (this.recipes == null)
                {
                    this.recipes = new ObservableCollection<Recipe>();
                }
                return this.recipes;
            }
            set
            {
                if (this.recipes == null)
                {
                    this.recipes = new ObservableCollection<Recipe>();
                }
                this.recipes.Clear();
                foreach (var item in value)
                {
                    this.recipes.Add(item);
                }
            }
        }

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        private async Task LoadData()
        {
            var _returnRecipes = await new ParseQuery<Recipe>().FindAsync();

            // Add to Local Storage if the recipe doesn't exist
            bool dbExists = await LocalDbManager.CheckDbAsync();

            if (!dbExists)
            {
                await LocalDbManager.CreateDatabaseAsync();
            }

            var _localRecipes = await LocalDbManager.GetAllLocalRecipes();

            App.IsDataLoaded = true;

            foreach (var recipe in _returnRecipes)
            {
                foreach (var localRecipe in _localRecipes)
                {
                    if (recipe.Name == localRecipe.Name)
                    {
                        recipe.IsLocal = true;
                    }
                }
            }

            this.Recipes = _returnRecipes.AsQueryable().ToList();

            App.remoteDbRecipes = this.recipes;
        }
    }
}
