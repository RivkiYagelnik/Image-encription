using Microsoft.AspNetCore.Mvc;
using DAL;
using BL;
using BL.Interfaces;
using BL.Services;
using DAL.DTO;
using Microsoft.AspNetCore.Authorization;

namespace Image_Encryption.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PictureController : Controller
    {
        private readonly IPictureService _IPictureService;
        public PictureController(IPictureService IPictureService)
        {
            _IPictureService = IPictureService;
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> AddPicture([FromQuery] string inputPath, [FromQuery] string message, [FromQuery] int id, [FromQuery] byte[]? key = null, [FromQuery] byte[]? iv = null)
        {
            try
            {
                Task<bool> res;
                if (key == null)
                {
                    res = _IPictureService.AddPicture(inputPath, message, DateTime.Now, id);
                }
                else
                {
                    res = _IPictureService.AddPicture(inputPath, message, key, iv, DateTime.Now, id);
                }
                return Ok(res);
            }
            catch (Exception ex)
            {
                // Log the exception (ex)
                return StatusCode(500, "An error occurred while adding the picture.");
            }
        }
        //[HttpPost]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public async Task<ActionResult<bool>> AddPicture([FromQuery] string inputPath, [FromQuery] string message, [FromQuery] int id, [FromQuery] byte[]? key = null, [FromQuery] byte[]? iv = null)
        //{
        //    try
        //    {
        //        Task<bool> res;
        //        if (key == null)
        //        {
        //            res = _IPictureService.AddPicture(inputPath, message, DateTime.Now, id);
        //        }
        //        else
        //        {
        //            res = _IPictureService.AddPicture(inputPath, message, key, iv, DateTime.Now, id);
        //        }
        //        return Ok(await res);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception (ex)
        //        Console.WriteLine(ex.Message);
        //        Console.WriteLine(ex.StackTrace);
        //        return StatusCode(500, "An error occurred while adding the picture.");
        //    }
        //}

        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> DeletePicture(int id)
        {
            try
            {
                var res = await _IPictureService.DeletePicture(id);
                return Ok(res);
            }
            catch (Exception ex)
            {

                return BadRequest(ex);
            }
        }
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PictureDto>> GetPictureById(int id)
        {
            try
            {
                var res = await _IPictureService.GetPictureById(id);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}"); 
            }
        }

        [Authorize]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ICollection<PictureDto>>> GetAllPictures()
        {
            try
            {
                var res = await _IPictureService.GetAllPictures();
                return Ok(res);
            }
            catch (Exception ex)
            {

                return BadRequest(ex);
            }
        }

        [HttpGet("DecryptMessageFromImage")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> DecryptMessageFromImage([FromQuery]int id,[FromQuery] byte[] key, [FromQuery]byte[] iv)
        {
            try
            {
                var res= await _IPictureService.DecryptMessageFromImage(id, key, iv);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
