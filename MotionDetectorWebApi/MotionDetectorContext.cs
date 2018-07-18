using Microsoft.EntityFrameworkCore;
using MotionDetectorWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MotionDetectorWebApi
{
    public class MotionDetectorContext : DbContext
    {
        public MotionDetectorContext(DbContextOptions<MotionDetectorContext> options) : base(options) { }

        public DbSet<UserPushSubscription> UserPushSubscriptions { get; set; }
        public DbSet<User> Users { get; set; }


    }
}
