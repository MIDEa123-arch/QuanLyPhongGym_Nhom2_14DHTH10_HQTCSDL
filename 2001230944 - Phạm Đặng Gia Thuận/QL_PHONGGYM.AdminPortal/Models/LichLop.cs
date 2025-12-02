using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QL_PHONGGYM.AdminPortal.Models
{
    [Table("LichLop")]
    public class LichLop
    {
        [Key]
        public int MaLichLop { get; set; }

        public int MaLop { get; set; }

        // ==========================================
        // == SỬA LỖI TẠI ĐÂY: Thêm dấu ? vào sau int ==
        // ==========================================
        public int? MaNV { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime NgayHoc { get; set; }

        [Required]
        public TimeSpan GioBatDau { get; set; }

        [Required]
        public TimeSpan GioKetThuc { get; set; }

        // (Đã xóa cột TrangThai)

        [ForeignKey("MaLop")]
        public virtual LopHoc LopHoc { get; set; }

        [ForeignKey("MaNV")]
        public virtual NhanVien NhanVien { get; set; }
    }
}