using Models;
using System.Collections.Generic;

namespace Repository
{
    public interface IPhoneBookRepository
    {
        bool DeleteContact(int id);
        Contact GetContactById(int id);
        List<Contact> GetContacts();
        bool SaveContact(Contact model);
    }
}
