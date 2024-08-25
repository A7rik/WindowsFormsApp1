using System;
using System.Collections.Generic;
using Npgsql;
using Models;
using NLog;

namespace Repository
{
    public class PhoneBookRepository : IPhoneBookRepository
    {
        private readonly string _connectionString;
        private static ILogger _logger = LogManager.GetCurrentClassLogger();

        public PhoneBookRepository(string connectionString, ILogger logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        public List<Contact> GetContacts()
        {
            var contacts = new List<Contact>();

            try
            {
                using (var conn = new NpgsqlConnection(_connectionString))
                {
                    conn.Open();
                    _logger.Info("Database connection opened for fetching contacts.");

                    using (var cmd = new NpgsqlCommand("SELECT id, \"firstName\", \"lastName\", \"phoneNumber\" FROM public.\"Contact\"", conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            contacts.Add(new Contact
                            {
                                Id = reader.GetInt32(0),
                                Firstname = reader.GetString(1),
                                Lastname = reader.GetString(2),
                                PhoneNumber = reader.GetInt32(3)
                            });
                        }
                    }
                }

                _logger.Info("Successfully retrieved contacts.");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error occurred while fetching contacts.");
            }

            return contacts;
        }

        public bool SaveContact(Contact contact)
        {
            try
            {
                using (var conn = new NpgsqlConnection(_connectionString))
                {
                    conn.Open();
                    _logger.Info("Database connection opened for saving contact.");

                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;

                        if (contact.Id == 0)
                        {
                            cmd.CommandText = "INSERT INTO public.\"Contact\" (\"firstName\", \"lastName\", \"phoneNumber\") VALUES (@firstName, @lastName, @phoneNumber)";
                        }
                        else
                        {
                            cmd.CommandText = "UPDATE public.\"Contact\" SET \"firstName\" = @firstName, \"lastName\" = @lastName, \"phoneNumber\" = @phoneNumber WHERE id = @id";
                            cmd.Parameters.AddWithValue("id", contact.Id);
                        }

                        cmd.Parameters.AddWithValue("firstName", contact.Firstname);
                        cmd.Parameters.AddWithValue("lastName", contact.Lastname);
                        cmd.Parameters.AddWithValue("phoneNumber", contact.PhoneNumber);

                        cmd.ExecuteNonQuery();
                    }

                    _logger.Info("Contact saved successfully.");
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error occurred while saving the contact.");
                return false;
            }
        }

        public bool DeleteContact(int id)
        {
            try
            {
                using (var conn = new NpgsqlConnection(_connectionString))
                {
                    conn.Open();
                    _logger.Info($"Database connection opened for deleting contact with ID: {id}.");

                    using (var cmd = new NpgsqlCommand("DELETE FROM public.\"Contact\" WHERE id = @id", conn))
                    {
                        cmd.Parameters.AddWithValue("id", id);
                        cmd.ExecuteNonQuery();
                    }

                    _logger.Info($"Contact with ID: {id} deleted successfully.");
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"An error occurred while deleting the contact with ID: {id}.");
                return false;
            }
        }

        public Contact GetContactById(int id)
        {
            Contact contact = null;

            try
            {
                using (var conn = new NpgsqlConnection(_connectionString))
                {
                    conn.Open();
                    _logger.Info($"Database connection opened for fetching contact with ID: {id}.");

                    using (var cmd = new NpgsqlCommand("SELECT id, \"firstName\", \"lastName\", \"phoneNumber\" FROM public.\"Contact\" WHERE id = @id", conn))
                    {
                        cmd.Parameters.AddWithValue("id", id);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                contact = new Contact
                                {
                                    Id = reader.GetInt32(0),
                                    Firstname = reader.GetString(1),
                                    Lastname = reader.GetString(2),
                                    PhoneNumber = reader.GetInt32(3)
                                };
                            }
                        }
                    }

                    _logger.Info($"Contact with ID: {id} fetched successfully.");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"An error occurred while retrieving the contact with ID: {id}.");
            }

            return contact;
        }
    }
}
