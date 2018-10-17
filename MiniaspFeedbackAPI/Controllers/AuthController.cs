using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MiniaspFeedbackAPI.Models;
using Newtonsoft.Json;

namespace MiniaspFeedbackAPI.Controllers
{
    public class AuthController : Controller
    {
        private readonly MyContext _context;

        public AuthController(MyContext context)
        {
            _context = context;
        }

        // GET: api/User
        [EnableCors("MyPolicy")]
        [HttpGet]
        [Route("api/Auth/IsLogined")]
        public IActionResult IsLogined()
        {
            var token = Request.Headers["UToken"].First();
            User user = new User();
            IsLoginedOutput output = new IsLoginedOutput();

            if (token == null)
            {
                output.msg = "未接收到token資料!";
                return Ok(output);
            }

            user = (
                from u in _context.User
                from t in _context.Utoken
                where t.UtokenId == token && t.UTokenTimeOut > DateTime.Now
                select u
                       ).FirstOrDefault();

            if (user == null)
            {
                output.msg = "連線逾時，請重新登入!";
                return Ok(output);
            }

            // 設定token逾時分鐘數
            int TimeoutMinute = 20;

            // 計算逾時時間
            DateTime UtokenTime = DateTime.Now.AddMinutes(TimeoutMinute);

            // 更新token的逾時時間
            var UpdateToken = _context.Utoken.FirstOrDefault(x => x.UtokenId == token);
            UpdateToken.UTokenTimeOut = UtokenTime;
            _context.SaveChanges();

            return Ok(new User() { UserId = user.UserId, Name = user.Name });
        }

        //// ' GET: api/User
        //// Public Function GetValues() As IEnumerable(Of String)
        //// Return New String() {"value1", "value2"}
        //// End Function

        //// ' GET: api/User/5
        //// Public Function GetValue(ByVal id As Integer) As String
        //// Return "value"
        //// End Function

        // POST: api/User/Login
        [EnableCors("MyPolicy")]
        [HttpPost]
        [Route("api/Auth/Login")]
        public IActionResult PostLogin([FromBody()] Login loginInfo)
        {
            LoginOutput output = new LoginOutput();

            if (!ModelState.IsValid)
            {
                output.result = "驗證失敗!";
                return Ok(output);
            }

            var user = _context.User.FirstOrDefault(x => x.UserId == loginInfo.userId && x.Password == loginInfo.password);

            if (user == null)
            {
                output.result = "帳號或密碼輸入錯誤!";
                return Ok(output);
            }
            
            // 取得ClientIP
            string ClientIP = HttpContext.Connection.RemoteIpAddress.ToString();

            // 產生Token
            var UToken = Guid.NewGuid().ToString().ToUpper();

            // 設定token逾時分鐘數
            int TimeoutMinute = 20;

            // 計算逾時時間
            DateTime UtokenTime = DateTime.Now.AddMinutes(TimeoutMinute);

            // 刪除所有逾時Token資料
            var timeoutToken = _context.Utoken.Where(x => x.UTokenTimeOut < DateTime.Now && x.UserId == user.UserId).ToList();
            if (timeoutToken.Count > 0)
            {
                _context.Utoken.RemoveRange(timeoutToken);
            }

            // 新增本次登入token資料
            Utoken newToken = new Utoken()
            {
                UserId = user.UserId,
                User = user,
                IP = ClientIP,
                UtokenId = UToken,
                LastInTime = DateTime.Now,
                UTokenTimeOut = UtokenTime
            };
            _context.Utoken.Add(newToken);
            _context.SaveChanges();

            output = new LoginOutput
            {
                result = "登入成功!",
                uToken = newToken.UtokenId
            };

            return Ok(output);
        }


    }
}