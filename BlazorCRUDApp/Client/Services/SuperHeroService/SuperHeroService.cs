using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace BlazorCRUDApp.Client.Services.SuperHeroService
{
    public class SuperHeroService : ISuperHeroService
    {
        private readonly HttpClient _http;
        private readonly NavigationManager _navigationManager;

        public SuperHeroService(HttpClient http, NavigationManager navigationManager)
        {
            _http = http;
            _navigationManager = navigationManager;
        }
        public List<SuperHero> Heros { get;set; } = new List<SuperHero>();
        public List<Comic> Comics { get; set ; } = new List<Comic>();

        public async Task CreateComic(Comic comic)
        {
            var result = await _http.PostAsJsonAsync("/api/superheros/comic", comic);
            List<Comic>? response = await result.Content.ReadFromJsonAsync<List<Comic>>();
            Comics = response;
            _navigationManager.NavigateTo("comics");

        }

        public async Task CreateHero(SuperHero hero)
        {
            var result = await _http.PostAsJsonAsync("/api/superheros", hero);
            await SetHeros(result);
        }

        public async Task DeleteComic(int id)
        {
            var result = await _http.DeleteAsync($"/api/superheros/comic/{id}");
            var response = await result.Content.ReadFromJsonAsync<List<Comic>>();
            Comics = response;
            _navigationManager.NavigateTo("comics");
        }

        public async Task DeleteHero(int id)
        {
            var result = await _http.DeleteAsync($"/api/superheros/{id}");
            await SetHeros(result);
        }

        public async Task GetComics()
        {
            List<Comic>? result = await _http.GetFromJsonAsync<List<Comic>>("/api/superheros/comics");
            if (result != null)
                Comics = result;
        }

        public async Task<Comic> GetSingleComicWithId(int id)
        {
            Comic? result = await _http.GetFromJsonAsync<Comic>($"/api/superheros/comic/{id}");
            if (result != null)
                return result;
            throw new Exception("Comic not found!");
        }

        public async Task<SuperHero> GetSingleHeroWithId(int id)
        {
            SuperHero? result = await _http.GetFromJsonAsync<SuperHero>($"/api/superheros/{id}");
            if (result != null)
                return result;
            throw new Exception("Hero not found!"); 
        }

        public async Task GetSuperHeros()
        {
            List<SuperHero>? result = await _http.GetFromJsonAsync<List<SuperHero>>("/api/superheros");
            if (result != null)
                Heros = result;

        }

        public async Task UpdateComic(Comic comic)
        {
            var result = await _http.PutAsJsonAsync($"/api/superheros/comic/{comic.Id}", comic);
            var response = await result.Content.ReadFromJsonAsync<List<Comic>>();
            Comics = response;
            _navigationManager.NavigateTo("comics");
        }

        public async Task UpdateSuperHero(SuperHero hero)
        {
            var result = await _http.PutAsJsonAsync($"/api/superheros/{hero.Id}", hero);
            await SetHeros(result);
        }

        private async Task SetHeros(HttpResponseMessage result)
        {
            var response = await result.Content.ReadFromJsonAsync<List<SuperHero>>();
            Heros = response;
            _navigationManager.NavigateTo("superheros");
        }
    }
}
