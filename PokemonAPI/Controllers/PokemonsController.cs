using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PokemonRepositoryLib;

namespace PokemonAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonsController : Controller
    {
        private readonly IPokemonRepository _pokemonService;

        public PokemonsController(IPokemonRepository pokemonService)
        {
            _pokemonService = pokemonService;
        }

        // GET: PokemonsController
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpGet]
        public ActionResult <IEnumerable<Pokemon>> Get([FromQuery] string? type = null, [FromQuery] string? nameIncludes = null, [FromQuery] string? sortBy = null)
        {
            IEnumerable<Pokemon> _pokemons = _pokemonService.Get(type, nameIncludes, sortBy);
            if (_pokemons.Any())
            {
                Response.Headers.Add("TotalCount", "" + _pokemons.Count());
                return Ok(_pokemons);
            }
            else
            {
                return NoContent();

            }

        }

        // GET: PokemonsController/Details/5
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public ActionResult<Pokemon?> GetPokemon(int id)
        {
            Pokemon? pokemon = _pokemonService.GetPokemonById(id);
            if (pokemon != null)
            {

                return Ok(pokemon);
            }
            else
            {
                return NotFound();
            }
        }


        // POST: PokemonsController/Create
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public ActionResult<Pokemon> Post([FromBody] Pokemon newPokemon)
        {
            try
            {
                Pokemon createdPokemon = _pokemonService.Add(newPokemon);
                return Created("/" + createdPokemon.PokemonId, createdPokemon);
            }
            catch (Exception ex) when (ex is ArgumentException || ex is ArgumentNullException)
            {
                return BadRequest(ex.Message);
            }

        }

        //PUT: PokemonsController/Edit/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Pokemon?> Put(int id, [FromBody] Pokemon pokemon)
        {

            Pokemon? updatedPokemon = _pokemonService.Update(id, pokemon);
            if (updatedPokemon != null)
            {
                return Ok(updatedPokemon);
            }
            else
            {
                return NotFound();
            }
        }

        // DELETE: PokemonsController/Delete/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Pokemon?> Delete(int id)
        {
            Pokemon? deletedPokemon = _pokemonService.Remove(id);
            if (deletedPokemon != null)
            {
                return Ok(deletedPokemon);
            }
            else
            {
                return NotFound();
            }
        }


    }
}
