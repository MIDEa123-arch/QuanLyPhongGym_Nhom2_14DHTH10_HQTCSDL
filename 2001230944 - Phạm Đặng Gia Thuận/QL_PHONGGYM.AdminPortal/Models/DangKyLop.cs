using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace QL_PHONGGYM.AdminPortal.Models
{
    // Ánh xạ (map) tới bảng DangKyLop
    [Table("DangKyLop")]
    public class DangKyLop
    {
        [Key]
        public int MaDKLop { get; set; }

        // Khóa ngoại tới KhachHang
        public int MaKH { get; set; }

        // Khóa ngoại tới LopHoc
        public int MaLop { get; set; }

        public DateTime NgayDangKy { get; set; }

        [StringLength(20)]
        public string TrangThai { get; set; }

        // ======================================================
        // Tạo mối quan hệ (Navigation Properties)
        // ======================================================

        [ForeignKey("MaKH")]
        public virtual KhachHang KhachHang { get; set; }

        [ForeignKey("MaLop")]
        public virtual LopHoc LopHoc { get; set; }
    }
}