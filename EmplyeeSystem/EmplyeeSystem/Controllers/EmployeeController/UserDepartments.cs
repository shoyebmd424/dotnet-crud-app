using EmplyeeSystem.Config;
using EmplyeeSystem.Model.employee;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmplyeeSystem.Controllers.EmployeeController
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserDepartmentsController : ControllerBase
    {
        private readonly ApplicationDB _context;

        public UserDepartmentsController(ApplicationDB context)
        {
            _context = context;
        }

        // GET: api/UserDepartments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDepartment>>> GetUserDepartments()
        {
            try
            {
                var userDepartments = await _context.UserDepartments.ToListAsync();
                return Ok(userDepartments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        // POST: api/UserDepartments
        [HttpPost]
        public async Task<ActionResult<UserDepartment>> PostUserDepartment(UserDepartment userDepartment)
        {
            try
            {
                if (userDepartment == null)
                {
                    return BadRequest("UserDepartment data is null.");
                }

                _context.UserDepartments.Add(userDepartment);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetUserDepartment", new { id = userDepartment.UserId }, userDepartment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}
