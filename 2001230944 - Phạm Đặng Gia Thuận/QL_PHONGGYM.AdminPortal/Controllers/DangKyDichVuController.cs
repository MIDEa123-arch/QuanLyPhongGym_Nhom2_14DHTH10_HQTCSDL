using QL_PHONGGYM.AdminPortal.Data;
using QL_PHONGGYM.AdminPortal.Models;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity; // Cần thêm để dùng .Include()

namespace QL_PHONGGYM.AdminPortal.Controllers
{
    // Yêu cầu đăng nhập (Cả Staff và Manager đều được vào)
    [Authorize]
    public class DangKyDichVuController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //
        // GET: /DangKyDichVu/Index
        // Hàm này để "Chuẩn bị" 3 cái Form
        //
        public ActionResult Index()
        {
            // 1. Chuẩn bị danh sách Khách Hàng (cho cả 3 form)
            ViewBag.MaKH = new SelectList(db.KhachHangs, "MaKH", "TenKH");

            // 2. Chuẩn bị danh sách Gói Tập (cho form 1)
            ViewBag.MaGoiTap = new SelectList(db.GoiTaps, "MaGoiTap", "TenGoi");

            // =======================================================
            // == ĐÃ SỬA LẠI DROPDOWN LỚP HỌC (THEO YÊU CẦU CỦA BẠN) ==
            // =======================================================

            // 3. Chuẩn bị danh sách Lớp Học (cho form 2)
            // Lấy các lớp CHƯA KẾT THÚC
            var lopHocData = db.LopHocs
                .Include(lh => lh.LichLops.Select(ll => ll.NhanVien)) // Tải kèm lịch và giáo viên
                .Where(lh => lh.NgayKetThuc >= DateTime.Today)
                .ToList() // Lấy về C#
                .Select(lh => new SelectListItem
                {
                    Value = lh.MaLop.ToString(),
                    // Cố gắng tìm tên HLV đầu tiên trong lịch
                    // Kết quả: "Boxing Nhập Môn (HLV: Nguyễn Văn Hùng)"
                    // Hoặc: "Yoga (HLV: Chưa xếp HLV)"
                    Text = $"{lh.TenLop} (HLV: {lh.LichLops.FirstOrDefault()?.NhanVien?.TenNV ?? "Chưa xếp HLV"})"
                });

            // Code cũ:
            // ViewBag.MaLop = new SelectList(db.LopHocs.Where(lh => lh.NgayKetThuc >= DateTime.Today), "MaLop", "TenLop");

            // Code mới:
            ViewBag.MaLop = new SelectList(lopHocData, "Value", "Text");
            // =======================================================


            // 4. Chuẩn bị danh sách TẤT CẢ Nhân Viên (cho form 3)
            var tatCaNhanVien = db.NhanViens
                                .Include(nv => nv.ChucVu)
                                .ToList() // Lấy dữ liệu về
                                .Select(nv => new SelectListItem
                                {
                                    Value = nv.MaNV.ToString(),
                                    // Hiển thị tên kèm chức vụ: "Trần Thị Yến (HLV Lớp)"
                                    Text = $"{nv.TenNV} ({nv.ChucVu?.TenChucVu ?? "Chưa có CV"})"
                                });
            ViewBag.MaNV = new SelectList(tatCaNhanVien, "Value", "Text");

            return View();
        }

        //
        // POST: /DangKyDichVu/DangKyGoiTap
        // (Giữ nguyên)
        //
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DangKyGoiTap(int MaKH, int MaGoiTap, DateTime NgayBatDau)
        {
            try
            {
                // Chuẩn bị tham số cho Stored Procedure
                var pMaKH = new SqlParameter("@MaKH", MaKH);
                var pMaGoiTap = new SqlParameter("@MaGoiTap", MaGoiTap);
                var pNgayBatDau = new SqlParameter("@NgayBatDau", NgayBatDau);

                // Gọi Stored Procedure (SP)
                db.Database.ExecuteSqlCommand("EXEC sp_DangKyGoiTap @MaKH, @MaGoiTap, @NgayBatDau",
                    pMaKH, pMaGoiTap, pNgayBatDau);

                // Gửi thông báo thành công về
                TempData["AlertMessage"] = "Đăng ký Gói Tập thành công!";
                TempData["AlertStatus"] = "success";
            }
            catch (Exception ex)
            {
                // Gửi thông báo lỗi về (Bắt lỗi từ Stored Procedure)
                TempData["AlertMessage"] = "LỖI: " + (ex.InnerException?.InnerException?.Message ?? ex.Message);
                TempData["AlertStatus"] = "error";
            }

            // Quay lại trang Index
            return RedirectToAction("Index");
        }

        //
        // POST: /DangKyDichVu/DangKyLop
        // (Giữ nguyên)
        //
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DangKyLop(int MaKH_Lop, int MaLop) // Đặt tên khác (MaKH_Lop) để tránh trùng lặp
        {
            try
            {
                var pMaKH = new SqlParameter("@MaKH", MaKH_Lop);
                var pMaLop = new SqlParameter("@MaLop", MaLop);

                // Gọi SP
                db.Database.ExecuteSqlCommand("EXEC sp_DangKyLop @MaKH, @MaLop",
                    pMaKH, pMaLop);

                TempData["AlertMessage"] = "Đăng ký Lớp Học thành công!";
                TempData["AlertStatus"] = "success";
            }
            catch (Exception ex)
            {
                TempData["AlertMessage"] = "LỖI: " + (ex.InnerException?.InnerException?.Message ?? ex.Message);
                TempData["AlertStatus"] = "error";
            }

            return RedirectToAction("Index");
        }

        //
        // POST: /DangKyDichVu/DangKyPT
        // (Giữ nguyên)
        //
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DangKyPT(int MaKH_PT, int MaNV, int SoBuoi, decimal GiaMoiBuoi) // Tên tham số phải khớp với Form
        {
            try
            {
                var pMaKH = new SqlParameter("@MaKH", MaKH_PT);
                var pMaNV = new SqlParameter("@MaNV", MaNV);
                var pSoBuoi = new SqlParameter("@SoBuoi", SoBuoi);
                var pGiaMoiBuoi = new SqlParameter("@GiaMoiBuoi", GiaMoiBuoi);

                // Gọi SP (SP này bạn đã sửa thành 'Chờ duyệt')
                db.Database.ExecuteSqlCommand("EXEC sp_DangKyPT @MaKH, @MaNV, @SoBuoi, @GiaMoiBuoi",
                    pMaKH, pMaNV, pSoBuoi, pGiaMoiBuoi);

                TempData["AlertMessage"] = "Đăng ký PT thành công! (Trạng thái: Chờ duyệt)";
                TempData["AlertStatus"] = "success";
            }
            catch (Exception ex)
            {
                // Bắt lỗi RAISERROR từ SP (ví dụ: "Nhân viên này không phải là PT")
                TempData["AlertMessage"] = "LỖI: " + (ex.InnerException?.InnerException?.Message ?? ex.Message);
                TempData["AlertStatus"] = "error";
            }

            return RedirectToAction("Index");
        }


        // Dọn dẹp
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
