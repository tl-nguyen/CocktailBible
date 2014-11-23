using CocktailBible.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace CocktailBible.Utils
{
    public class LocalDbManager
    {
        private const string DBNAME = "cocktailBible";

        public static async Task<bool> CheckDbAsync()
        {
            bool dbExist = true;

            try
            {
                StorageFile sf = await ApplicationData.Current.LocalFolder.GetFileAsync(DBNAME);
            }
            catch (Exception)
            {
                dbExist = false;
            }

            return dbExist;
        }

        public static async Task CreateDatabaseAsync()
        {
            try
            {
                SQLiteAsyncConnection conn = new SQLiteAsyncConnection(DBNAME);
                await conn.CreateTableAsync<LocalRecipe>();
            }
            catch (Exception e)
            {

            }

        }

        public static async Task<LocalRecipe> AddLocalRecipeAsync(string recipeName)
        {
            var recipe = new LocalRecipe
            {
                Name = recipeName
            };

            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(DBNAME);
            await conn.InsertAsync(recipe);

            return recipe;
        }

        public static async Task<IEnumerable<LocalRecipe>> GetAllLocalRecipes()
        {
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(DBNAME);

            List<LocalRecipe> result = await conn.Table<LocalRecipe>().ToListAsync();

            return result;
        }

        public static async Task DeleteRecipeAsync(string name)
        {
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(DBNAME);

            // Retrieve Article
            var article = await conn.Table<LocalRecipe>().Where(x => x.Name == name).FirstOrDefaultAsync();
            if (article != null)
            {
                // Delete record
                await conn.DeleteAsync(article);
            }
        }

    }
}
