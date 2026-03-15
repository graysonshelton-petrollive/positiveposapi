using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PositivePOSAPI.Data;

namespace PositivePOSAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    private readonly PositiveDbContext _db;

    public HealthController(PositiveDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var canConnect = await _db.Database.CanConnectAsync();
        return Ok(new { ok = true, db = canConnect ? "connected" : "not-connected" });
    }
}