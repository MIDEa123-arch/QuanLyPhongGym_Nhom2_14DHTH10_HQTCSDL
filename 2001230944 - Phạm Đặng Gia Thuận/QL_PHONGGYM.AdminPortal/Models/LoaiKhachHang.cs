using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QL_PHONGGYM.AdminPortal.Models
{
    // Ánh xạ file C# này tới bảng "LoaiKhachHang" trong CSDL
    [Table("LoaiKhachHang")]
    public class LoaiKhachHang
    {
        [Key] // Đánh dấu đây là Khóa chính
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MaLoaiKH { get; set; }

        [Display(Name = "Tên loại khách hàng")]
        [StringLength(50)]
        public string TenLoai { get; set; }

        [Display(Name = "Mức giảm (%)")]
        public decimal? MucGiam { get; set; }
    }
}
