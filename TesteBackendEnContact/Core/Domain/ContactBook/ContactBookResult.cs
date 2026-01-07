using System.Collections.Generic;
using TesteBackendEnContact.Core.Interface;
using TesteBackendEnContact.Core.Interface.ContactBook;

namespace TesteBackendEnContact.Core.Domain.ContactBook
{
    public class ContactBookResult : IContactBookResult
    {
        public int ContactBookId { get; }
        public string ContactBookName { get; }
        public IEnumerable<IContact> Contacts { get; }

        public ContactBookResult(
            int contactBookId,
            string contactBookName,
            IEnumerable<IContact> contacts)
        {
            ContactBookId = contactBookId;
            ContactBookName = contactBookName;
            Contacts = contacts;
        }
    }
}
