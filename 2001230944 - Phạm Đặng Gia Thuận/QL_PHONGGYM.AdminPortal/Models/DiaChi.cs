using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QL_PHONGGYM.AdminPortal.Models
{
    [Table("DiaChi")]
    public class DiaChi
    {
        [Key]
        public int MaDC { get; set; }

        public int MaKH { get; set; } // Khóa ngoại

        [Required]
        [StringLength(100)]
        [Display(Name = "Tỉnh/Thành Phố")]
        public string TinhThanhPho { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Quận/Huyện")]
        public string QuanHuyen { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Phường/Xã")]
        public string PhuongXa { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Địa chỉ cụ thể")]
        public string DiaChiCuThe { get; set; }

        [Display(Name = "Là địa chỉ mặc định")]
        public bool LaDiaChiMacDinh { get; set; }

        // Quan hệ: Thuộc về một Khách Hàng
        [ForeignKey("MaKH")]
        public virtual KhachHang KhachHang { get; set; }
    }
}