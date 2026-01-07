using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TesteBackendEnContact.Core.Interface.ContactBook
{
    public interface IContact
    {
        int Id { get; }
        int ContactBookId { get; }
        int CompanyId { get; }
        string Name { get; }
        int Phone { get; }
        string Email { get; }
        string Address { get; }
        
    }
}