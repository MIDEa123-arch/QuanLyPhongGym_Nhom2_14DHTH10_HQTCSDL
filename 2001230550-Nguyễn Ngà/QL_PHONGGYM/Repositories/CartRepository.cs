using QL_PHONGGYM.Models;
using QL_PHONGGYM.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.Mvc;


namespace QL_PHONGGYM.Repositories
{
    public class CartRepository
    {
        private readonly QL_PHONGGYMEntities _context;

        public CartRepository(QL_PHONGGYMEntities context)
        {
            _context = context;
        }

        public DangKyPTCheckout HoaDonPT(int MaKH, Decimal TongTien, Decimal ThanhTien, Decimal GiamGia)
        {
            var kh = _context.KhachHangs.Find(MaKH).TenKH;
            var hlv = _context.DangKyPTs.Find(MaKH).NhanVien.TenNV;
            var soBuoi = _context.DangKyPTs.Find(MaKH).SoBuoi;
            var giaBuoi = _context.DangKyPTs.Find(MaKH).GiaMoiBuoi;
            var mucTieu = _context.DangKyPTs.Find(MaKH).GhiChu;
            var ngaydk = _context.DangKyPTs.Find(MaKH).NgayDangKy;

            DangKyPTCheckout dk = new DangKyPTCheckout()
            {
                TenKH = kh,
                TenHLV = hlv,
                SoBuoi = soBuoi,
                GiaMoiBuoi = giaBuoi,
                NgayDK = ngaydk,
                MucTieu = mucTieu,
                TongTien = TongTien,
                ThanhTien = ThanhTien,
                GiamGia = GiamGia

            };

            return dk;
        }

        public bool DangKyPTDetail(int maHD)
        {            
            var hd = _context.HoaDons.Find(maHD);

            if (hd == null)
            {
                return false; 
            }
            
            hd.TrangThai = "Đã thanh toán";            
            _context.SaveChanges();

            return true;
        }


        public bool DangKyPT(FormCollection form, int makh)
        {
            try
            {
                DangKyPT dangKy = new DangKyPT
                {
                    MaKH = makh,
                    SoBuoi = Convert.ToInt32(form["soBuoi"]),
                    NgayDangKy = DateTime.Now,
                    TrangThai = "Chờ duyệt",
                    GhiChu = form["ghiChu"]
                };
                _context.DangKyPTs.Add(dangKy);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                throw;
            }
        }

        public bool KiemTraTrung(int makh)
        {
            var goiHienTai = _context.DangKyGoiTaps
                         .Where(x => x.MaKH == makh && x.TrangThai == "Còn hiệu lực")
                         .OrderByDescending(x => x.NgayKetThuc)
                         .FirstOrDefault();
            if (goiHienTai == null)
            {
                return false;
            }
            else
                return true;
        }

        public void TaoHoaDon(FormCollection form, int makh, List<GioHangViewModel> cart = null, int? maGoiTap = null, int? maLop = null)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    HoaDon hoaDon = new HoaDon
                    {
                        MaKH = makh,
                        TongTien = Convert.ToDecimal(form["tongTien"]),
                        ThanhTien = Convert.ToDecimal(form["thanhTien"]),
                        TrangThai = form["paymentMethod"] == "BANK" ? "Đã thanh toán" : "Chưa thanh toán",
                        GiamGia = form["giamGia"] != null ? Convert.ToDecimal(form["giamGia"]) : 0,
                        NgayLap = DateTime.Now,
                    };
                    _context.HoaDons.Add(hoaDon);
                    _context.SaveChanges(); 

                    if (cart != null && cart.Count > 0)
                    {
                        foreach (var item in cart)
                        {
                            var DonGia = item.GiaKhuyenMaiSP ?? item.DonGia;
                            ChiTietHoaDon ct = new ChiTietHoaDon
                            {
                                MaHD = hoaDon.MaHD,
                                MaSP = item.MaSP,
                                MaDKGT = null,
                                MaDKLop = null,
                                MaDKPT = null,
                                SoLuong = item.SoLuong,
                                DonGia = DonGia
                            };
                            _context.ChiTietHoaDons.Add(ct);

                            var list = _context.ChiTietGioHangs.Where(sp => sp.MaSP == item.MaSP && sp.MaKH == makh);
                            _context.ChiTietGioHangs.RemoveRange(list);
                        }
                    }

