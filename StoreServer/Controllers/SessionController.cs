using Microsoft.AspNetCore.Mvc;
using StoreServer.DatabaseModels;
using StoreServer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreServer.Controllers
{
    public class SessionController : Controller
    {
        private readonly SessionsDbService _sessionsDbService;
        private readonly UsersDbService _usersDbService;
        public SessionController(SessionsDbService sessionsDbService, UsersDbService usersDbService)
        {
            _sessionsDbService = sessionsDbService;
            _usersDbService = usersDbService;
        }

        [HttpPost]
        public ActionResult<SessionNetworkObject> Get([FromHeader] string username, [FromHeader]string password)
        {
            var dbUser = _usersDbService.Get(username);
            if (dbUser == null) return BadRequest();
            // TODO szyfrowanie hasla
            if (dbUser.Password != password) return BadRequest();
            var sessionToken = GenerateRandomString(32);
            var accessKey = GenerateRandomString(28);
            var expirationDate = DateTime.UtcNow.AddHours(1);
            SESSION session = new SESSION()
            {
                SessionToken = sessionToken,
                AccessKey = accessKey,
                User = dbUser,
                ExpirationDate = expirationDate
            };
            _sessionsDbService.Insert(session);
            SessionNetworkObject sessionObjectToSend = new SessionNetworkObject()
            {
                SessionToken = sessionToken,
                AccessKey = accessKey,
                ExpirationDate = expirationDate
            };
            return Ok(sessionObjectToSend);
        }
        [HttpDelete]
        public ActionResult Delete([FromHeader]string username, [FromHeader]string sessionToken)
        {
            var sessionDbObj = _sessionsDbService.Get(username);
            if(sessionDbObj.SessionToken != sessionToken) return BadRequest();
            _sessionsDbService.Delete(sessionDbObj.Id);
            return Ok();
        }
        private static string GenerateRandomString(int size)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            string randomString = new string(Enumerable.Repeat(chars, size)
              .Select(s => s[random.Next(s.Length)]).ToArray());
            return randomString;
        }
        public class SessionNetworkObject
        {
            public string SessionToken { get; set; }
            public string AccessKey { get; set; }
            public DateTime ExpirationDate { get; set; }
        }
    }
}
