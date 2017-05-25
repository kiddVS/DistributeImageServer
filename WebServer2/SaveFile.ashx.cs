using Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WebServer2
{
    /// <summary>
    /// SaveFile 的摘要说明
    /// </summary>
    public class SaveFile : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {

            context.Response.ContentType = "text/plain";
            int serverID = int.Parse(context.Request["serverID"]);
            string ext = context.Request["ext"];
            // HttpPostedFile file= context.Request.Files[0];
            Stream stream = context.Request.InputStream;
            string dir = context.Request.MapPath(@"/Image/" + DateTime.Now.ToString("yyyyMMdd"));
            Directory.CreateDirectory(dir);
            string fileName = @"/Image/" + DateTime.Now.ToString("yyyyMMdd") +"/"+ Guid.NewGuid() + ext;
            using (FileStream fs = new FileStream(context.Server.MapPath(fileName), FileMode.Create))
            {
                stream.CopyTo(fs);
            }
            MyImageServerEntities db = new MyImageServerEntities();
            ImageInfo entity = new ImageInfo() { ImageName = fileName, ImageServerId = serverID };
            db.ImageInfo.Add(entity);
            db.SaveChanges();
            context.Response.Write("OK");
            // context.Response.Write("Hello World");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}