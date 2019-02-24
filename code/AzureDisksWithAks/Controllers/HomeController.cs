using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AzureDisksWithAks.Models;
using System;
using System.IO;
using System.Text;

namespace AzureDisksWithAks.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create(FileCreateViewModel model)
        {
            return View(model);
        }

        [HttpPost]
        public IActionResult CreateFile()
        {
            var fileName = $"File_{DateTime.Now.Ticks}.txt";
            var fileContent = $"Current time is {DateTime.Now.ToLongDateString()}";
            var filePath = $"/files/{fileName}";

            using (var fs = System.IO.File.Create(filePath))
            {
                var content = Encoding.UTF8.GetBytes(fileContent);
                fs.Write(content, 0, content.Length);
            }

            var result = new FileCreateViewModel
            {
                FileName = fileName,
                IsSuccess = true
            };

            return RedirectToAction("Create", "Home", result);
        }

        public IActionResult ViewFiles()
        {
            var directory = "/files";
            var dirInfo = new DirectoryInfo(directory);
            var files = dirInfo.GetFiles();
            return View(files);
            
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
