namespace BlazorCRUDApp.Client.Services.SuperHeroService
{
    public interface ISuperHeroService
    {
        List<SuperHero> Heros { get; set; }
        List<Comic> Comics { get; set; }
        Task GetComics();
        Task GetSuperHeros();
        Task<SuperHero> GetSingleHeroWithId(int id);
        Task<Comic> GetSingleComicWithId(int id);
        Task CreateHero(SuperHero hero);
        Task DeleteHero(int id);
        Task UpdateSuperHero(SuperHero hero);
        Task CreateComic(Comic comic);
        Task UpdateComic(Comic comic);
        Task DeleteComic(int id);

    }
}
