using MagicVilla_VillaAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/VillaAPI")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<Villa>> GetVillas()
        {
            return Ok(VillaStore.Villas);
        }
        [HttpGet("{id:int}",Name ="GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Villa> GetVilla(int id)
        {
            if(id <= 0)
            {
                return BadRequest();
            }
            var villa = VillaStore.Villas?.FirstOrDefault(x => x.Id == id);
            if(villa == null)
            {
                return NotFound();
            }
            return Ok(villa);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<Villa> CreateVilla([FromBody]Villa villa)
        {
            if(VillaStore.Villas.FirstOrDefault(x=>x?.Name?.ToLower() == villa?.Name?.ToLower()) != null)
            {
                ModelState.AddModelError("CustomError", "Villa Already exists");
                return BadRequest (ModelState);
            }
            if(villa == null)
            {
                return BadRequest();
            }
            int id = VillaStore.Villas.LastOrDefault().Id;
            villa.Id =  id + 1;
            VillaStore.Villas.Add(villa);
            return CreatedAtRoute("GetVilla", new { id = villa.Id }, villa);
        }
        [HttpDelete("{id:int}",Name ="DeleteVilla")]

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteVilla(int id)
        {
            if(id == 0)
            {
                return BadRequest();
            }
            var villa = VillaStore.Villas.FirstOrDefault(x=>x.Id == id);
            if(villa == null) {
                return NotFound();
            }
            VillaStore.Villas.Remove(villa);
            return NoContent();
        }

        [HttpPut("{id:int}",Name ="UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateVilla(int id, [FromBody]Villa Input)
        {
            if(Input.Id != id)
            {
                return BadRequest();
            }
            var villa = VillaStore.Villas.FirstOrDefault(x => x.Id == id);
            if(villa == null)
            {
                return NotFound();
            }
            villa.Name = Input.Name;
            villa.Ocuupancy = Input.Ocuupancy;
            villa.SquareFt = Input.SquareFt;
            return NoContent();
        }
        [HttpPatch("{id:int}",Name ="UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult UpdatePartialVilla(int id, JsonPatchDocument<Villa> patch)
        {
            if(patch == null || id == 0)
            {
                return BadRequest();
            }
            var villa = VillaStore.Villas.FirstOrDefault(x => x.Id == id);
            if(villa == null)
            {
                return BadRequest();
            }
            patch.ApplyTo(villa, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();
        }
    }
}
