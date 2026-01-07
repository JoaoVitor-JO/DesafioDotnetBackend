
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TesteBackendEnContact.Core.Interface.ContactBook;
using TesteBackendEnContact.Repository.Interface;
using TesteBackendEnContact.Database;
using TesteBackendEnContact.Core.Domain.ContactBook;
using Dapper;
using Microsoft.Data.Sqlite;
using Dapper.Contrib.Extensions;
using System.Runtime.CompilerServices;
using Microsoft.Data.SqlClient;
using TesteBackendEnContact.Core.Interface;
using Microsoft.IdentityModel.Tokens;


namespace TesteBackendEnContact.Repository
{
    public class ContactRepository : IContactRepository
    {
        private readonly DatabaseConfig databaseConfig;

        public ContactRepository(DatabaseConfig databaseConfig)
        {
            this.databaseConfig = databaseConfig;
        }

        public async Task<IEnumerable<IContact>> GetAllContactsAsync()
        {
            using var connection = new SqliteConnection(databaseConfig.ConnectionString);
            await connection.OpenAsync();

            var query = "SELECT * FROM Contact";
            var result = await connection.QueryAsync<ContactDao>(query);

            var contacts = new List<IContact>();

            foreach(var contactDao in result.ToList())
            {
                IContact contact = new Contact(contactDao.Id, contactDao.ContactBookId, contactDao.CompanyId, contactDao.Name, contactDao.Phone, contactDao.Email, contactDao.Address);
                contacts.Add(contact);
            }     
            return contacts;
        }

        public async Task<IEnumerable<IContact>> SearchContactsAsync(string search)
        {
            using var connection = new SqliteConnection(databaseConfig.ConnectionString);
            await connection.OpenAsync();
            var query = @"SELECT
             c.Id,
             c.ContactBookId,
             c.CompanyId,
             c.Name,
             c.Phone,
             c.Email,
             c.Address
            
             FROM Contact c
             
             WHERE(
             c.Name      LIKE '%' || @search || '%'
             OR c.Email  LIKE '%' || @search || '%'
             OR c.Phone  LIKE '%' || @search || '%'
             OR c.Address LIKE '%' || @search || '%'
             )
             ORDER BY c.Id
             ;  ";

            var result = await connection.QueryAsync<ContactDao>(query, new {search});

            var contacts = new List<IContact>();

            foreach(var contactDao in result)
            {
                IContact contact = new Contact(contactDao.Id, contactDao.ContactBookId, contactDao.CompanyId, contactDao.Name, contactDao.Phone, contactDao.Email, contactDao.Address);
                contacts.Add(contact);
                
            }     
            return contacts;
        }

      

        public async Task<IEnumerable<IContactBookResult>> SearchContactsInContactBookAsync(string search, int page, int pageSize)
        {
            
               
            var offset = (page - 1) * pageSize;

            using var connection = new SqliteConnection(databaseConfig.ConnectionString);
            await connection.OpenAsync();

            var query = @"
             SELECT
                c.Id,
                c.ContactBookId,
                c.CompanyId,
                c.Name,
                c.Phone,
                c.Email,
                c.Address,
                
                cb.Name AS ContactBookName

             FROM Contact c
             LEFT JOIN ContactBook cb ON cb.Id = c.ContactBookId
             WHERE(
                c.Name      LIKE '%' || @search || '%'
                OR c.Email  LIKE '%' || @search || '%'
                OR c.Phone  LIKE '%' || @search || '%'
                OR c.Address LIKE '%' || @search || '%'
                OR cb.Name  LIKE '%' || @search || '%'
             )
             ORDER BY c.Id
             LIMIT @pageSize OFFSET @offset;
             ";

               

             var result = await connection.QueryAsync<ResultQueryDao>(query,new{search,pageSize,offset});
             var contacts = new List<IContactBookResult>();

             var groupedResult = result.GroupBy(r => new { r.ContactBookId, r.ContactBookName });

             foreach (var group in groupedResult)
             {
                 var contactList = group.Select(r => new Contact(
                        r.Id, 
                        r.ContactBookId, 
                        r.CompanyId, 
                        r.Name, 
                        r.Phone, 
                        r.Email, 
                        r.Address)
                    ).ToList();
                
                IContactBookResult bookResult = new ContactBookResult(
                    group.Key.ContactBookId, 
                    group.Key.ContactBookName, 
                    contactList);

                 contacts.Add(bookResult);
             }

            return contacts;
        }
        public async Task DeleteAsync(int id)
        {
            
            using var connection = new SqliteConnection(databaseConfig.ConnectionString);
            await connection.OpenAsync();

            var dao =await connection.GetAsync<ContactDao>(id);

            await connection.DeleteAsync(dao);           
            
        }
        
        public async Task<IContact> PostContactAsync(IContact contact)
        {
            using var connection = new SqliteConnection(databaseConfig.ConnectionString);
            await connection.OpenAsync();
            
            var dao = new ContactDao(contact);
            dao.Id = await connection.InsertAsync(dao);
                        
            return dao.Export();
        }

        [Table("Contact")]
        public class ContactDao 
        {
            [Key]
            public int Id{get;set;}

            public int ContactBookId{get;set;}
            public int CompanyId{get;set;}
            public string Name{get;set;}
            public int Phone{get;set;}
            public string Email{get;set;}
            public string Address{get;set;}

            public ContactDao()
            {
                
            }
            public ContactDao(IContact contact)
            {
               Id = contact.Id;
               ContactBookId = contact.ContactBookId;
               CompanyId = contact.CompanyId;
               Name = contact.Name;
               Phone = contact.Phone;
               Email = contact.Email;
               Address = contact.Address;

            }

            

            public IContact Export() => new Contact(Id, ContactBookId,CompanyId, Name, Phone, Email, Address);

        }
        public class ResultQueryDao 
        {
            public int Id{get;set;}

            public int ContactBookId{get;set;}
            public string ContactBookName{get;set;}
            public int CompanyId{get;set;}
            public string Name{get;set;}
            public int Phone{get;set;}
            public string Email{get;set;}
            public string Address{get;set;}

            public ResultQueryDao()
            {
                
            }
           

            }
        
    }
}

