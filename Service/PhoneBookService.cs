using Models;

using Repository;

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;


namespace Service
{
    public class PhoneBookService
    {
        PhoneBookRespository _repo;
        public PhoneBookService()
        {
            _repo = new PhoneBookRespository();
        }

        public bool DeleteContact(string Id)
        {
            try
            {
                var contacts = _repo.GetContacts();
                var contactForDelete = contacts.FirstOrDefault(x => x.Id.ToString() == Id);
                contacts.Remove(contactForDelete);
                _repo.SaveContact(contacts);
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        public Contact GetContactById(string id)
        {

            var contacts = GetContacts();
            var contact = contacts.FirstOrDefault(x => x.Id.ToString() == id.ToString());
            return contact;

            //if (contact == null)
            //{
            //    return null;
            //}
            //return contact;
        }

        public List<Contact> GetContacts()
        {
            return _repo.GetContacts();
        }

        public bool SaveContact(Contact model)
        {
            var contacts = GetContacts();
            if (ValidateModel(model))
            {

                if (model.Id == Guid.Empty)
                {
                    model.Id = Guid.NewGuid();
                    contacts.Add(model);
                }
                else
                {
                    var contactForEdit = contacts.FirstOrDefault(c => c.Id == model.Id);
                    if (contactForEdit != null)
                    {
                        contacts.Remove(contactForEdit);
                        contacts.Add(model);
                    }

                }
            }



            return _repo.SaveContact(contacts);
        }
        public bool ValidateModel(Contact model)
        {
            var phoneRegex = new Regex(@"^09\d{9}$");
            var nameRegex = new Regex(@"^[\u0600-\u06FF\s]+$");

            if (!nameRegex.IsMatch(model.Firstname))
            {
                return false;
            }

            if (!nameRegex.IsMatch(model.Lastname))
            {
                return false;
            }

            if (!phoneRegex.IsMatch(model.PhoneNumber))
            {
                return false;
            }

            return true;
        }
    }
}

