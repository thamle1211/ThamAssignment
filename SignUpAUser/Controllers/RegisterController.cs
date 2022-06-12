using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SignUpAUser.Models;
using SignUpAUser.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignUpAUser.Controllers
{
    public class RegisterController : Controller
    {
        private readonly IUserService _userService;

        public RegisterController(IUserService userService)
        {
            _userService = userService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ProcessRegister(UserModel userModel)
        {
            var checkIfExist = _userService.CheckIfExist(userModel.EmailAddress);
            if (checkIfExist)
            {
                ModelState.AddModelError(nameof(userModel.EmailAddress), "This email already in use");
                return View("Index");
            }
            
            await _userService.RegisterUser(userModel);
            
            return View("RegisterSucess", userModel);
        }


        public IActionResult VerifiedUser(Guid token, string email)
        {
            var userId = _userService.VerifiyUser(token, email);
            if(userId > 0)
            {
                //update state
                _userService.UpdateStateUser(userId);
                return View("VerifiedUser");
            }
            return View("VerifiedFailed");
        }
    }
}
