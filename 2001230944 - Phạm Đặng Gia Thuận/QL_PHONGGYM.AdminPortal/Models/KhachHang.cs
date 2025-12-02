using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; // Cần dùng thư viện này

namespace QL_PHONGGYM.AdminPortal.Models
{
    // Ánh xạ file C# này tới bảng "KhachHang" trong CSDL
    [Table("KhachHang")]
    public class KhachHang
    {
        [Key] // Đánh dấu đây là Khóa chính (MaKH)
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Báo cho EF biết đây là cột tự động tăng
        public int MaKH { get; set; }

        [Display(Name = "Tên khách hàng")]
        [Required(ErrorMessage = "Tên không được để trống")]
        [StringLength(100)]
        public string TenKH { get; set; }

        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date)] // Giúp trình duyệt hiển thị ô chọn ngày
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? NgaySinh { get; set; } // Dấu ? vì cột này là NULL

        [Display(Name = "Số điện thoại")]
        [StringLength(15)]
        public string SDT { get; set; }

        [StringLength(100)]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }

        [Display(Name = "Mã Loại KH")]
        public int? MaLoaiKH { get; set; } // Dấu ? vì cột này là NULL

        [Display(Name = "Giới tính")]
        [StringLength(60)]
        public string GioiTinh { get; set; }

        [Display(Name = "Tên đăng nhập")]
        [StringLength(300)]
        public string TenDangNhap { get; set; } // Đã thêm cột này

        [Display(Name = "Mật khẩu")]
        [StringLength(1000)]
        [DataType(DataType.Password)] // Giúp ẩn mật khẩu khi nhập
        public string MatKhau { get; set; } // Đã thêm cột này

        // Thêm dòng này để tạo mối quan hệ 1-Nhiều
        public virtual ICollection<DiaChi> DiaChis { get; set; }
        [ForeignKey("MaLoaiKH")]
        public virtual LoaiKhachHang LoaiKhachHang { get; set; }
    }
}