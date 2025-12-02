using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QL_PHONGGYM.AdminPortal.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            string loginName = User.Identity.Name;

            // Gửi tên này sang cho View để chào mừng
            ViewBag.LoginName = loginName;

            // Dữ liệu thống kê (Tạm thời gán cứng)
            // Sau này, bạn sẽ gọi CSDL ở đây để lấy dữ liệu thật
            ViewBag.CheckInHomNay = 48;
            ViewBag.HoaDonHomNay = 12;
            ViewBag.DangKyMoi = 5;
            ViewBag.TapThu = 3;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}