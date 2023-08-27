using OTTMyPlatform.Models;
using OTTMyPlatform.Models.Responce;

namespace OTTMyPlatform.Repository.Interface
{
    public interface IAuthRepository
    {
        Task<List<UserLoginDetail>> GetAllUser();
        Task<UserLoginResponce> VerifyUserGenerateTocken(UserLoginResponce responce, UserLoginDetail loginUser);
        Task<UserLoginResponce>  Register(UserLoginDetail myRegister);

    }
}
