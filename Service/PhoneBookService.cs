using Models;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Service
{
    public class PhoneBookService
    {
        private readonly PhoneBookRepository _repo;

        public PhoneBookService()
        {
            _repo = new PhoneBookRepository();
        }

        public bool DeleteContact(int id)
        {
            try
            {
                return _repo.DeleteContact(id);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Contact GetContactById(int id)
        {
            return _repo.GetContactById(id);
        }

        public List<Contact> GetContacts()
        {
            return _repo.GetContacts();
        }

        public bool SaveContact(Contact model)
        {
            if (ValidateModel(model))
            {
                return _repo.SaveContact(model);
            }
            return false;
        }

        public bool ValidateModel(Contact model)
        {
            //var phoneRegex = new Regex(@"^09\d{9}$");
            var nameRegex = new Regex(@"^[a-zA-Z\s]+$");

            if (!nameRegex.IsMatch(model.Firstname))
            {
                return false;
            }

            if (!nameRegex.IsMatch(model.Lastname))
            {
                return false;
            }

            //if (!phoneRegex.IsMatch(model.PhoneNumber.ToString()))
            //{
            //    return false;
            //}

            return true;
        }
    }
}
