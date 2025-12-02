using System;
using System.Configuration;
using System.Data.SqlClient; // Cần thiết để kết nối SQL
using System.Web.Mvc;
using System.Web.Security; // Cần thiết cho FormsAuthentication

namespace QL_PHONGGYM.AdminPortal.Controllers
{
    public class LoginController : Controller
    {
        // Biến tĩnh lưu chuỗi kết nối cơ sở (không có user/pass)
        private static string baseConnectionString = ConfigurationManager.ConnectionStrings["BaseConnection"].ConnectionString;

        // GET: /Login
        // Hiển thị trang đăng nhập
        [AllowAnonymous] // Cho phép truy cập dù chưa đăng nhập
        public ActionResult Index()
        {
            // Nếu đã đăng nhập rồi thì đá về trang chủ
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // POST: /Login/Authenticate
        // Xử lý việc đăng nhập
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken] // Chống tấn công CSRF
        public ActionResult Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                TempData["ErrorMessage"] = "Tên đăng nhập và mật khẩu không được để trống.";
                return RedirectToAction("Index");
            }

            // Đây là logic cốt lõi dựa trên SQL script của bạn
            // Chúng ta sẽ xây dựng chuỗi kết nối ĐỘNG
            // DÙNG TÊN ĐĂNG NHẬP VÀ MẬT KHẨU MÀ USER NHẬP VÀO
            string dynamicConnectionString = $"{baseConnectionString}User ID={username};Password={password};";

            SqlConnection connection = null;
            try
            {
                // Thử kết nối đến SQL Server
                connection = new SqlConnection(dynamicConnectionString);
                connection.Open();

                // Nếu kết nối thành công (không ném Exception)
                // -> Login và Password là ĐÚNG

                // 1. Tạo "vé" đăng nhập (Cookie)
                // Chúng ta lưu Tên đăng nhập vào "vé"
                FormsAuthentication.SetAuthCookie(username, false); // false = không "Remember me"

                // 2. Chuyển hướng về trang chủ
                return RedirectToAction("Index", "Home");
            }
            catch (SqlException ex)
            {
                // Nếu kết nối thất bại (Lỗi 18456: Login failed)
                // -> Login hoặc Password là SAI
                TempData["ErrorMessage"] = "Tên đăng nhập hoặc mật khẩu không chính xác.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Lỗi khác (ví dụ: không tìm thấy server)
                TempData["ErrorMessage"] = "Lỗi hệ thống: " + ex.Message;
                return RedirectToAction("Index");
            }
            finally
            {
                // Luôn đóng kết nối
                if (connection != null && connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        // GET: /Login/Logout
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear(); // Xóa mọi session
            return RedirectToAction("Index", "Login");
        }
    }
}
