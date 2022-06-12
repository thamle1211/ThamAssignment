using Microsoft.Extensions.Configuration;
using SignUpAUser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignUpAUser.Services
{
    public class UserService : IUserService
    {
        private readonly IUserDAO _userDAO;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        public UserService(IUserDAO userDAO, IEmailService emailService, IConfiguration configuration)
        {
            _userDAO = userDAO;
            _emailService = emailService;
            _configuration = configuration;
        }
        public async Task<bool> RegisterUser(UserModel userModel)
        {
            userModel.State = "new";
            userModel.ActivateToken = Guid.NewGuid();
            if (_userDAO.RegisterUser(userModel))
            {
                return await SendEmail(userModel);
            }
            
            return false;
        }

        private async Task<bool> SendEmail(UserModel userModel)
        {
            var activateLink = BuildActivateLink(userModel.ActivateToken, userModel.EmailAddress);
            await _emailService.SendEmail(activateLink, userModel.EmailAddress);
            return true;
        }
        private string BuildActivateLink(Guid activateToken, string email)
        {
            var rootLink = _configuration.GetSection("ActivateUrl").Value;
            var link = $"{rootLink}?token={activateToken}&email={email}";
            return link;
        }

        public int VerifiyUser(Guid token, string email)
        {
            return _userDAO.VerifiyUser(token, email);
        }
        public bool UpdateStateUser(int userId)
        {
            return _userDAO.UpdateStateUser(userId);
        }
        public bool CheckIfExist(string email)
        {
            return _userDAO.CheckIfExist(email);
        }
        public List<UserModel> GetListUser()
        {
            return _userDAO.GetListUser();
        }
    }
}   
