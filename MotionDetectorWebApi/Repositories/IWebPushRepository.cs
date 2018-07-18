using MotionDetectorWebApi.Models;
using System.Threading.Tasks;

namespace MotionDetectorWebApi.Repositories
{
    public interface IWebPushRepository
    {
        Task<UserPushSubscription> Find(int userId);
        Task<UserPushSubscription> Create(UserPushSubscription subscription);
        Task<UserPushSubscription> Update(UserPushSubscription subscription);
    }
}
