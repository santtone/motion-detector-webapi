using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using MotionDetectorWebApi.Models;

namespace MotionDetectorWebApi.Services
{
    public class StreamTokenService : IStreamTokenService
    {
        private List<StreamToken> _tokens;

        public StreamTokenService()
        {
            _tokens = new List<StreamToken>();
        }

        public bool IsValidToken(string token)
        {
            RemoveExpiredTokens();
            var exists = _tokens.Find(t => t.Token == token);
            return exists != null;
        }

        public StreamToken GetToken()
        {
            RemoveExpiredTokens();
            var token = new StreamToken()
            {
                Token = Guid.NewGuid().ToString(),
                ExpiresOn = DateTime.Now.AddHours(1)
            };
            _tokens.Add(token);
            return token;
        }

        private void RemoveExpiredTokens()
        {
            // TODO: Run on background as scheduled task?
            _tokens.RemoveAll(t => t.ExpiresOn < DateTime.Now);
        }
    }
}