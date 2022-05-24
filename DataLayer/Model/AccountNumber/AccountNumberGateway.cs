namespace DataLayer.Model.AccountNumber
{
    public class AccountNumberGateway
    {
        public int Id { get; set; }
        public string? AcountNumberTitle { get; set; }
        public string? BankName { get; set; }
        public string? IbanNumber { get; set; }
        public string? AccountNumber { get; set; }
        public string? AccountOwnerFullName { get; set; }
        public string? GatewayTitle { get; set; }
        public string? GatewayURL { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
