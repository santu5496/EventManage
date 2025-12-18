using DbOperation.Interface;
using DbOperation.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace EventManagement.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public new IActionResult User()
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("login", "Home");
            }
            return View();
        }

        [HttpPost]
        public IActionResult AddOrUpdateUser(Users user)
        {
            if (user.userId != 0)
            {
                var result = _userService.UpdateUser(user);
                if (!result)
                {
                    return Json(new { success = false, message = "Failed to update user." });
                }
                return Json(new { success = true, message = "User updated successfully." });
            }
            else
            {
                var result = _userService.AddUser(user);
                if (!result)
                {
                    return Json(new { success = false, message = "Failed to add user." });
                }
                return Json(new { success = true, message = "User added successfully." });
            }
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            try
            {
                var result = _userService.DeleteUser(id);
                if (!result)
                {
                    return Json(new { success = false, message = "Failed to delete user. User may have associated bookings or events." });
                }
                return Json(new { success = true, message = "User deleted successfully." });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Delete error: {ex.Message}");
                return Json(new { success = false, message = "Cannot delete user. User may have associated bookings or events." });
            }
        }

        public IActionResult GetAllUsers()
        {
            var result = _userService.GetAllUsers();
            if (result == null || result.Count == 0)
            {
                return Json(new { success = false, message = "No users found." });
            }
            return Json(result);
        }
      
    }
}
