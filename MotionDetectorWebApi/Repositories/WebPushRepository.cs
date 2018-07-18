using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MotionDetectorWebApi.Models;

namespace MotionDetectorWebApi.Repositories
{
    public class WebPushRepository : IWebPushRepository
    {
        private readonly MotionDetectorContext _context;

        public WebPushRepository(MotionDetectorContext context)
        {
            _context = context;
        }

        public async Task<UserPushSubscription> Create(UserPushSubscription subscription)
        {
            await _context.UserPushSubscriptions.AddAsync(subscription);
            await _context.SaveChangesAsync();
            return null;
        }

        public async Task<UserPushSubscription> Update(UserPushSubscription subscription)
        {
            _context.UserPushSubscriptions.Update(subscription);
            await _context.SaveChangesAsync();
            return subscription;
        }

        public Task<UserPushSubscription> Find(int userId)
        {
            return _context.UserPushSubscriptions.FirstOrDefaultAsync(s => s.UserId == userId);
        }
    }
}
