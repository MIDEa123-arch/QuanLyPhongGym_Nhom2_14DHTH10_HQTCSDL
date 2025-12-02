using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QL_PHONGGYM.AdminPortal.Data;
using QL_PHONGGYM.AdminPortal.Models;
using System.Data.SqlClient;

namespace QL_PHONGGYM.AdminPortal.Controllers
{
    //[Authorize(Roles = "ManagerRole")]
    public class LopHocsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: LopHoc
        public ActionResult Index()
        {
            var lopHocs = db.LopHocs
                .Include(l => l.ChuyenMon)
                .Include(l => l.NhanVien)
                .OrderBy(l => l.TenLop); // <-- Đã thêm sắp xếp
            return View(lopHocs.ToList());
        }

        // GET: LopHoc/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            LopHoc lopHoc = db.LopHocs.Include(l => l.ChuyenMon).Include(l => l.NhanVien).SingleOrDefault(l => l.MaLop == id);
            if (lopHoc == null) return HttpNotFound();
            return View(lopHoc);
        }

        // GET: LopHoc/Create
        public ActionResult Create()
        {
            ViewBag.MaCM = new SelectList(db.ChuyenMons, "MaCM", "TenChuyenMon");

            // Lấy danh sách HLV/PT (Hiển thị cả chuyên môn của họ để dễ chọn)
            // Ví dụ: Nguyễn Văn A (Boxing, Yoga)
            var giaoVienList = db.NhanViens
                .Include(n => n.ChucVu)
                .Include(n => n.NhanVienChuyenMons.Select(cm => cm.ChuyenMon))
                .Where(n => n.ChucVu.TenChucVu == "HLV Lớp" || n.ChucVu.TenChucVu == "PT")
                .ToList() // Lấy về RAM để xử lý chuỗi chuyên môn
                .Select(n => new
                {
                    MaNV = n.MaNV,
                    // Hiển thị: Tên NV (Các môn dạy được)
                    TenNV = n.TenNV + " (" + string.Join(", ", n.NhanVienChuyenMons.Select(cm => cm.ChuyenMon.TenChuyenMon)) + ")"
                });

            ViewBag.MaNV = new SelectList(giaoVienList, "MaNV", "TenNV");
            return View();
        }

        // POST: LopHoc/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaLop,TenLop,MaCM,HocPhi,NgayBatDau,NgayKetThuc,SoBuoi,SiSoToiDa,MaNV,GioBatDau,GioKetThuc")] LopHoc lopHoc)
        {
            // ============================================================
            // == [QUAN TRỌNG] KIỂM TRA CHUYÊN MÔN NHÂN VIÊN ==
            // ============================================================
            if (lopHoc.MaNV != null)
            {
                // Kiểm tra xem nhân viên này có chuyên môn (MaCM) của lớp học này không
                bool coChuyenMon = db.NhanVienChuyenMons
                                     .Any(nc => nc.MaNV == lopHoc.MaNV && nc.MaCM == lopHoc.MaCM);

                if (!coChuyenMon)
                {
                    // Lấy tên chuyên môn để báo lỗi cho rõ
                    var tenCM = db.ChuyenMons.Find(lopHoc.MaCM)?.TenChuyenMon;
                    ModelState.AddModelError("MaNV", $"Nhân viên này KHÔNG CÓ chuyên môn '{tenCM}'. Vui lòng chọn nhân viên khác hoặc đổi chuyên môn.");
                }
            }
            // ============================================================

            if (ModelState.IsValid)
            {
                try
                {
                    var pTenLop = new SqlParameter("@TenLop", lopHoc.TenLop);
                    var pMaCM = new SqlParameter("@MaCM", lopHoc.MaCM);
                    var pHocPhi = new SqlParameter("@HocPhi", lopHoc.HocPhi);
                    var pNgayBatDau = new SqlParameter("@NgayBatDau", lopHoc.NgayBatDau);
                    var pNgayKetThuc = new SqlParameter("@NgayKetThuc", lopHoc.NgayKetThuc);
                    var pSoBuoi = new SqlParameter("@SoBuoi", lopHoc.SoBuoi);
                    var pSiSoToiDa = new SqlParameter("@SiSoToiDa", (object)lopHoc.SiSoToiDa ?? 30);
                    var pMaNV = new SqlParameter("@MaNV", (object)lopHoc.MaNV ?? DBNull.Value);
                    var pGioBatDau = new SqlParameter("@GioBatDau", lopHoc.GioBatDau);
                    var pGioKetThuc = new SqlParameter("@GioKetThuc", lopHoc.GioKetThuc);

                    db.Database.ExecuteSqlCommand("EXEC sp_ThemLopHocVaLich @TenLop, @MaCM, @HocPhi, @NgayBatDau, @NgayKetThuc, @SoBuoi, @SiSoToiDa, @MaNV, @GioBatDau, @GioKetThuc",
                        pTenLop, pMaCM, pHocPhi, pNgayBatDau, pNgayKetThuc, pSoBuoi, pSiSoToiDa, pMaNV, pGioBatDau, pGioKetThuc);

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    string loi = ex.Message;
                    if (ex.InnerException != null) loi = ex.InnerException.InnerException.Message;
                    ModelState.AddModelError("", "LỖI: " + loi);
                }
            }

            ViewBag.MaCM = new SelectList(db.ChuyenMons, "MaCM", "TenChuyenMon", lopHoc.MaCM);

            var giaoVienList = db.NhanViens
                .Include(n => n.ChucVu)
                .Include(n => n.NhanVienChuyenMons.Select(cm => cm.ChuyenMon))
                .Where(n => n.ChucVu.TenChucVu == "HLV Lớp" || n.ChucVu.TenChucVu == "PT")
                .ToList()
                .Select(n => new
                {
                    MaNV = n.MaNV,
                    TenNV = n.TenNV + " (" + string.Join(", ", n.NhanVienChuyenMons.Select(cm => cm.ChuyenMon.TenChuyenMon)) + ")"
                });
            ViewBag.MaNV = new SelectList(giaoVienList, "MaNV", "TenNV", lopHoc.MaNV);

            return View(lopHoc);
        }

        // GET: LopHoc/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            LopHoc lopHoc = db.LopHocs.Include(l => l.LichLops).SingleOrDefault(l => l.MaLop == id);
            if (lopHoc == null) return HttpNotFound();

            // Lấy giờ từ lịch đầu tiên để hiển thị
            var lichDauTien = lopHoc.LichLops.FirstOrDefault();
            if (lichDauTien != null)
            {
                lopHoc.GioBatDau = lichDauTien.GioBatDau;
                lopHoc.GioKetThuc = lichDauTien.GioKetThuc;
            }

            ViewBag.MaCM = new SelectList(db.ChuyenMons, "MaCM", "TenChuyenMon", lopHoc.MaCM);

            // Hiển thị list GV kèm chuyên môn
            var giaoVienList = db.NhanViens
                .Include(n => n.ChucVu)
                .Include(n => n.NhanVienChuyenMons.Select(cm => cm.ChuyenMon))
                .Where(n => n.ChucVu.TenChucVu == "HLV Lớp" || n.ChucVu.TenChucVu == "PT")
                .ToList()
                .Select(n => new
                {
                    MaNV = n.MaNV,
                    TenNV = n.TenNV + " (" + string.Join(", ", n.NhanVienChuyenMons.Select(cm => cm.ChuyenMon.TenChuyenMon)) + ")"
                });
            ViewBag.MaNV = new SelectList(giaoVienList, "MaNV", "TenNV", lopHoc.MaNV);

            return View(lopHoc);
        }

        // POST: LopHoc/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaLop,TenLop,MaCM,HocPhi,NgayBatDau,NgayKetThuc,SoBuoi,SiSoToiDa,MaNV,GioBatDau,GioKetThuc")] LopHoc lopHoc)
        {
            // 1. Kiểm tra Chuyên môn Nhân viên
            if (lopHoc.MaNV != null)
            {
                bool coChuyenMon = db.NhanVienChuyenMons
                                     .Any(nc => nc.MaNV == lopHoc.MaNV && nc.MaCM == lopHoc.MaCM);
                if (!coChuyenMon)
                {
                    var tenCM = db.ChuyenMons.Find(lopHoc.MaCM)?.TenChuyenMon;
                    ModelState.AddModelError("MaNV", $"Nhân viên này KHÔNG CÓ chuyên môn '{tenCM}'.");
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Các tham số cơ bản
                    var pMaLop = new SqlParameter("@MaLop", lopHoc.MaLop);
                    var pTenLop = new SqlParameter("@TenLop", lopHoc.TenLop);
                    var pMaCM = new SqlParameter("@MaCM", lopHoc.MaCM);
                    var pHocPhi = new SqlParameter("@HocPhi", lopHoc.HocPhi);
                    var pNgayBatDau = new SqlParameter("@NgayBatDau", lopHoc.NgayBatDau);
                    var pNgayKetThuc = new SqlParameter("@NgayKetThuc", lopHoc.NgayKetThuc);
                    var pSoBuoi = new SqlParameter("@SoBuoi", lopHoc.SoBuoi);
                    var pSiSoToiDa = new SqlParameter("@SiSoToiDa", (object)lopHoc.SiSoToiDa ?? 30);
                    var pMaNV = new SqlParameter("@MaNV", (object)lopHoc.MaNV ?? DBNull.Value);

                    // XỬ LÝ GIỜ: Nếu là 00:00 (do người dùng không sửa), truyền NULL để SP bỏ qua
                    object valGioBD = DBNull.Value;
                    object valGioKT = DBNull.Value;

                    // Chỉ truyền giờ nếu cả 2 đều khác 0 và Hợp lệ
                    if (lopHoc.GioBatDau != TimeSpan.Zero && lopHoc.GioKetThuc != TimeSpan.Zero && lopHoc.GioKetThuc > lopHoc.GioBatDau)
                    {
                        valGioBD = lopHoc.GioBatDau;
                        valGioKT = lopHoc.GioKetThuc;
                    }

                    var pGioBatDau = new SqlParameter("@GioBatDau", valGioBD);
                    var pGioKetThuc = new SqlParameter("@GioKetThuc", valGioKT);

                    // Gọi SP
                    db.Database.ExecuteSqlCommand("EXEC sp_SuaLopHoc @MaLop, @TenLop, @MaCM, @HocPhi, @NgayBatDau, @NgayKetThuc, @SoBuoi, @SiSoToiDa, @MaNV, @GioBatDau, @GioKetThuc",
                        pMaLop, pTenLop, pMaCM, pHocPhi, pNgayBatDau, pNgayKetThuc, pSoBuoi, pSiSoToiDa, pMaNV, pGioBatDau, pGioKetThuc);

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    string loi = ex.Message;
                    if (ex.InnerException != null) loi = ex.InnerException.InnerException.Message;
                    ModelState.AddModelError("", "LỖI: " + loi);
                }
            }

            // Load lại Dropdown nếu lỗi
            ViewBag.MaCM = new SelectList(db.ChuyenMons, "MaCM", "TenChuyenMon", lopHoc.MaCM);
            var giaoVienList = db.NhanViens
                .Include(n => n.ChucVu)
                .Where(n => n.ChucVu.TenChucVu == "HLV Lớp" || n.ChucVu.TenChucVu == "PT")
                .Select(n => new { MaNV = n.MaNV, TenNV = n.TenNV + " (" + n.ChucVu.TenChucVu + ")" }).ToList();
            ViewBag.MaNV = new SelectList(giaoVienList, "MaNV", "TenNV", lopHoc.MaNV);

            return View(lopHoc);
        }
        // ... (Các hàm Delete giữ nguyên) ...
        // GET: LopHoc/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            LopHoc lopHoc = db.LopHocs.Include(l => l.ChuyenMon).Include(l => l.NhanVien).SingleOrDefault(l => l.MaLop == id);
            if (lopHoc == null) return HttpNotFound();
            return View(lopHoc);
        }

        // POST: LopHoc/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                var pMaLop = new System.Data.SqlClient.SqlParameter("@MaLop", id);
                db.Database.ExecuteSqlCommand("EXEC sp_XoaLopHoc @MaLop", pMaLop);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                string loi = ex.Message;
                if (ex.InnerException != null) loi = ex.InnerException.InnerException.Message;
                ModelState.AddModelError("", "KHÔNG THỂ XÓA: " + loi);
                LopHoc lopHoc = db.LopHocs.Include(l => l.ChuyenMon).Include(l => l.NhanVien).SingleOrDefault(l => l.MaLop == id);
                return View(lopHoc);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}
