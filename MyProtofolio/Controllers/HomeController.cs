using EmailService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyProtofolio.Controllers
{
    public class HomeController : Controller
    {
        IEmailSender _emailSender;
        public HomeController(IEmailSender emailSender)
        {
          
            _emailSender = emailSender;
           
        }
       
        public async Task<IActionResult> IndexAsync(string name, string email, string message, IFormCollection form, IFormFileCollection files)
        {
            try 
            {
                if (name != null)
                {
                    var userEmail = email;

                    var messages = new Message(new string[] {"Kassem@kassemvoice.com"}, "Email From Customer " + email, "This is the content from our async email. i am happy", files);
                    //var filePaths = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Uploads", ImageName);
                    //var files = Request.Form.Files.Any() ? Request.Form.Files : new FormFileCollection();
                    await _emailSender.SendEmailAsync(messages, message);
                    return View();
                }
                else
                {
                    return View();
                }

            }
            catch (Exception ex) 
            {
                ViewBag.ex = ex;
                return View();

            }
            
           
        }


        public async Task<IActionResult> ArabicIndexAsync(string name, string email, string message, IFormCollection form, IFormFileCollection files)
        {
            if (name != null)
            {
                var userEmail = email;

                var messages = new Message(new string[] { "Kassem@kassemvoice.com" }, "Email From Customer " + email, "This is the content from our async email. i am happy", files);
                //var filePaths = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Uploads", ImageName);
                //var files = Request.Form.Files.Any() ? Request.Form.Files : new FormFileCollection();
                await _emailSender.SendEmailAsync(messages, message);
                return View();
            }
            else
            {
                return View();
            }

        }
        public IActionResult Blog() 
        {
            return View();
        }


     
    }
}
