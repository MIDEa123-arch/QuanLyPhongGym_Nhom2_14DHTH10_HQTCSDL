using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace QL_PHONGGYM.AdminPortal.Models
{
    [Table("NhanVien")]
    public class NhanVien
    {
        [Key]
        public int MaNV { get; set; }

        [Display(Name = "Chức Vụ")]
        public int? MaChucVu { get; set; }

        [StringLength(300)]
        [Display(Name = "Tên Đăng Nhập")]
        public string TenDangNhap { get; set; }

        [StringLength(1000)]
        [Display(Name = "Mật Khẩu")]
        [DataType(DataType.Password)] // Ẩn mật khẩu khi nhập
        public string MatKhau { get; set; }

        [StringLength(60)]
        [Display(Name = "Giới Tính")]
        public string GioiTinh { get; set; }

        [Required(ErrorMessage = "Tên nhân viên không được để trống")]
        [StringLength(100)]
        [Display(Name = "Tên Nhân Viên")]
        public string TenNV { get; set; }

        [StringLength(15)]
        [Display(Name = "Số Điện Thoại")]
        public string SDT { get; set; }

        [Display(Name = "Ngày Sinh")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? NgaySinh { get; set; }

        // Navigation property (để EF biết mối quan hệ)
        [ForeignKey("MaChucVu")]
        public virtual ChucVu ChucVu { get; set; }

        // Mối quan hệ: Một Nhân Viên có thể có nhiều NhanVienChuyenMon
        public virtual ICollection<NhanVienChuyenMon> NhanVienChuyenMons { get; set; }
    }
}