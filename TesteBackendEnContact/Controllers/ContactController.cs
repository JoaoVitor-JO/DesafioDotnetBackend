using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using TesteBackendEnContact.Core.Domain.ContactBook;
using TesteBackendEnContact.Core.Interface;
using TesteBackendEnContact.Core.Interface.ContactBook;
using TesteBackendEnContact.Repository;
using TesteBackendEnContact.Repository.Interface;
using TesteBackendEnContact.Service;
using TesteBackendEnContact.Service.Interface;

namespace TesteBackendEnContact.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContactController : ControllerBase
    {
        private readonly ILogger<ContactController> _logger;
        private readonly IContactService _contact;

        public ContactController(ILogger<ContactController> logger, IContactService contactService)
        {
            _logger = logger;
            _contact = contactService;
        }



        [HttpPost("Upload")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
           var contacts = await _contact.ReadFromCsvAsync(file); 
           
           return Ok(contacts);
        }

        [HttpGet]
        public async Task<IEnumerable<IContact>> Get([FromServices] IContactRepository contactRepository)
        {
            return await contactRepository.GetAllContactsAsync();
        }         

        [HttpGet("search")]
        public async Task<IEnumerable<IContact>> Search(
            [FromQuery]string search,
            [FromServices] IContactRepository contactRepository)
        {
            return await contactRepository.SearchContactsAsync(search);
        }

        [HttpGet("searchWichContactBook")]
        public async Task<IEnumerable<IContactBookResult>> SearchWichContactBook(
            [FromQuery]string search,
            [FromServices] IContactRepository contactRepository,
            [FromQuery]int page =1,
            [FromQuery]int pageSize =3
            )
        {
            return await contactRepository.SearchContactsInContactBookAsync(search,page,pageSize);
        }

         [HttpDelete("id/{id:int}")]
        public async Task DeleteID(
            int id, 
            [FromServices] IContactRepository contactRepository)
        {
            await contactRepository.DeleteAsync(id);
        }
    }
}
