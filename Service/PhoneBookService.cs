using Models;
using NLog;
using Repository;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Service
{
    public class PhoneBookService : IPhoneBookService
    {
        private readonly IPhoneBookRepository _repo;
        private readonly ILogger _logger;

        public PhoneBookService(IPhoneBookRepository repo, ILogger logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public bool DeleteContact(int id)
        {
            try
            {
                return _repo.DeleteContact(id);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error occurred while deleting contact with ID {Id}", id);
                return false;
            }
        }

        public Contact GetContactById(int id)
        {
            try
            {
                return _repo.GetContactById(id);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error occurred while retrieving contact with ID {Id}", id);
                return null;
            }
        }

        public List<Contact> GetContacts()
        {
            try
            {
                return _repo.GetContacts();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error occurred while retrieving contacts.");
                return new List<Contact>();
            }
        }

        public bool SaveContact(Contact model)
        {
            if (ValidateModel(model))
            {
                try
                {
                    return _repo.SaveContact(model);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "An error occurred while saving contact.");
                    return false;
                }
            }
            _logger.Warn("Validation failed for contact: {@Contact}", model);
            return false;
        }

        public bool ValidateModel(Contact model)
        {
            var nameRegex = new Regex(@"^[a-zA-Z\s]+$");

            if (!nameRegex.IsMatch(model.Firstname))
            {
                _logger.Warn("Validation failed: First name is invalid for contact: {@Contact}", model);
                return false;
            }

            if (!nameRegex.IsMatch(model.Lastname))
            {
                _logger.Warn("Validation failed: Last name is invalid for contact: {@Contact}", model);
                return false;
            }

            return true;
        }
    }
}
