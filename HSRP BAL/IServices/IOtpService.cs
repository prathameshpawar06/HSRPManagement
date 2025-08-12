using HSRP_DAL.Models;

namespace HSRP_BAL.IServices
{
    public interface IOtpService
    {
        Task<SendOtpResponse> SendOtpAsync(SendOtpRequest req);
        Task<VerifyOtpResponse> VerifyOtpAsync(VerifyOtpRequest req);
    }
}
