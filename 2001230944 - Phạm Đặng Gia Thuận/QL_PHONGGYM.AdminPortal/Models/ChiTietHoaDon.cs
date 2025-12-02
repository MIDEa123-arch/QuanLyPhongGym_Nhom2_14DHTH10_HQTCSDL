using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QL_PHONGGYM.AdminPortal.Models
{
    [Table("ChiTietHoaDon")]
    public class ChiTietHoaDon
    {
        [Key]
        public int MaCTHD { get; set; }
        public int MaHD { get; set; }

        // Các cột liên kết (nullable vì 1 dòng chỉ là 1 loại)
        public int? MaDKGT { get; set; }
        public int? MaDKPT { get; set; }
        public int? MaDKLop { get; set; }
        public int? MaSP { get; set; }

        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }

        [ForeignKey("MaHD")]
        public virtual HoaDon HoaDon { get; set; }

        // Liên kết các bảng dịch vụ/sản phẩm
        [ForeignKey("MaSP")]
        public virtual SanPham SanPham { get; set; }

        [ForeignKey("MaDKGT")]
        public virtual DangKyGoiTap DangKyGoiTap { get; set; }

        [ForeignKey("MaDKPT")]
        public virtual DangKyPT DangKyPT { get; set; }

        [ForeignKey("MaDKLop")]
        public virtual DangKyLop DangKyLop { get; set; }
    }
}