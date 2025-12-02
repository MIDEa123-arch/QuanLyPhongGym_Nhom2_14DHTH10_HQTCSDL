using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QL_PHONGGYM.AdminPortal.Models
{
    [Table("HINHANH")]
    public class HINHANH
    {
        [Key]
        public int MaHinh { get; set; }

        public int MaSP { get; set; } // FK

        [StringLength(500)]
        [Required]
        public string Url { get; set; }

        [DisplayName("Là Ảnh Chính")]
        public bool? IsMain { get; set; }

        // Mối quan hệ: Một Hình Ảnh thuộc về MỘT Sản Phẩm
        public virtual SanPham SanPham { get; set; }
    }
}