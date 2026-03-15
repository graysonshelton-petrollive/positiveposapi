using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PositivePOSAPI.Data;

namespace PositivePOSAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LocationsController : ControllerBase
{
    private readonly PositiveDbContext _db;
    public LocationsController(PositiveDbContext db) => _db = db;

    // GET api/locations?companyId=...
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] Guid? companyId = null)
    {
        var q = _db.Locations.AsNoTracking();

        if (companyId.HasValue)
            q = q.Where(x => x.CompanyGuid == companyId.Value); // CHANGE if needed

        var rows = await q.ToListAsync();
        return Ok(rows);
    }
}