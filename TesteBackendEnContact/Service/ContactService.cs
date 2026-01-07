using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using TesteBackendEnContact.Core.Domain.ContactBook;
using TesteBackendEnContact.Core.Interface.ContactBook;
using TesteBackendEnContact.Repository;
using TesteBackendEnContact.Repository.Interface;
using TesteBackendEnContact.Service.Interface;




namespace TesteBackendEnContact.Service
{
    public class ContactService : IContactService
    {
        private readonly IContactRepository _repository;
        private readonly ICompanyRepository _companyRepository;

        public ContactService(IContactRepository repository , ICompanyRepository companyRepository)
        {
            _repository =repository;
            _companyRepository = companyRepository;
        }

        

        

        public async Task<IEnumerable<IContact>> ReadFromCsvAsync(IFormFile file)
        {
            var contacts = new List<IContact>();

            using var stream = file.OpenReadStream();
            using var reader = new StreamReader(stream);

            while (!reader.EndOfStream)
            {
                var line =  await reader.ReadLineAsync();

                
                var colum = line.Split(';');

                int id = 0;
                int.TryParse(colum[0], out int contactBookId);
                int.TryParse(colum[1], out int companyId);
                var name = colum[2];
                int.TryParse(colum[3], out int phone);
                var email = colum[4];
                var address = colum[5];

                var company = await _companyRepository.GetAsync(contactBookId);
                
                IContact contact;

                if( company != null &&  name == company.Name)
                {
                     contact = new Contact(id, contactBookId, companyId, name, phone, email, address);

                }else{
                     contact = new Contact(id, 0, companyId, name, phone, email, address);
                }

                if (contactBookId != 0)
                {
                   var savedDB = await _repository.PostContactAsync(contact);
                contacts.Add(savedDB); 
                }

                
                
            }
            
            return contacts;

        }
       
        

        
    }
}