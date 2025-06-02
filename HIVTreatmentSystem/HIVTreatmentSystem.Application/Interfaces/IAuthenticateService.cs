using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIVTreatmentSystem.Application.Interfaces
{
    public interface IAuthenticateService
    {
        Task<bool> ChangePassword(string oldPassword, string newPassword, int id);
    }
}
