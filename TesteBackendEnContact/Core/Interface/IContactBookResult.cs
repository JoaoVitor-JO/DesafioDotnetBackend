using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TesteBackendEnContact.Core.Interface.ContactBook;

namespace TesteBackendEnContact.Core.Interface
{
    public interface IContactBookResult
    {
        int ContactBookId { get; }
        string ContactBookName { get; }   
        IEnumerable<IContact> Contacts { get; }
    }
}