using System;
using System.Collections.Generic;
using Npgsql;
using Models;

namespace Repository
{
    public class PhoneBookRepository
    {
        private readonly string _connectionString = "Host=localhost;Database=postgres;Username=postgres;Password=123";

        public List<Contact> GetContacts()
        {
            var contacts = new List<Contact>();

            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();

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

            return contacts;
        }

        public bool SaveContact(Contact contact)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();

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
            }

            return true;
        }

        public bool DeleteContact(int id)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand("DELETE FROM public.\"Contact\" WHERE id = @id", conn))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    cmd.ExecuteNonQuery();
                }
            }

            return true;
        }

        public Contact GetContactById(int id)
        {
            Contact contact = null;

            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();

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
            }

            return contact;
        }
    }
}
