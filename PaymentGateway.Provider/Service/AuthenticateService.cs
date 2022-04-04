using DataLayer.Model.Authenticate;
using DataLayer.ViewModels.Authenticate;
using Microsoft.AspNetCore.Identity;
using PaymentGateway.Common;
using PaymentGateway.Provider.Interface;

namespace PaymentGateway.Provider.Service
{
    public class AuthenticateService : IAuthenticateService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        public AuthenticateService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<Result> RegisterUserAsync(RegisterViewModel model)
        {
            var user = new RegisterUser
            {
                UserName = model.Mobile,
                FullName = model.FullName,
                Password = model.Password,
                RepeatPassword = model.RepeatPassword,
                NationalId = model.NationalId,
                IntroducedCode = model.IntroducedCode,
            };

            if (model.Password == model.RepeatPassword)
            {
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return Result.Success(result);
                }
                return Result.Failed("این کاربر قبلا ثبت نام کرده .");
            }
            return Result.Failed("خطا در ثبت نام لطفا مجددا تلاش کنید .");
        }

        public async Task<Result> LoginUserAsync(LoginViewModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Mobile, model.Password, false, true);
            if (result.Succeeded)
                return Result.Success(result);
            return Result.Failed("خطا در ورود لطفا اطلاعات را با دقت وارد کنید .");
        }

        public Result LogOutUserAsync()
        {
            var result = _signInManager.SignOutAsync();
            return Result.Success(result);
        }
    }
}
