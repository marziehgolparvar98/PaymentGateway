using DataLayer.DbContext;
using DataLayer.Model.AccountNumber;
using Microsoft.EntityFrameworkCore;
using PaymentGateway.Common;

namespace DataLayer.Repository
{
    public class AccountNumberGatewayRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public AccountNumberGatewayRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task<Result<AccountNumberGateway>> AddAccountNumberAsync(AccountNumberGateway accountNumber)
        {
            await _applicationDbContext.accountNumberGateways.AddAsync(accountNumber);
            await _applicationDbContext.SaveChangesAsync();
            return Result.Success(accountNumber);
        }
        public async Task<Result<List<AccountNumberGateway>>> GetAccountNumberGatewayAsync()
        {
            var result = await _applicationDbContext.accountNumberGateways.OrderByDescending(x => x.CreatedDate).ToListAsync();
            return Result.Success(result);
        }

        public async Task<Result<AccountNumberGateway>> GetAccountNumberGatewayByIdAsync(int id)
        {
            var result = await _applicationDbContext.accountNumberGateways.FirstOrDefaultAsync(x => x.Id == id);
            if (result == null)
                return Result.Failed<AccountNumberGateway>("شماره حسابی با این id یافت نشد.");
            return Result.Success(result);
        }

        public async Task<Result<AccountNumberGateway>> DeleteAccountNumberGatewayByIdAsync(int id)
        {
            var result = _applicationDbContext.accountNumberGateways.FirstOrDefaultAsync(x => x.Id == id);
            if (result != null)
            {
                _applicationDbContext.accountNumberGateways.Remove(result);
            }
        }
    }
}