using Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ImageUpload.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult ImageUpload()
        {
            HttpPostedFileBase files = Request.Files["file"];
            if (files != null)
            {
                string fileName = files.FileName;
                string extensionName = Path.GetExtension(fileName);
                if (extensionName == ".jpg" || extensionName == ".png" || extensionName == "gif")
                {
                    Stream stream = files.InputStream;
                    byte[] buffer = new byte[stream.Length];
                    stream.Read(buffer, 0, buffer.Length);
                    stream.Seek(0, SeekOrigin.Begin);
                    MyImageServerEntities db = new MyImageServerEntities();
                    var imageServersEntity = db.ImageServerInfo.Where(u => u.FlgUsable == true).ToList();
                    Random random = new Random();
                    int serverNum = random.Next() % imageServersEntity.Count() + 1;
                    var uploadServer = imageServersEntity.Where(u => u.ServerId == serverNum).FirstOrDefault();
                    WebClient client = new WebClient();
                    string url = @"http://" + uploadServer.ServerUrl + @"/SaveFile.ashx?serverID=" + serverNum + "&ext=" + extensionName;
                    // WebRequest.Create(url);
                   // client.UploadData();
                    client.UploadData(url, buffer);
                    return Content("OK");
                }
                return Content("文件不是图片");
            }
            else
            {
                return Content("选择文件");
            }
        }

        public ActionResult Index()
        {
            return View();
        }




    }
}