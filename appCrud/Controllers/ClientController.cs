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
        public ClientController(ApplicationDbContext context) {
            this.context = context;
                }

        [HttpGet]
        public List<Client> GetClients()
        {
            return context.Clients.OrderByDescending(c => c.Id).ToList();
        }
        [HttpGet("${id}")]
        public IActionResult GetClient(int id)
        {
            var client = context.Clients.Find(id);
            if (client == null)
            {
                return NotFound();
            }
            return Ok(client);
        }

        [HttpGet]
        public IActionResult createClient(Client client)
        {
            var otherClient= context.Clients.FirstOrDefault(c=>c.Email==client.Email);
            if (otherClient == null)
            {
                ModelState.AddModelError("Email", "The Email Address is already used");
                var validation = new ValidationProblemDetails(ModelState);
                return BadRequest(validation);
            }
           client.CreatedAt=DateTime.Now;
            context.Clients.Add(client);
            context.SaveChanges();
            return Ok(client);
        }

        [HttpPut("{id}")]
        public IActionResult updateClient(int id, Client client)
        {
            var otherClient=context.Clients.FirstOrDefault(c=>c.Id!=id&&c.Email==client.Email);
            if(otherClient == null)
            {
                ModelState.AddModelError("Email", "The Email Address is already used");
                var validation = new ValidationProblemDetails(ModelState);
                return BadRequest(validation);
            }
            var cnt = context.Clients.Find(id);
            if(cnt == null)
            {
                return NotFound();
            }
            context.SaveChanges();

            return Ok(cnt);
        }

        [HttpDelete("{id}")]
        public IActionResult deleteClient(int id)
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
