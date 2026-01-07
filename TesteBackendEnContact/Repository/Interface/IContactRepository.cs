using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TesteBackendEnContact.Core.Interface;
using TesteBackendEnContact.Core.Interface.ContactBook;

namespace TesteBackendEnContact.Repository.Interface
{
    public interface IContactRepository
    {
        Task<IContact> PostContactAsync(IContact contact);
        Task<IEnumerable<IContact>> GetAllContactsAsync();
        Task DeleteAsync(int Id);
        Task<IEnumerable<IContact>> SearchContactsAsync(string search);
        Task<IEnumerable<IContactBookResult>> SearchContactsInContactBookAsync(string search, int page, int pageSize);
    }
}