using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIVTreatmentSystem.Application.Dtos;

namespace HIVTreatmentSystem.Application.Models.Responses
{
    public class ListAccountsResponse
    {
        public IEnumerable<AccountDto> Accounts { get; set; } = new List<AccountDto>();
        public int TotalCount { get; set; }
    }
}
