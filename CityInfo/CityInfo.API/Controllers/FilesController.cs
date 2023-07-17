using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers;

[ApiController]
[Route("/api/files")]
public class FilesController : ControllerBase
{
    [HttpGet("{fileId}")]
    public ActionResult GetFile(int fileId)
    {
        // Hard coded for demo purposes.
        // File paths start at the root.
        var filePath = "file1.txt";

        if (!System.IO.File.Exists(filePath))
        {
            return NotFound();
        }

        var bytes = System.IO.File.ReadAllBytes(filePath);
        
        // When this method is called, you automatically download the file
        return File(bytes, "text/plain", Path.GetFileName(filePath));
    }
}