                    if (maLop.HasValue)
                    {
                        var lop = _context.LopHocs.FirstOrDefault(l => l.MaLop == maLop);
                        if (lop != null)
                        {
                            var maDKLopParam = new SqlParameter
                            {
                                ParameterName = "@MaDKLop",
                                SqlDbType = System.Data.SqlDbType.Int,
                                Direction = System.Data.ParameterDirection.Output
                            };

                            _context.Database.ExecuteSqlCommand(
                                "EXEC sp_DangKyLop_KiemTra @MaKH, @MaLop, @MaDKLop OUT",
                                new SqlParameter("@MaKH", makh),
                                new SqlParameter("@MaLop", maLop),
                                maDKLopParam
                            );

                            int newMaDKLop = (int)maDKLopParam.Value;

                            ChiTietHoaDon ctLop = new ChiTietHoaDon
                            {
                                MaHD = hoaDon.MaHD,
                                MaSP = null,
                                MaDKGT = null,
                                MaDKLop = newMaDKLop,
                                MaDKPT = null,
                                SoLuong = 1,
                                DonGia = lop.HocPhi
                            };
                            _context.ChiTietHoaDons.Add(ctLop);
                        }
                    }

                    if (maGoiTap.HasValue)
                    {
                        var goiTap = _context.GoiTaps.FirstOrDefault(gt => gt.MaGoiTap == maGoiTap);
                        if (goiTap != null)
                        {
                            var goiHienTai = _context.DangKyGoiTaps
                                .Where(x => x.MaKH == makh && x.TrangThai == "Còn hiệu lực")
                                .OrderByDescending(x => x.NgayKetThuc)
                                .FirstOrDefault();

                            DangKyGoiTap dangKy;

                            if (goiHienTai != null)
                            {
                                goiHienTai.NgayKetThuc = goiHienTai.NgayKetThuc.AddMonths(goiTap.ThoiHan);
                                _context.SaveChanges();
                                dangKy = goiHienTai;
                            }
                            else
                            {
                                dangKy = new DangKyGoiTap
                                {
                                    MaKH = makh,
                                    MaGoiTap = goiTap.MaGoiTap,
                                    NgayDangKy = DateTime.Now,
                                    NgayBatDau = DateTime.Now,
                                    NgayKetThuc = DateTime.Now.AddMonths(goiTap.ThoiHan),
                                    TrangThai = "Còn hiệu lực"
                                };
                                _context.DangKyGoiTaps.Add(dangKy);
                                _context.SaveChanges();
                            }

                            ChiTietHoaDon ctGoiTap = new ChiTietHoaDon
                            {
                                MaHD = hoaDon.MaHD,
                                MaSP = null,
                                MaDKGT = dangKy.MaDKGT,
                                MaDKLop = null,
                                MaDKPT = null,
                                SoLuong = 1,
                                DonGia = goiTap.Gia
                            };
                            _context.ChiTietHoaDons.Add(ctGoiTap);
                        }
                    }

                    _context.SaveChanges();
                    dbContextTransaction.Commit();
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }

