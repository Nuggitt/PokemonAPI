using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PokemonRepositoryLib;

namespace PokemonAPI.Controllers
{
    // Define the namespace and declare necessary dependencies.
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonsController : Controller
    {
        // Declare a private variable to hold the Pokemon service.
        private readonly IPokemonRepository _pokemonService;

        // Constructor to initialize the Pokemon service.
        public PokemonsController(IPokemonRepository pokemonService)
        {
            _pokemonService = pokemonService;
        }

        // GET action to retrieve a list of Pokemon.
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpGet]
        public ActionResult<IEnumerable<Pokemon>> Get([FromQuery] string? type = null, [FromQuery] string? nameIncludes = null, [FromQuery] string? sortBy = null)
        {
            // Retrieve Pokemon based on provided query parameters.
            IEnumerable<Pokemon> _pokemons = _pokemonService.Get(type, nameIncludes, sortBy);

            // Check if any Pokemon is returned.
            if (_pokemons.Any())
            {
                // Add total count of Pokemon to response headers.
                Response.Headers.Append("TotalCount", "" + _pokemons.Count());
                // Return OK response with the list of Pokemon.
                return Ok(_pokemons);
            }
            else
            {
                // If no Pokemon is found, return a No Content response.
                return NoContent();
            }
        }

        // GET action to retrieve details of a specific Pokemon by ID.
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public ActionResult<Pokemon?> GetPokemon(int id)
        {
            // Retrieve the Pokemon with the given ID.
            Pokemon? pokemon = _pokemonService.GetPokemonById(id);
            // Check if the Pokemon is found.
            if (pokemon != null)
            {
                // Return OK response with the Pokemon details.
                return Ok(pokemon);
            }
            else
            {
                // If the Pokemon is not found, return a Not Found response.
                return NotFound();
            }
        }

        // POST action to create a new Pokemon.
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public ActionResult<Pokemon> Post([FromBody] Pokemon newPokemon)
        {
            try
            {
                // Add the new Pokemon and return a Created response with the created Pokemon.
                Pokemon createdPokemon = _pokemonService.Add(newPokemon);
                return Created("/" + createdPokemon.PokemonId, createdPokemon);
            }
            catch (Exception ex) when (ex is ArgumentException || ex is ArgumentNullException)
            {
                // If there's an error adding the Pokemon, return a Bad Request response with the error message.
                return BadRequest(ex.Message);
            }
        }

        // PUT action to update an existing Pokemon.
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Pokemon?> Put(int id, [FromBody] Pokemon pokemon)
        {
            // Update the Pokemon with the given ID and return OK response with the updated Pokemon.
            Pokemon? updatedPokemon = _pokemonService.Update(id, pokemon);
            if (updatedPokemon != null)
            {
                return Ok(updatedPokemon);
            }
            else
            {
                // If the Pokemon with the given ID is not found, return a Not Found response.
                return NotFound();
            }
        }

        // DELETE action to delete a Pokemon by ID.
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Pokemon?> Delete(int id)
        {
            // Remove the Pokemon with the given ID and return OK response with the deleted Pokemon.
            Pokemon? deletedPokemon = _pokemonService.Remove(id);
            if (deletedPokemon != null)
            {
                return Ok(deletedPokemon);
            }
            else
            {
                // If the Pokemon with the given ID is not found, return a Not Found response.
                return NotFound();
            }
        }
    }
}
