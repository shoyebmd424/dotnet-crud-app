using appCrud.Models;
using appCrud.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace appCrud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        public ClientController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public List<Client> GetClients()
        {
            return context.Clients.OrderByDescending(c => c.Id).ToList();
        }

        [HttpGet("test")]
        public string Sample()
        {
            return "Server working fine";
        }

        [HttpGet("{id}")]
        public IActionResult GetClient(int id)
        {
            var client = context.Clients.Find(id);
            if (client == null)
            {
                return NotFound();
            }
            return Ok(client);
        }

        [HttpPost]
        public IActionResult CreateClient(Client client)
        {
            try
            {
                var existingClient = context.Clients.FirstOrDefault(c => c.Email == client.Email);
                if (existingClient != null)
                {
                    ModelState.AddModelError("Email", "The Email Address is already used");
                    var validation = new ValidationProblemDetails(ModelState);
                    return BadRequest(validation);
                }

                client.CreatedAt = DateTime.Now;
                context.Clients.Add(client);
                context.SaveChanges();
                return Ok(client);
            }catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error saving client: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateClient(int id, Client client)
        {
            var existingClient = context.Clients.FirstOrDefault(c => c.Id != id && c.Email == client.Email);
            //if (existingClient != null)
            //{
            //    ModelState.AddModelError("Email", "The Email Address is already used");
            //    var validation = new ValidationProblemDetails(ModelState);
            //    return BadRequest(validation);
            //}

            var cnt = context.Clients.Find(id);
            if (cnt == null)
            {
                return NotFound();
            }

            // Updating the client data
            cnt.FirstName = client.FirstName;
            cnt.Email = client.Email;

            context.SaveChanges();
            return Ok(cnt);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteClient(int id)
        {
            var client = context.Clients.Find(id);
            if (client == null)
            {
                return NotFound();
            }

            context.Clients.Remove(client);
            context.SaveChanges();
            return Ok();
        }
    }
}
