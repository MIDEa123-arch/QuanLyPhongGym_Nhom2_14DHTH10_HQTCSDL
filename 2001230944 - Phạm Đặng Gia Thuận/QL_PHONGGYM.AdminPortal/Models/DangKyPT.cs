using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace QL_PHONGGYM.AdminPortal.Models
{
    // Ánh xạ (map) tới bảng DangKyPT
    [Table("DangKyPT")]
    public class DangKyPT
    {
        [Key]
        public int MaDKPT { get; set; }

        // Khóa ngoại tới KhachHang
        public int MaKH { get; set; }

        // Khóa ngoại tới NhanVien (người dạy)
        public int MaNV { get; set; }

        public int SoBuoi { get; set; }
        public decimal GiaMoiBuoi { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime NgayDangKy { get; set; }

        [StringLength(20)]
        public string TrangThai { get; set; }

        // ======================================================
        // Tạo mối quan hệ (Navigation Properties)
        // ======================================================

        [ForeignKey("MaKH")]
        public virtual KhachHang KhachHang { get; set; }

        [ForeignKey("MaNV")]
        public virtual NhanVien NhanVien { get; set; }

        // Một Gói Đăng Ký PT có nhiều Lịch Tập PT
        public virtual ICollection<LichTapPT> LichTapPTs { get; set; }
    }
}
