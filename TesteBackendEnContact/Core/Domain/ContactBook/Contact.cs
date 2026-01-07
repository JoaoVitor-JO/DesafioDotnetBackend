using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TesteBackendEnContact.Core.Interface.ContactBook;

namespace TesteBackendEnContact.Core.Domain.ContactBook
{
    public class Contact: IContact
    {
        public int Id { get; private set; }
        public int ContactBookId { get;private set; }
        public int CompanyId { get;private set; }
        public string Name { get;private set; }
        public int Phone { get;private set; }
        public string Email { get;private set; }
        public string Address { get;private set; }
        public Contact(int id,int contactBookId,int companyId, string name, int phone, string email, string address)
        {
            Id = id;
            ContactBookId = contactBookId;
            CompanyId = companyId;
            Name = name ;
            Phone = phone ;
            Email = email ;
            Address = address;
        }

    }
}