using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QL_PHONGGYM.AdminPortal.Data;
using QL_PHONGGYM.AdminPortal.Models;

namespace QL_PHONGGYM.AdminPortal.Controllers
{
    //[Authorize(Roles = "ManagerRole")] // Chỉ Quản lý mới được xem tiền nong
    public class BaoCaoController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: BaoCao (Trang hiển thị biểu đồ)
        public ActionResult Index()
        {
            return View();
        }

        // GET: BaoCao/GetDoanhThuData (API trả về JSON để vẽ biểu đồ)
        public JsonResult GetDoanhThuData()
        {
            // Lấy dữ liệu hóa đơn "Đã thanh toán"
            var data = db.HoaDons
                .Where(h => h.TrangThai == "Đã thanh toán")
                .GroupBy(h => new { h.NgayLap.Year, h.NgayLap.Month }) // Nhóm theo Năm và Tháng
                .Select(g => new
                {
                    Thang = g.Key.Month,
                    Nam = g.Key.Year,
                    TongTien = g.Sum(x => x.ThanhTien) // Tính tổng thành tiền
                })
                .ToList()
                .OrderBy(x => x.Nam).ThenBy(x => x.Thang) // Sắp xếp từ cũ đến mới
                .Select(x => new
                {
                    Label = "Tháng " + x.Thang + "/" + x.Nam,
                    Value = x.TongTien
                });

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}