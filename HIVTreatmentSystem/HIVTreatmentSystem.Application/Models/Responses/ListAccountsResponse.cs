namespace HIVTreatmentSystem.Application.Models.Responses
{
    public class ListAccountsResponse
    {
        public IEnumerable<AccountResponse> Accounts { get; set; } = new List<AccountResponse>();
        public int TotalCount { get; set; }
    }
}
