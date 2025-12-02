using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace QL_PHONGGYM.AdminPortal.Models
{
    // Ánh xạ (map) tới bảng DangKyGoiTap
    [Table("DangKyGoiTap")]
    public class DangKyGoiTap
    {
        // Khóa chính
        [Key]
        public int MaDKGT { get; set; }

        // Khóa ngoại tới KhachHang
        public int MaKH { get; set; }

        // Khóa ngoại tới GoiTap
        public int MaGoiTap { get; set; }

        public DateTime NgayDangKy { get; set; }
        public DateTime NgayBatDau { get; set; }
        public DateTime NgayKetThuc { get; set; }

        [StringLength(20)]
        public string TrangThai { get; set; }

        // ======================================================
        // Tạo mối quan hệ (Navigation Properties)
        // "Một Lượt Đăng Ký này thuộc về MỘT Khách Hàng"
        // ======================================================
        [ForeignKey("MaKH")]
        public virtual KhachHang KhachHang { get; set; }

        // "Một Lượt Đăng Ký này thuộc về MỘT Gói Tập"
        [ForeignKey("MaGoiTap")]
        public virtual GoiTap GoiTap { get; set; }
    }
}
