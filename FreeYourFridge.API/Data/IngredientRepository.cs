using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FreeYourFridge.API.DTOs;
using FreeYourFridge.API.Helpers;
using FreeYourFridge.API.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace FreeYourFridge.API.Data
{
    public class IngredientRepository : IIngredientRepository
    {
        private readonly IMakePartialUrl _makePartialUrl;
        private readonly DataContext _context;
        public IngredientRepository(DataContext context)
        {
            _context = context;
        }
    public async Task<IngredientDto> GetIngredientsFromAPI(int id)
    {
        var client = new RestClient($"https://api.spoonacular.com/food/ingredients/{id}/information?apiKey=410c59bf9317462d8f0c3a0b3b0f0586");
        var request = new RestRequest(Method.GET);
        IRestResponse response = await client.ExecuteAsync(request);
            if (response.IsSuccessful)
            {
                var content = JsonConvert.DeserializeObject<JToken>(response.Content);
                
                var idFromApi = content["id"].Value<int>();
                var originalNameFromApi =  content["originalName"].Value<string>();
                var imageFromApi = content["image"].Value<string>();
                var listPossibleUnitsFromApi = new List<string>();
                foreach(var unit in content["possibleUnits"])
                {
                    var possibleUnitsFromApi = unit.Value<string>();
                    listPossibleUnitsFromApi.Add(possibleUnitsFromApi);
                }

                return new IngredientDto
                {
                    id = idFromApi,
                    originalName = originalNameFromApi,
                    possibleUnits = listPossibleUnitsFromApi,
                    image = "https://spoonacular.com/cdn/ingredients_100x100/"+imageFromApi
                };
            }

        return null;
    }
    public async Task<List<ListOfIngredients>> GetAllIngredients()
    {
        var ListOfIngredients = _context.ListOfIngredients.ToList();
        return ListOfIngredients;
    }
    public async void Add(Ingredient newIngredient, Fridge fridge, string image)
    {
        var newIngredientToAdd = new Ingredient(){
            SpoonacularId = newIngredient.Id,
            Amount = newIngredient.Amount,
            Name = newIngredient.Name,
            Fridge = fridge,
            FridgeId =fridge.Id,
            Unit = newIngredient.Unit,
            Image = image
        };
        _context.Add( newIngredientToAdd);
        await this.SaveAll();
    }
    public async Task<bool> SaveAll()
    {
        return await _context.SaveChangesAsync() > 0;
    }

    }
}