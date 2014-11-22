using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Linq;
using Windows.ApplicationModel;

using Parse;

using CocktailBible.Models;
using System.Threading.Tasks;

namespace CocktailBible.ViewModels
{
    public class RecipeViewModel: ViewModelBase
    {
        private Recipe _recipe;

        public Recipe Recipe
        {

            get { return _recipe; }
            set
            {
                _recipe = value;
                OnPropertyChanged("Recipe");
            }
        } 
        
        public RecipeViewModel()
        {
            _recipe = new Recipe();
        }

        public async Task<bool> SaveData()
        {
            int existingRecipeCount = App.dbRecipes.Where(r => r.ObjectId == _recipe.ObjectId).Count();

            try
            {
                if (existingRecipeCount <= 0)
                {
                    App.dbRecipes.Add(_recipe);
                }

                await _recipe.SaveAsync();

                return true;
            }
            catch
            {
                return false;
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
    }
}
