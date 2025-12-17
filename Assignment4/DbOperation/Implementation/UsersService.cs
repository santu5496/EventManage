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
                var a= db.Users.ToList();
                return db.Users.ToList();
            }
        }
        public Users? ValidateUser(string UserName, string password)
        {
            try
            {
                using (var db = new EventContext(_context))
                {
                    // Retrieve the user by username
                    var user = db.Users.FirstOrDefault(u => u.username == UserName);

                    if (user == null)
                    {
                        // User not found
                        return null;
                    }

                    // Check if the password matches (assumes password is stored as plain text, you should hash it in production)
                    if (user.passwordHash == password)
                    {
                        // Password matches, return the user
                        return user;
                    }

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
                var existingUser = db.Users.FirstOrDefault(u => u.userId == user.userId);
                if (existingUser == null)
                {
                    return false;
                }

                // Update only the fields that are provided (not null or empty)
                if (!string.IsNullOrEmpty(user.fullName))
                    existingUser.fullName = user.fullName;
                if (!string.IsNullOrEmpty(user.email))
                    existingUser.email = user.email;
                if (!string.IsNullOrEmpty(user.phoneNumber))
                    existingUser.phoneNumber = user.phoneNumber;
                if (!string.IsNullOrEmpty(user.userRole))
                    existingUser.userRole = user.userRole;
                if (!string.IsNullOrEmpty(user.passwordHash))
                    existingUser.passwordHash = user.passwordHash;
                if (!string.IsNullOrEmpty(user.username))
                    existingUser.username = user.username;

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
