using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QL_PHONGGYM.AdminPortal.Models
{
    // Ánh xạ (map) class này tới bảng "LoaiSanPham" trong CSDL
    [Table("LoaiSanPham")]
    public class LoaiSanPham
    {
        [Key] // Đánh dấu đây là Khóa chính (Primary Key)
        [DisplayName("Mã Loại")]
        public int MaLoaiSP { get; set; }

        [DisplayName("Tên Loại Sản Phẩm")]
        [Required(ErrorMessage = "Tên loại sản phẩm là bắt buộc.")]
        [StringLength(50)]
        public string TenLoaiSP { get; set; }

        // Khai báo mối quan hệ: Một Loại Sản Phẩm có thể có NHIỀU Sản Phẩm
        // Dòng này giúp Entity Framework hiểu mối quan hệ
        public virtual ICollection<SanPham> SanPhams { get; set; }
    }
}