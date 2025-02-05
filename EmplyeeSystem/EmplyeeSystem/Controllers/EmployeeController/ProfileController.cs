using System;
using EmplyeeSystem.Config;
using EmplyeeSystem.Model.employee;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmplyeeSystem.Controllers.EmployeeController
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfilesController : ControllerBase
    {
        private readonly ApplicationDB _context;

        public ProfilesController(ApplicationDB context)
        {
            _context = context;
        }

        // GET: api/Profiles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Profile>>> GetProfiles()
        {
            try
            {
                var profiles = await _context.Profiles.ToListAsync();
                return Ok(profiles);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        // GET: api/Profiles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Profile>> GetProfile(int id)
        {
            try
            {
                var profile = await _context.Profiles.FindAsync(id);
                if (profile == null)
                {
                    return NotFound("Profile not found.");
                }
                return Ok(profile);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        // POST: api/Profiles
        [HttpPost]
        public async Task<ActionResult<Profile>> PostProfile(Profile profile)
        {
            try
            {
                if (profile == null)
                {
                    return BadRequest("Profile data is null.");
                }

                _context.Profiles.Add(profile);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetProfile", new { id = profile.Id }, profile);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        // DELETE: api/Profiles/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Profile>> DeleteProfile(int id)
        {
            try
            {
                var profile = await _context.Profiles.FindAsync(id);
                if (profile == null)
                {
                    return NotFound("Profile not found.");
                }

                _context.Profiles.Remove(profile);
                await _context.SaveChangesAsync();

                return Ok(profile);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}
