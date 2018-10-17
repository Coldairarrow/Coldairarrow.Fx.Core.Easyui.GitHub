using Microsoft.AspNetCore.Mvc;

namespace Coldairarrow.Web
{
    [IgnoreLogin]
    public class TestController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RequestDemo()
        {
            return View();
        }
    }
}