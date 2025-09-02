using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExploreNasaBackendService.Models;

namespace ExploreNasaBackendService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ApodsController : ControllerBase
{
    private readonly DataBaseContext _context;
    public ApodsController(DataBaseContext context)
    {
        _context = context;
    }

    // GET: api/Apods
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodayApod>>> GetTodayApod()
    {
        return await _context.TodayApod.ToListAsync();
    }

    // GET: api/Apods
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodayApod>>> GetTodayApodFromNasa()
    {

        return await _context.TodayApod.ToListAsync();
    }

    // GET: api/Apods/5
    [HttpGet("{id}")]
    public async Task<ActionResult<TodayApod>> GetTodayApod(string id)
    {
        var todayApod = await _context.TodayApod.FindAsync(id);
    
        if (todayApod == null)
        {
            return NotFound();
        }
    
        return todayApod;
    }
    
    // PUT: api/Apods/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTodayApod(string id, TodayApod todayApod)
    {
        if (id != todayApod.Id)
        {
            return BadRequest();
        }
    
        _context.Entry(todayApod).State = EntityState.Modified;
    
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!TodayApodExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }
    
        return NoContent();
    }
    
    // POST: api/Apods
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<TodayApod>> PostTodayApod(TodayApod todayApod)
    {
        _context.TodayApod.Add(todayApod);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            if (TodayApodExists(todayApod.Id))
            {
                return Conflict();
            }
            else
            {
                throw;
            }
        }
    
        return CreatedAtAction("GetTodayApod", new { id = todayApod.Id }, todayApod);
    }
    
    // DELETE: api/Apods/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodayApod(string id)
    {
        var todayApod = await _context.TodayApod.FindAsync(id);
        if (todayApod == null)
        {
            return NotFound();
        }
    
        _context.TodayApod.Remove(todayApod);
        await _context.SaveChangesAsync();
    
        return NoContent();
    }
    
    private bool TodayApodExists(string id)
    {
        return _context.TodayApod.Any(e => e.Id == id);
    }
}
