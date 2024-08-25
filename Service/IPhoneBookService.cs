using Models;
using System.Collections.Generic;

namespace Service
{
    public interface IPhoneBookService
    {
        bool DeleteContact(int id);
        Contact GetContactById(int id);
        List<Contact> GetContacts();
        bool SaveContact(Contact model);
    }
}
