using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIVTreatmentSystem.Application.Dtos;

namespace HIVTreatmentSystem.Application.Models.Responses
{
    public class AccountResponse
    {
        public AccountDto Account { get; set; } = default!;
    }
}
