using SignUpAUser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignUpAUser.Services
{
    public interface IUserService
    {
        Task<bool> RegisterUser(UserModel userModel);
        int VerifiyUser(Guid token, string email);
        bool UpdateStateUser(int userId);
        bool CheckIfExist(string email);
        List<UserModel> GetListUser();
    }
}
