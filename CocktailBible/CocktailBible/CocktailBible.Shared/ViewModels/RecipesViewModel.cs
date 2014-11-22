using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using System.Linq;

using CocktailBible.Models;
using Parse;

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
                Recipes = App.dbRecipes;
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

            App.IsDataLoaded = true;

            this.Recipes = _returnRecipes.AsQueryable().ToList();

            App.dbRecipes = this.recipes;
        }
    }
}
