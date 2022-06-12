using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignUpAUser.Services
{
    public interface IEmailService
    {
        Task<bool> SendEmail(string activateLink, string email);
    }
}
