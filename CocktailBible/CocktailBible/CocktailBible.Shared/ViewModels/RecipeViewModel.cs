using CocktailBible.Models;
using Parse;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Windows.ApplicationModel;

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

        public string SaveData()
        {
            string result = string.Empty;

            try
            {

                Recipe existingRecipe = null;
                    
                  //(App.Recipes().Where(r => r.Id == Recipe.Id)).SingleOrDefault();

                if (existingRecipe != null)
                {
                    //App.dbRecipes.Remove(existingRecipe);
                    //App.dbRecipes.Add(_recipe);
                }
                else
                {
                    App.dbRecipes.Add(_recipe);
                }
                result = "Success";
            }
            catch
            {
                result = "This recipe was not saved.";
            }

            return result;
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
