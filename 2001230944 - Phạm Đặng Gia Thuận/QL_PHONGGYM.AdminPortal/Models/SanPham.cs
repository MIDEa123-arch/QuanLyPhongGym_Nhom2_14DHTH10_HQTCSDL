using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QL_PHONGGYM.AdminPortal.Models
{
    // Ánh xạ class này tới bảng "SanPham" trong CSDL
    [Table("SanPham")]
    public class SanPham
    {
        [Key] // Đánh dấu đây là Khóa chính
        [DisplayName("Mã SP")]
        public int MaSP { get; set; }

        [DisplayName("Tên Sản Phẩm")]
        [StringLength(100)]
        public string TenSP { get; set; }

        [DisplayName("Loại SP")]
        public int? MaLoaiSP { get; set; } // FK

        [DisplayName("Đơn Giá")]
        [Required(ErrorMessage = "Đơn giá là bắt buộc.")]
        [Column(TypeName = "decimal")]
        public decimal DonGia { get; set; }

        [DisplayName("Số Lượng Tồn")]
        [Required(ErrorMessage = "Số lượng tồn là bắt buộc.")]
        public int SoLuongTon { get; set; }

        [DisplayName("Giá Khuyến Mãi")]
        [Column(TypeName = "decimal")]
        public decimal? GiaKhuyenMai { get; set; }

        [StringLength(100)]
        public string Hang { get; set; }

        [StringLength(100)]
        [DisplayName("Xuất Xứ")]
        public string XuatXu { get; set; }

        [StringLength(50)]
        [DisplayName("Bảo Hành")]
        public string BaoHanh { get; set; }

        [StringLength(1000)]
        [DisplayName("Mô Tả Chung")]
        public string MoTaChung { get; set; }

        [DisplayName("Mô Tả Chi Tiết")]
        public string MoTaChiTiet { get; set; }

        // --- Navigation Properties (Mối quan hệ) ---

        // Khai báo mối quan hệ: Một Sản Phẩm thuộc về MỘT Loại Sản Phẩm
        // Dòng này giúp Entity Framework hiểu MaLoaiSP là Khóa ngoại
        public virtual LoaiSanPham LoaiSanPham { get; set; }

        // Khai báo mối quan hệ: Một Sản Phẩm có thể có NHIỀU Hình Ảnh
        public virtual ICollection<HINHANH> HINHANHs { get; set; }
    }
}
