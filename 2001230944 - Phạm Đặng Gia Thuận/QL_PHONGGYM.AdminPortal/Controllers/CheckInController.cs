using QL_PHONGGYM.AdminPortal.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QL_PHONGGYM.AdminPortal.Controllers
{
    // Bắt buộc phải đăng nhập để vào trang này
    [Authorize]
    public class CheckInController : Controller
    {
        // GET: CheckIn
        // Hiển thị trang Check-in
        public ActionResult Index()
        {
            return View();
        }

        // POST: CheckIn
        // Xử lý khi nhấn nút "Check-in"
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckIn(string maKhachHang)
        {
            // Kiểm tra xem chuỗi nhập vào có phải là số không
            if (string.IsNullOrWhiteSpace(maKhachHang) || !int.TryParse(maKhachHang, out int maKH))
            {
                // Dùng TempData để gửi thông báo lỗi về View
                TempData["CheckInStatus"] = "error";
                TempData["CheckInMessage"] = "Mã khách hàng không hợp lệ. Vui lòng nhập bằng SỐ.";
                return RedirectToAction("Index");
            }

            // Dùng 'using' để đảm bảo kết nối CSDL được đóng an toàn
            using (var db = new ApplicationDbContext()) // Dùng DbContext của EF để lấy chuỗi kết nối
            {
                // Chuẩn bị các tham số cho Stored Procedure (SP)
                var maKhParam = new SqlParameter("@MaKH", SqlDbType.Int)
                {
                    Value = maKH
                };

                try
                {
                    // Thực thi Stored Procedure 'sp_CheckInKhachHang'
                    // SP này sẽ trả về 1 hàng, 1 cột (tên là TrangThaiCheckIn)
                    var ketQua = db.Database.SqlQuery<string>(
                        "EXEC sp_CheckInKhachHang @MaKH",
                        maKhParam
                    ).FirstOrDefault(); // Lấy kết quả đầu tiên

                    if (ketQua != null)
                    {
                        if (ketQua == "Hợp lệ")
                        {
                            TempData["CheckInStatus"] = "success";
                            TempData["CheckInMessage"] = $"Check-in thành công cho Mã KH: {maKH}. (Trạng thái: Hợp lệ)";
                        }
                        else // (ketQua == "Hết hạn")
                        {
                            TempData["CheckInStatus"] = "warning";
                            TempData["CheckInMessage"] = $"Check-in thành công cho Mã KH: {maKH}. (Cảnh báo: Gói tập đã Hết hạn)";
                        }
                    }
                    else
                    {
                        // Trường hợp SP chạy nhưng không trả về gì (lạ)
                        TempData["CheckInStatus"] = "error";
                        TempData["CheckInMessage"] = "Stored Procedure không trả về kết quả.";
                    }
                }
                catch (SqlException ex)
                {
                    // Bắt lỗi từ SQL Server (ví dụ: Không tìm thấy KH, lỗi CSDL...)
                    TempData["CheckInStatus"] = "error";
                    TempData["CheckInMessage"] = "Lỗi SQL: " + ex.Message;
                }
                catch (Exception ex)
                {
                    // Bắt các lỗi C# khác
                    TempData["CheckInStatus"] = "error";
                    TempData["CheckInMessage"] = "Lỗi hệ thống: " + ex.Message;
                }
            }

            // Quay lại trang Check-in (Index) để hiển thị thông báo
            return RedirectToAction("Index");
        }
    }
}
