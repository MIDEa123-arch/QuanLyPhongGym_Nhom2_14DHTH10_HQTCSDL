using QL_PHONGGYM.AdminPortal.Models;
using System.Data.Entity;

namespace QL_PHONGGYM.AdminPortal.Data
{
    public class ApplicationDbContext : DbContext
    {
        // "FullConnection" là tên của chuỗi kết nối trong Web.config
        // mà chúng ta đã thêm ở bước trước (chuỗi kết nối có User/Pass)
        public ApplicationDbContext() : base("name=FullConnection")
        {
        }

        // === PHẦN KHÁCH HÀNG ===
        public DbSet<KhachHang> KhachHangs { get; set; }
        public DbSet<LoaiKhachHang> LoaiKhachHangs { get; set; }
        public DbSet<CheckIn> CheckIns { get; set; }
        public DbSet<DiaChi> DiaChis { get; set; }
        // public DbSet<DiaChi> DiaChis { get; set; } // (Chúng ta chưa tạo Model cho bảng này)

        // === PHẦN NHÂN VIÊN ===
        public DbSet<NhanVien> NhanViens { get; set; }
        public DbSet<ChucVu> ChucVus { get; set; }
        public DbSet<ChuyenMon> ChuyenMons { get; set; }
        public DbSet<NhanVienChuyenMon> NhanVienChuyenMons { get; set; }

        // === PHẦN SẢN PHẨM ===
        public DbSet<SanPham> SanPhams { get; set; }
        public DbSet<LoaiSanPham> LoaiSanPhams { get; set; }
        public DbSet<HINHANH> HINHANHs { get; set; }

        // === PHẦN DỊCH VỤ - GÓI TẬP ===
        public DbSet<GoiTap> GoiTaps { get; set; }
        public DbSet<DangKyGoiTap> DangKyGoiTaps { get; set; }

        // === PHẦN DỊCH VỤ - LỚP HỌC ===
        public DbSet<LopHoc> LopHocs { get; set; }
        public DbSet<DangKyLop> DangKyLops { get; set; }
        public DbSet<LichLop> LichLops { get; set; }

        // === PHẦN DỊCH VỤ - PT ===
        public DbSet<DangKyPT> DangKyPTs { get; set; }
        public DbSet<LichTapPT> LichTapPTs { get; set; }

        public System.Data.Entity.DbSet<QL_PHONGGYM.AdminPortal.Models.HoaDon> HoaDons { get; set; }


        // === CÁC BẢNG KHÁC (Hóa Đơn, Giỏ Hàng...) ===
        // (Chúng ta sẽ thêm sau khi tạo Model cho chúng)
        // public DbSet<HoaDon> HoaDons { get; set; }
        // public DbSet<ChiTietHoaDon> ChiTietHoaDons { get; set; }
        // public DbSet<ChiTietGioHang> ChiTietGioHangs { get; set; }
    }
}
