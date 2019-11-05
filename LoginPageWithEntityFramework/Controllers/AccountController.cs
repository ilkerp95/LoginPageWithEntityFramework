using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LoginPageWithEntityFramework.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
namespace LoginPageWithEntityFramework.Controllers
{
    [Route("account")]
    public class AccountController : Controller
    {
        private DataContext _db = new DataContext();


        [Route("")]
        [Route("index")]
        [Route("~/")]   
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        [Route("signup")]
        public IActionResult SignUp()
        {
            return View("SignUp",new Account());
        }

        [HttpPost]
        [Route("signup")]
        public IActionResult SignUp(Account account)
        {
            account.Password = BCrypt.Net.BCrypt.HashPassword(account.Password);
            _db.Account.Add(account);    
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login(string username,string password)
        {
            var account = chechAccount(username, password);
            if (account==null)
            {
                return View("Index");
            }
            else
            {
                HttpContext.Session.SetString("username", username);
                return RedirectToAction("Index", "Home");
            }
        }
        private Account chechAccount(string username,string password)
        {
            var account = _db.Account.SingleOrDefault(a => a.Username.Equals(username));
            if (account!=null)
            {
                if(BCrypt.Net.BCrypt.Verify(password,account.Password))
                {
                    return account;
                }

            }
            return null;
        }

        [HttpGet]
        [Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("username");
            return RedirectToAction("Index");
        }
    }
}