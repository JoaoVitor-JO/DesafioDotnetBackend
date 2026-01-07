using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TesteBackendEnContact.Core.Interface.ContactBook;
using Microsoft.AspNetCore.Http;

namespace TesteBackendEnContact.Service.Interface
{
    public interface IContactService
    {
        Task<IEnumerable<IContact>> ReadFromCsvAsync(IFormFile file);
        
    }
}