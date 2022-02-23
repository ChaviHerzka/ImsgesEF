using EFCoreWebDemo.Web.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using ImagesEF.Data;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace EFCoreWebDemo.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly string _connectionString;
        private readonly IWebHostEnvironment _environment;

        public HomeController(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
            _environment = environment;
        }

        public IActionResult Index()
        {
            var connectionString = _connectionString;
            var repo = new ImagesRepository(connectionString);
            var vm = new HomeViewModel
            {
                Images = repo.GetAll()
            };
            return View(vm);
        }

        public IActionResult Upload()
        { 
            return View();  
        }

        public IActionResult ViewImage(int id)
        {
            var connectionString = _connectionString;
            var repo = new ImagesRepository(connectionString);
            var ids = HttpContext.Session.Get<List<int>>("Ids");
            bool liked = false;
            if (ids != null)
            {
                liked = ids.Contains(id);
            }
            var vm = new ViewImagesViewModel
            {
                Image = repo.GetById(id),
                AlreadyLiked = liked
            };
            return View(vm);
        }
        [HttpPost]
        public IActionResult Upload(Image image, IFormFile imageFile)
        {
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(imageFile.FileName)}";
            string fullPath = Path.Combine(_environment.WebRootPath, "uploads", fileName);
            using (FileStream stream = new FileStream(fullPath, FileMode.CreateNew))
            {
                imageFile.CopyTo(stream);
            }
            image.FileName = fileName;
            image.DateUploaded = DateTime.Now;
            var connectionString = _connectionString;
            var repo = new ImagesRepository(connectionString);
            repo.AddImage(image);
            return Redirect("/");
        }
        public IActionResult CurrentLikes(int id)
        { 
            var connectionString = _connectionString;
            var repo = new ImagesRepository(connectionString);
            return Json(repo.GetLikesOfImages(id));
        }
        [HttpPost]
        public IActionResult AddLike(int id)
        {
            var connectionString = _connectionString;
            var repo = new ImagesRepository(connectionString);
            repo.AddLike(id);
            var session = HttpContext.Session.Get<List<int>>("Ids");
            if (session == null)
            { 
                session = new List<int>();
            }
            session.Add(id);
            HttpContext.Session.Set("Ids", session);
            return Json(id);
        }
    }
    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            string value = session.GetString(key);
            return value == null ? default(T) :
                JsonConvert.DeserializeObject<T>(value);
        }
    }
}


