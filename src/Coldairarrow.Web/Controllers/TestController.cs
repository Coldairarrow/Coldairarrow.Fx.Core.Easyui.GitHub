using Coldairarrow.Util;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Text;

namespace Coldairarrow.Web
{
    [IgnoreLogin]
    public class TestController : BaseMvcController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RequestDemo()
        {
            return View();
        }

        public ActionResult GetData()
        {
            return Success("获取成功！");
        }

        public ActionResult SaveFile()
        {
            string rootPath = AutofacHelper.GetService<IHostingEnvironment>().WebRootPath;

            string path = Path.Combine(rootPath, "Upload");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            string file = Path.Combine(path, "text.txt");
            System.IO.File.WriteAllText(file, "测试", Encoding.UTF8);

            return Success();
        }

        public ActionResult Test()
        {
            string imgBase64Url = System.IO.File.ReadAllText(Path.Combine(GlobalSwitch.WebRootPath, "img.txt"));
            Console.WriteLine(imgBase64Url);
            var img = ImgHelper.GetImgFromBase64Url(imgBase64Url);
            img.Save(Path.Combine(GlobalSwitch.WebRootPath, "Upload", "1.jpg"));

            return Success(GlobalSwitch.WebRootPath);
        }
    }
}