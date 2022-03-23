using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorCRUDApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuperHerosController : ControllerBase
    {
        private readonly DataContext _context;

        public SuperHerosController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<SuperHero>>> GetSuperHeros()
        {
            var heros = await _context.SuperHeroes
                .Include(h => h.Comic)
                .ToListAsync();
            return Ok(heros);
        }

        [HttpGet("comics")]
        public async Task<ActionResult<List<Comic>>> GetComics()
        {
            var comics = await _context.Comics.ToListAsync();
            return Ok(comics);
        }

        [HttpGet("{id}")]
        public ActionResult<List<SuperHero>> GetSingleHeroWithId(int id)
        {
            var hero = _context.SuperHeroes.FirstOrDefault(h => h.Id == id);

            if (hero == null)
            {
                return NotFound("Sorry, no hero exists :(");
            }
            return Ok(hero);
        }
        
        [HttpGet("comic/{id}")]
        public ActionResult<List<Comic>> GetSingleComicWithId(int id)
        {
            var comic = _context.Comics.FirstOrDefault(c => c.Id == id);

            if (comic == null)
            {
                return NotFound("Sorry, no comic exists :(");
            }
            return Ok(comic);
        }

        [HttpPost]
        public async Task<ActionResult<List<SuperHero>>> CreateSuperHero(SuperHero hero)
        {
            hero.Comic = null;
            _context.SuperHeroes.Add(hero);
            await _context.SaveChangesAsync();

            return Ok(await GetDbHeros()) ;
        }
        
        [HttpPost("comic")]
        public async Task<ActionResult<List<Comic>>> CreateComic(Comic comic)
        {
            _context.Comics.Add(comic);
            await _context.SaveChangesAsync();

            return Ok(await GetDbComics()) ;
        }

        // Updates a SuperHero in the db 

        [HttpPut("{id}")]
        public async Task<ActionResult<List<SuperHero>>> UpdateSuperHero(SuperHero hero, int id)
        {
            var dbHero = await _context.SuperHeroes
                .Include(sh => sh.Comic)
                .FirstOrDefaultAsync(sh => sh.Id == id);
            if (dbHero == null)
                return NotFound("Sorry, no hero was updated.");

            dbHero.FirstName = hero.FirstName;
            dbHero.LastName = hero.LastName;
            dbHero.HeroName = hero.HeroName;
            dbHero.ComicId = hero.ComicId;

            await _context.SaveChangesAsync();

            return Ok(await GetDbHeros()) ;
        }
        
        [HttpPut("comic/{id}")]
        public async Task<ActionResult<List<Comic>>> UpdateComic(Comic comic, int id)
        {
            var dbComic = await _context.Comics
                .FirstOrDefaultAsync(c => c.Id == id);
            if (dbComic == null)
                return NotFound("Sorry, no comic was updated.");

            dbComic.Name = comic.Name;
            await _context.SaveChangesAsync();

            return Ok(await GetDbComics()) ;
        }

        // Deletes a SuperHero from the db and returns an updated list from the db
        
        [HttpDelete("{id}")]
        public async Task<ActionResult<List<SuperHero>>> DeleteSuperHero(int id)
        {
            var dbHero = await _context.SuperHeroes
                .Include(sh => sh.Comic)
                .FirstOrDefaultAsync(sh => sh.Id == id);
            if (dbHero == null)
                return NotFound("Sorry, no hero was deleted.");

            _context.SuperHeroes.Remove(dbHero);
            await _context.SaveChangesAsync();

            return (Ok(await GetDbHeros())) ;
        }
        
        [HttpDelete("comic/{id}")]
        public async Task<ActionResult<List<Comic>>> DeleteComic(int id)
        {
            var dbComic = await _context.Comics
                .FirstOrDefaultAsync(c => c.Id == id);
            if (dbComic == null)
                return NotFound("Sorry, no hero was deleted.");

            _context.Comics.Remove(dbComic);
            await _context.SaveChangesAsync();

            return (Ok(await GetDbComics())) ;
        }

        // Retreives list of superheros from database
        // includes the Comic info
        private async Task<List<SuperHero>> GetDbHeros()
        {
            return await _context.SuperHeroes.Include(sh => sh.Comic).ToListAsync();
        }

        private async Task<List<Comic>> GetDbComics()
        {
            return await _context.Comics.ToListAsync();
        }

    }
}
