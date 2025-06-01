using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIVTreatmentSystem.Domain.Enums;

namespace HIVTreatmentSystem.Application.Models.Requests
{
    public class ListAccountsRequest
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public AccountStatus? AccountStatus { get; set; }
        public int? RoleId { get; set; }

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;

        public string? SortBy { get; set; }
        public bool SortDescending { get; set; } = false;
    }
}
