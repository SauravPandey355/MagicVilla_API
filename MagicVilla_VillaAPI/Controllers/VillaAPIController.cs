using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Logging;
using MagicVilla_VillaAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Net;

namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/VillaAPI")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        private readonly ILogger<VillaAPIController> logger;
        private readonly AppDbContext db;
        private APIResponse response;
        public VillaAPIController(ILogger<VillaAPIController> _logger,AppDbContext _db)
        {
            logger = _logger;
            db = _db;
            this.response = new APIResponse();
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetVillas() 
        {
            db.Villas.ToList();
            response.StatusCode = HttpStatusCode.OK;
            response.Status = "Success";
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            response.response = await db.Villas.ToListAsync();
            stopwatch.Stop();
            response.Message = $"Total time taken by Api is {stopwatch.ElapsedMilliseconds} ms";
            return Ok(response);
        }
        [HttpGet("{id:int}",Name ="GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VillaDTO> GetVilla(int id)
        {
            if(id <= 0)
            {
                return BadRequest();
            }
            var villa = db.Villas.FirstOrDefault(x => x.Id == id);
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
        public ActionResult<VillaDTO> CreateVilla([FromBody]VillaDTO villaDTO)
        {

            if (db.Villas.FirstOrDefault(x => x.Name.ToLower() == villaDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("CustomError", "Villa Already exists");
                return BadRequest (ModelState);
            }
            if(villaDTO == null)
            {
                return BadRequest();
            }
            if(villaDTO.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            Villa villa = villaDTO;
            villa.CreatedDate = DateTime.Today;
            villa.UpdatedDate = DateTime.Today;
            db.Villas.Add(villa);
            db.SaveChanges();
            return CreatedAtRoute("GetVilla", new { id = villaDTO.Id }, villaDTO);
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
            var villa = db.Villas.FirstOrDefault(x=>x.Id == id);
            if(villa == null) {
                return NotFound();
            }
            db.Villas.Remove(villa);
            return NoContent();
        }

        [HttpPut("{id:int}",Name ="UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateVilla(int id, [FromBody]VillaDTO Input)
        {
            if(Input.Id != id)
            {
                return BadRequest();
            }
            var villa = db.Villas.AsNoTracking().FirstOrDefault(x => x.Id == id);
            if(villa == null)
            {
                return NotFound();
            }
            villa = Input;
            villa.UpdatedDate = DateTime.Today;
            db.Villas.Update(villa);
            db.SaveChanges();
            return NoContent();
        }
        [HttpPatch("{id:int}",Name ="UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult UpdatePartialVilla(int id, JsonPatchDocument<VillaDTO> patch)
        {
            if(patch == null || id == 0)
            {
                return BadRequest();
            }
            var villa = db.Villas.AsNoTracking().FirstOrDefault(x => x.Id == id);
            if(villa == null)
            {
                return BadRequest();
            }
            patch.ApplyTo(villa, ModelState);
            villa.UpdatedDate = DateTime.Today;
            db.Villas.Update(villa);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();
        }
    }
}
