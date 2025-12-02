using QL_PHONGGYM.Models;
using QL_PHONGGYM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QL_PHONGGYM.Repositories
{
    public class ProductRepository
    {
        private readonly QL_PHONGGYMEntities _context;

        public ProductRepository(QL_PHONGGYMEntities context)
        {
            _context = context;
        }

        public LopHoc LopHocDetail(int maLop)
        {
            return _context.LopHocs.FirstOrDefault(sp => sp.MaLop == maLop);
        }

        public List<ChuyenMon> GetChuyenMons()
        {
            var list = _context.ChuyenMons.ToList();
            return list;
        }

        public List<LoaiSanPham> GetLoaiSanPhams()
        {
            return _context.LoaiSanPhams.ToList();
        }

        public List<string> GetHangsByLoai()
        {
            return _context.SanPhams.Where(sp => sp.Hang != null).Select(sp => sp.Hang).Distinct().ToList();
        }

        public List<string> GetXuatSu()
        {
            return _context.SanPhams.Where(sp => sp.XuatXu != null).Select(sp => sp.XuatXu).Distinct().ToList();
        }

        public List<GoiTap> GetGoiTaps()
        {   
            return _context.GoiTaps.Where(sp => sp.Gia == 399000.00m || sp.Gia == 10000000.00m).ToList();
        }

        public List<SanPhamViewModel> GetSanPhams()
        {
            var list = (from sp in _context.SanPhams
                        join ha in _context.HINHANHs on sp.MaSP equals ha.MaSP into haGroup
                        select new SanPhamViewModel
                        {
                            MaSP = sp.MaSP,
                            TenSP = sp.TenSP,
                            LoaiSP = sp.LoaiSanPham.TenLoaiSP,
                            DonGia = sp.DonGia,
                            SoLuongTon = sp.SoLuongTon,
                            GiaKhuyenMai = (decimal)sp.GiaKhuyenMai,
                            Hang = sp.Hang,
                            XuatXu = sp.XuatXu,
                            BaoHanh = sp.BaoHanh,
                            MoTaChung = sp.MoTaChung,
                            MaTaChiTiet = sp.MoTaChiTiet,
                            UrlHinhAnhChinh = haGroup.FirstOrDefault(h => h.IsMain == true).Url,
                            UrlHinhAnhsPhu = haGroup.Where(h => h.IsMain == false)
                                                    .Select(h => h.Url)
                                                    .ToList()
                        }).ToList();

            return list;
        }

        public List<ThongTinLop> GetLopHocs(string keyword, int? maCM, int? maKH, string filterType)
        {
            var query = _context.LopHocs.AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(l => l.TenLop.Contains(keyword));

            if (maCM.HasValue)
                query = query.Where(l => l.MaCM == maCM.Value);

            var listLopRaw = query.OrderByDescending(l => l.NgayBatDau).ToList();
            var result = new List<ThongTinLop>();

            List<dynamic> userSchedule = new List<dynamic>();
            List<int> joinedClasses = new List<int>();

            if (maKH.HasValue)
            {
                joinedClasses = _context.DangKyLops
                    .Where(dk => dk.MaKH == maKH.Value && dk.TrangThai == "Còn hiệu lực")
                    .Select(dk => dk.MaLop).ToList();

                var lichPT = (from lpt in _context.LichTapPTs
                              join dkpt in _context.DangKyPTs on lpt.MaDKPT equals dkpt.MaDKPT
                              where dkpt.MaKH == maKH.Value && lpt.TrangThai == "Chưa tập"
                              select new { Ngay = lpt.NgayTap, Start = lpt.GioBatDau, End = lpt.GioKetThuc }).ToList();

                var lichLop = (from ll in _context.LichLops
                               join dkl in _context.DangKyLops on ll.MaLop equals dkl.MaLop
                               where dkl.MaKH == maKH.Value && ll.TrangThai == "Chưa diễn ra" && dkl.TrangThai == "Còn hiệu lực"
                               select new { Ngay = ll.NgayHoc, Start = ll.GioBatDau, End = ll.GioKetThuc }).ToList();

                userSchedule.AddRange(lichPT);
                userSchedule.AddRange(lichLop);
            }

            foreach (var lop in listLopRaw)
            {
                bool isJoined = joinedClasses.Contains(lop.MaLop);
                bool isConflict = false;

                if (maKH.HasValue && !isJoined)
                {
                    var lichCuaLopNay = _context.LichLops.Where(l => l.MaLop == lop.MaLop).ToList();

                    foreach (var buoiHoc in lichCuaLopNay)
                    {
                        foreach (var buoiBan in userSchedule)
                        {
                            if (buoiHoc.NgayHoc == buoiBan.Ngay &&
                                buoiHoc.GioBatDau < buoiBan.End &&
                                buoiHoc.GioKetThuc > buoiBan.Start)
                            {
                                isConflict = true;
                                break;
                            }
                        }
                        if (isConflict) break;
                    }
                }

                if (filterType == "not-joined" && isJoined) continue;
                if (filterType == "suitable" && (isJoined || isConflict)) continue;
                
                var hlvName = _context.NhanViens.FirstOrDefault(nv => nv.MaNV == lop.MaNV)?.TenNV ?? "Đang cập nhật";
                var chuyenMonName = lop.ChuyenMon?.TenChuyenMon ?? "Gym";
                var siSo = _context.DangKyLops.Count(x => x.MaLop == lop.MaLop && x.TrangThai == "Còn hiệu lực");

                result.Add(new ThongTinLop
                {
                    MaLop = lop.MaLop,
                    TenLop = lop.TenLop,
                    TenHLV = hlvName,
                    TenChuyenMon = chuyenMonName,
                    NgayBatDau = lop.NgayBatDau,
                    NgayKetThuc = lop.NgayKetThuc,
                    HocPhi = lop.HocPhi,
                    SiSoHienTai = siSo,
                    SiSoToiDa = lop.SiSoToiDa,
                    DaDangKy = isJoined,
                    BiTrungLich = isConflict
                });
            }

            return result;
        }
    }

    public class ThongTinLop
    {
        public int MaLop { get; set; }
        public string TenLop { get; set; }
        public string TenHLV { get; set; }
        public string TenChuyenMon { get; set; }
        public DateTime NgayBatDau { get; set; }
        public DateTime NgayKetThuc { get; set; }
        public decimal HocPhi { get; set; }
        public int SiSoHienTai { get; set; }
        public int? SiSoToiDa { get; set; }
        public bool DaDangKy { get; set; }
        public bool BiTrungLich { get; set; }
    }
}