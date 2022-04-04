using DataLayer.ViewModels.Authenticate;
using PaymentGateway.Common;

namespace PaymentGateway.Provider.Interface
{
    public interface IAuthenticateService
    {
        Task<Result> LoginUserAsync(LoginViewModel model);
        Result LogOutUserAsync();
        Task<Result> RegisterUserAsync(RegisterViewModel model);
    }
}