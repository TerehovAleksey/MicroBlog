namespace MicroBlog.WebApi.Controllers
{
    [Route("api/files")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        [HttpPost("image")]
        public async Task<IActionResult> UploadImage([FromForm] IEnumerable<IFormFile> files)
        {
            const long maxFileSize = 1024 * 1024 * 5;
            string[] permittedExtensions = { ".jpg", ".jpeg", ".png" };

            try
            {
                var file = files.FirstOrDefault();

                if (file == null)
                {
                    return BadRequest();
                }
                
                if (file.Length is 0 or > maxFileSize)
                {
                    return BadRequest();
                }

                var ext = Path.GetExtension(file.FileName).ToLowerInvariant();

                if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
                {
                    return BadRequest();
                }

                var folderName = Path.Combine("wwwroot", "img");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                Directory.CreateDirectory(pathToSave);

                var newFileName = Path.ChangeExtension(Path.GetRandomFileName(), ext);
                var path = Path.Combine(pathToSave, newFileName);

                await using FileStream fs = new(path, FileMode.Create);
                await file.CopyToAsync(fs);

                return Ok(newFileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
    }
}
