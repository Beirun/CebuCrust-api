using CebuCrust_api.ServiceModels;

namespace CebuCrust_api.Interfaces
{
    public interface IResetService
    {
        Task<bool> ResetRequestAsync(string email);

        Task<bool> ResetVerifyAsync(string resetCode);

        Task<bool> ResetPasswordAsync(string resetCode, string newPassword, string confirmPassword);
    }
}
