using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace QL_PHONGGYM.AdminPortal.Models
{
    // Ánh xạ (map) tới bảng LichTapPT
    [Table("LichTapPT")]
    public class LichTapPT
    {
        [Key]
        public int MaLichPT { get; set; }

        // Khóa ngoại tới DangKyPT
        public int MaDKPT { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime NgayTap { get; set; }

        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = @"{0:hh\:mm}", ApplyFormatInEditMode = true)]
        public TimeSpan GioBatDau { get; set; }

        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = @"{0:hh\:mm}", ApplyFormatInEditMode = true)]
        public TimeSpan GioKetThuc { get; set; }

        [StringLength(20)]
        public string TrangThai { get; set; }

        // ======================================================
        // Tạo mối quan hệ (Navigation Properties)
        // ======================================================

        [ForeignKey("MaDKPT")]
        public virtual DangKyPT DangKyPT { get; set; }
    }
}
