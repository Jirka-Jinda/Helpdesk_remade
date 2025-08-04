using Microsoft.AspNetCore.Mvc;
using Services.Abstractions.Services;

namespace Helpdesk.Controllers;

public class ArchiveController : Controller
{
    private readonly IArchiveService _archiveService;

    public ArchiveController(IArchiveService archiveService)
    {
        _archiveService = archiveService;
    }

    [HttpGet]
    public async Task<IActionResult> Overview(string? filter = null)
    {
        var archives = await _archiveService.GetAllAsync();

        if (filter != null && !string.IsNullOrWhiteSpace(filter))
        {
            archives = archives
                .Where(t => t.Header.Contains(filter, StringComparison.OrdinalIgnoreCase) ||
                    t.Category.ToString().Contains(filter, StringComparison.OrdinalIgnoreCase))
                .ToList();

            ViewBag.Filter = filter;
        }
            
        return View(archives);
    }

    [HttpGet]
    public async Task<IActionResult> Detail(Guid archiveId)
    {
        var archive = await _archiveService.GetAsync(archiveId);

        return View(archive);
    }
}
