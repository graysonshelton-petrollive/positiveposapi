using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PositivePOSAPI.Data;

namespace PositivePOSAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CompaniesController : ControllerBase
{
    private readonly PositiveDbContext _db;
    public CompaniesController(PositiveDbContext db) => _db = db;

    // GET api/companies
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        // CHANGE: _db.Companies to the actual DbSet name in PositiveDbContext
        var rows = await _db.Companies
            .AsNoTracking()
            .ToListAsync();

        return Ok(rows);
    }

    // GET api/companies/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var row = await _db.Companies
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.CompanyGuid == id); // CHANGE props if different

        return row is null ? NotFound() : Ok(row);
    }
}