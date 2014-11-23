using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Linq;
using Windows.ApplicationModel;

using Parse;

using CocktailBible.Models;
using System.Threading.Tasks;
using Windows.Storage;
using SQLite;
using CocktailBible.Utils;

namespace CocktailBible.ViewModels
{
    public class RecipeViewModel: ViewModelBase
    {
        private Recipe _recipe;

        public RecipeViewModel()
        {
            _recipe = new Recipe();
        }

        public Recipe Recipe
        {

            get { return _recipe; }
            set
            {
                _recipe = value;
                OnPropertyChanged("Recipe");
            }
        } 

        public string Name 
        {
            get { return _recipe.Name; }
            set
            {
                _recipe.Name = value;
                OnPropertyChanged("Name");
            }
        }

        public string Description 
        {
            get { return _recipe.Description; }
            set
            {
                _recipe.Description = value;
                OnPropertyChanged("Description");
            }
        }

        public string Ingredients
        {
            get { return _recipe.Ingredients; }
            set
            {
                _recipe.Ingredients = value;
                OnPropertyChanged("Ingredients");
            }
        }

        public string Instructions
        {
            get { return _recipe.Instructions; }
            set
            {
                _recipe.Instructions = value;
                OnPropertyChanged("Instructions");
            }
        }

        public ParseFile ImageSource
        {
            get { return _recipe.ImageSource;  }
            set
            {
                _recipe.ImageSource = value;
                OnPropertyChanged("ImageSource");
            }
        }

        public async Task<bool> SaveData()
        {
            int existingRecipeCount = App.remoteDbRecipes.Where(r => r.ObjectId == _recipe.ObjectId).Count();

            try
            {
                if (existingRecipeCount <= 0)
                {
                    App.remoteDbRecipes.Add(_recipe);

                    // Add to Local Storage if the recipe doesn't exist
                    bool dbExists = await LocalDbManager.CheckDbAsync();
                    if (!dbExists)
                    {
                        await LocalDbManager.CreateDatabaseAsync();
                    }

                    await LocalDbManager.AddLocalRecipeAsync(_recipe.Name);
                    _recipe.IsLocal = true;
                }

                await _recipe.SaveAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
