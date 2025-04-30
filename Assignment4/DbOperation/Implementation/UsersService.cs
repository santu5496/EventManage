using DbOperation.Interface;
using DbOperation.Models;
using iText.Layout.Tagging;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DbOperation.Implementation
{
    public class UsersService : IUserService
    {
        private readonly DbContextOptions<EventContext> _context;

        public UsersService(string context)
        {
            _context = new DbContextOptionsBuilder<EventContext>().UseSqlServer(context).Options;
        }


        public bool AddUser(Users user)
        {
            try
            {
                using (var db = new EventContext(_context))
                {
                    db.Users.Add(user);
                    return db.SaveChanges() > 0;  // This returns `true` if rows are affected, `false` if no rows are affected
                }
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                Console.WriteLine($"Error: {ex.Message}");
                return false;  // Return false if any error occurs
            }
        }





        public List<Users> GetAllUsers()
        {
            using (var db = new EventContext(_context))
            {
                return db.Users.ToList();
            }
        }
        public Users? ValidateUser(string phoneNumber, string password)
        {
            try
            {
                using (var db = new EventContext(_context))
                {
                    // Retrieve the user by phone number
                    var user = db.Users.FirstOrDefault(u => u.phoneNumber == phoneNumber);

                    if (user == null)
                    {
                        // User not found
                        return null;
                    }

                    // Password matches, return the user
                        return user;
                    

                    // Password does not match
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error validating user: {ex.Message}");
                return null;
            }
        }


        public bool UpdateUser(Users user)
        {
            using (var db = new EventContext(_context))
            {
                db.Users.Update(user);
                return db.SaveChanges() > 0;
            }
        }

        public bool DeleteUser(int userId)
        {
            using (var db = new EventContext(_context))
            {
                var user = db.Users.FirstOrDefault(u => u.userId == userId);
                if (user != null)
                {
                    db.Users.Remove(user);
                    return db.SaveChanges() > 0;
                }
                return false;
            }
        }
    }
    }