        public void Xoa(int id)
        {
            var item = _context.ChiTietGioHangs.FirstOrDefault(sp => sp.MaSP == id);
            item.SoLuong = item.SoLuong - 1;

            if (item.SoLuong <= 0)
                _context.ChiTietGioHangs.Remove(item);

            _context.SaveChanges();
        }
        public void Them(int id)
        {
            var item = _context.ChiTietGioHangs.FirstOrDefault(sp => sp.MaSP == id);
            var product = _context.SanPhams.FirstOrDefault(sp => sp.MaSP == id);
            if (item.SoLuong >= product.SoLuongTon)
            {
                item.SoLuong = product.SoLuongTon;
            }
            item.SoLuong = item.SoLuong + 1;
            _context.SaveChanges();
        }
        public void XoaDon(int id, int makh)
        {


            var item = _context.ChiTietGioHangs.FirstOrDefault(c => c.MaKH == makh && c.MaSP == id);
            if (item != null)
            {
                _context.ChiTietGioHangs.Remove(item);
                _context.SaveChanges();
            }

        }
        public void XoaDaChon(FormCollection form, int makh)
        {
            string[] selected = form.GetValues("selectedItems");
            List<int> selectedIds = selected.Select(int.Parse).ToList();
            foreach (var id in selectedIds)
            {
                var item = _context.ChiTietGioHangs.FirstOrDefault(c => c.MaKH == makh && c.MaSP == id);
                if (item != null)
                    _context.ChiTietGioHangs.Remove(item);
            }

            _context.SaveChanges();
        }


        public List<GioHangViewModel> ChonSanPham(FormCollection form, int makh)
        {
            string[] selected = form.GetValues("selectedItems");
            List<int> selectedIds = selected.Select(int.Parse).ToList();
            List<GioHangViewModel> list = new List<GioHangViewModel>();
            var cart = GetCart(makh);

            foreach (var id in selectedIds)
            {
                var item = cart.FirstOrDefault(c => c.MaKH == makh && c.MaSP == id);              
                if (item != null)
                    list.Add(item);
            }

            return list;
        }
        public bool AddToCart(int maKH, int maSP, int soLuong)
        {
            if (soLuong <= 0)
                return false;
            try
            {
                _context.Database.ExecuteSqlCommand(
                    "EXEC sp_ThemVaoGioHang @MaKH, @MaSP, @SoLuong",
                    new SqlParameter("@MaKH", maKH),
                    new SqlParameter("@MaSP", maSP),
                    new SqlParameter("@SoLuong", soLuong)
                );

                return true; 
            }
            catch (SqlException ex)
            {               
                throw ex;
            }
        }

        public List<GioHangViewModel> GetCart(int maKH)
        {
            try
            {
                var cart = _context.ChiTietGioHangs
                    .Where(c => c.MaKH == maKH)
                    .Select(c => new GioHangViewModel
                    {
                        MaChiTietGH = c.MaChiTietGH,
                        MaKH = c.MaKH,
                        MaSP = c.MaSP,
                        SoLuong = c.SoLuong,
                        DonGia = c.SanPham.DonGia,
                        NgayThem = c.NgayThem,
                        GiaKhuyenMaiSP = c.SanPham.GiaKhuyenMai,
                        TenMonHang = c.SanPham.TenSP,
                        AnhDaiDienSP = c.SanPham.HINHANHs.FirstOrDefault(a => a.IsMain.HasValue && a.IsMain.Value == true).Url,
                        SoLuongTon = c.SanPham.SoLuongTon
                    }).ToList();

                return cart;
            }
            catch
            {
                throw;
            }
        }
        public bool DangKyLop(int maKH, int maLop)
        {
            try
            {
                var daDangKy = _context.DangKyLops.Any(x => x.MaKH == maKH && x.MaLop == maLop && x.TrangThai == "Còn hiệu lực");
                if (daDangKy) return false;

                var lop = _context.LopHocs.Find(maLop);
                int siSoHienTai = _context.DangKyLops.Count(x => x.MaLop == maLop && x.TrangThai == "Còn hiệu lực");

                if (lop.SiSoToiDa.HasValue && siSoHienTai >= lop.SiSoToiDa.Value) return false;

                var dangKy = new DangKyLop
                {
                    MaKH = maKH,
                    MaLop = maLop,
                    NgayDangKy = DateTime.Now,
                    TrangThai = "Còn hiệu lực"
                };

                _context.DangKyLops.Add(dangKy);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }


    }
}