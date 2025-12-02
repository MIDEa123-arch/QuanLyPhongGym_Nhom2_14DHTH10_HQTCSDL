using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace QL_PHONGGYM.AdminPortal.Models
{
    [Table("NhanVienChuyenMon")]
    public class NhanVienChuyenMon
    {
        // Khóa chính phức hợp (Composite Key)
        [Key]
        [Column(Order = 0)] // Đây là cột đầu tiên của khóa
        public int MaNV { get; set; }

        [Key]
        [Column(Order = 1)] // Đây là cột thứ hai của khóa
        public int MaCM { get; set; }

        // Navigation properties (để EF biết mối quan hệ)
        [ForeignKey("MaNV")]
        public virtual NhanVien NhanVien { get; set; }

        [ForeignKey("MaCM")]
        public virtual ChuyenMon ChuyenMon { get; set; }
    }
}
