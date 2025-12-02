using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QL_PHONGGYM.AdminPortal.Models
{
    // Ánh xạ (map) class C# này tới bảng "CheckIn" trong CSDL
    [Table("CheckIn")]

    public class CheckIn
    {
        // Đánh dấu đây là Khóa chính
        [Key]
        public int MaCheckIn { get; set; }

        // Bắt buộc phải có
        [Required]
        public int MaKH { get; set; }

        [Required]
        public DateTime ThoiGianVao { get; set; }

        // Dấu ? có nghĩa là "có thể null" (cho phép null)
        // vì khi mới check-in, ThoiGianRa sẽ là null
        public DateTime? ThoiGianRa { get; set; }

        public string ThoiGianRaFormatted
        {
            get
            {
                // Nếu ThoiGianRa có giá trị, format theo "dd/MM/yyyy HH:mm", nếu null trả về rỗng hoặc thông báo
                return ThoiGianRa.HasValue ? ThoiGianRa.Value.ToString("dd/MM/yyyy HH:mm") : "";
            }
        }

        [StringLength(50)]
        public string TrangThai { get; set; }

        // ======================================================
        // Tạo mối quan hệ (Navigation Property)
        // "Một lượt CheckIn thuộc về Một Khách Hàng"
        // ======================================================

        // Foreign Key (Khóa ngoại)
        // Từ khóa "virtual" giúp Entity Framework tải dữ liệu liên quan (lười biadini)
        [ForeignKey("MaKH")]
        public virtual KhachHang KhachHang { get; set; }
    }
}
