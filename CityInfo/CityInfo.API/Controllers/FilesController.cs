using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace CityInfo.API.Controllers;

[ApiController]
[Authorize]
[Route("/api/files")]
public class FilesController : ControllerBase
{
    private readonly FileExtensionContentTypeProvider _fileExtensionContentTypeProvider;

    public FilesController(FileExtensionContentTypeProvider fileExtensionContentTypeProvider)
    {
        _fileExtensionContentTypeProvider = fileExtensionContentTypeProvider
            ?? throw new System.ArgumentNullException(
                nameof(fileExtensionContentTypeProvider));
    }

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

        if (!_fileExtensionContentTypeProvider.TryGetContentType(
                filePath, out var contentType))
        {
            // Setting it to a default media type for arbitrary binary data.
            contentType = "application/octet-stream";
        }

        var bytes = System.IO.File.ReadAllBytes(filePath);
        
        // When this method is called, you automatically download the file
        return File(bytes, contentType, Path.GetFileName(filePath));
    }
}