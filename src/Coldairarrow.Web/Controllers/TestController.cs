using Microsoft.AspNetCore.Mvc;

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
    }
}