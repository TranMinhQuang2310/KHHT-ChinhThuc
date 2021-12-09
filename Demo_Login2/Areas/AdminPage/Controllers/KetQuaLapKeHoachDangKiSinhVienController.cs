using Demo_Login2.Areas.AdminPage.Business;
using Demo_Login2.Models.DTO;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Demo_Login2.Areas.AdminPage.Controllers
{
    public class KetQuaLapKeHoachDangKiSinhVienController : Controller
    {
        // GET: AdminPage/KetQuaLapKeHoachDangKiSinhVien
        public ActionResult Index()
        {
            var lstketqua = this.LayDanhSachKetQuaLapKeHoachDangKiSinhVien(0, 0, 0,0);

            thuvienChung(0,0);

            ViewBag.HocKi = LayDanhSachHocKi();
            ViewBag.SinhVien = LayDanhSachTaiKhoan();

            Session["lstketquakehoach"] = lstketqua;
            

            return View(lstketqua);
        }

        public void thuvienChung(int idkhoa, int idlop)
        {

            var lstkhoaDT = LayDanhSachKhoaDaoTao();
            var lsthocki = LayDanhSachHocKi();
            var lstsinhvien = LayDanhSachSinhVienTheoLopHoc(idlop);
            var lstlophoc = LayDanhSachLopHocTheoKhoaDaoTaoTrongKetQuaLapKeHoachDangKi(idkhoa);

            lstkhoaDT.Insert(0, new KhoaDaoTaoDTO
            {
                ID = 0,
                TenKhoaDaoTao = "Chọn Khóa Đào Tạo"
            });
            lsthocki.Insert(0, new HocKiDTO
            {
                ID = 0,
                TenHocKi = "Chọn Học Kì"
            });
            lstsinhvien.Insert(0, new AccountDTO
            {
                ID = 0,
                Ma = "Chọn Mã Sinh Viên"
            });
            lstlophoc.Insert(0, new LopHocDTO
            {
                ID = 0,
                TenLop = "Chọn Lớp Học"
            });
            Session["idkhoa"] = idkhoa;
            Session["idlop"] = idlop;
            ViewData["khoaDT"] = new SelectList(lstkhoaDT, "ID", "TenKhoaDaoTao",idkhoa);
            ViewData["HK"] = new SelectList(lsthocki, "ID", "TenHocKi",idlop);
            ViewData["SV"] = new SelectList(lstsinhvien, "ID", "Ma");
            ViewData["LH"] = new SelectList(lstlophoc, "ID", "TenLop");
        }

        [HttpPost]
        public ActionResult Index(int idKhoaDT,int idlop,int idHocKi,int idSinhVien)
        {
            var lstketqua = this.LayDanhSachKetQuaLapKeHoachDangKiSinhVien(idKhoaDT,idlop, idHocKi, idSinhVien);

            thuvienChung(idKhoaDT, idlop);

            ViewBag.HocKi = LayDanhSachHocKi();
            ViewBag.SinhVien = LayDanhSachTaiKhoan();

            Session["lstketquakehoach"] = lstketqua;
            Session["idHocKi"] = idHocKi;

            return View(lstketqua);
        }

        //XuatFileExcel
        public ActionResult onchangeKhoa(string idkhoa, string idlop)
        {
            var lstketqua = this.LayDanhSachKetQuaLapKeHoachDangKiSinhVien(0,0,0,0);

            thuvienChung(Convert.ToInt32(idkhoa), Convert.ToInt32(idlop));

            ViewBag.HocKi = LayDanhSachHocKi();
            ViewBag.SinhVien = LayDanhSachTaiKhoan();


            return View("Index",lstketqua);
        }
        public void XuatBaoCaoKeHoachSinhVien()
        {
            ExcelPackage EP = new ExcelPackage();
            ExcelWorksheet Sheet = EP.Workbook.Worksheets.Add("Report");
            Sheet.Cells["A1"].Value = "STT";
            Sheet.Cells["B1"].Value = "Tên Sinh viên";
            Sheet.Cells["C1"].Value = "Khóa Đào Tạo";
            Sheet.Cells["D1"].Value = "Lớp";
            Sheet.Cells["E1"].Value = "Học kì";
            Sheet.Cells["F1"].Value = "Tên Môn Học";
            Sheet.Cells["G1"].Value = "Số Tín Chỉ";
            Sheet.Cells["H1"].Value = "Số Tiết Lý Thuyết";
            Sheet.Cells["I1"].Value = "Số Tiết Thực Hành";
            Sheet.Cells["J1"].Value = "Tên Khoa Bộ Môn";
            Sheet.Cells["K1"].Value = "Môn Học Tiên Quyết";
            Sheet.Cells["L1"].Value = "Môn Học Học Trước";
            Sheet.Cells["M1"].Value = "Loại Môn Học";
            Sheet.Cells["N1"].Value = "Trạng thái Đăng Kí";

            int row = 2;
            foreach(var item in (List<SinhVienDangKiKeHoachHocTapDTO>)Session["lstketquakehoach"])
            {
                Sheet.Cells[String.Format("A{0}", row)].Value = row - 1;
                Sheet.Cells[String.Format("B{0}", row)].Value = item.TenSinhVien;
                Sheet.Cells[String.Format("C{0}", row)].Value = item.TenKhoaDaoTao;
                Sheet.Cells[String.Format("D{0}", row)].Value = item.TenLop;
                Sheet.Cells[String.Format("E{0}", row)].Value = item.TenHocKi;
                Sheet.Cells[String.Format("F{0}", row)].Value = item.TenMonHoc;
                Sheet.Cells[String.Format("G{0}", row)].Value = item.SoTinChi;
                Sheet.Cells[String.Format("H{0}", row)].Value = item.SoTietLyThuyet;
                Sheet.Cells[String.Format("I{0}", row)].Value = item.SoTietThucHanh;
                Sheet.Cells[String.Format("J{0}", row)].Value = item.TenKhoaBoMon;
                Sheet.Cells[String.Format("K{0}", row)].Value = item.TenMonHocTienQuyet;
                Sheet.Cells[String.Format("L{0}", row)].Value = item.TenMonHocHocTruoc;
                Sheet.Cells[String.Format("M{0}", row)].Value = getLoaiDangKi((int)item.IDPhanLoaiMonHoc);
                Sheet.Cells[String.Format("N{0}", row)].Value = getTrangThai(item.TrangThai);
                row++;
            }
            Sheet.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.BinaryWrite(EP.GetAsByteArray());
            Response.End();
        }

        public string getLoaiDangKi(int idPhanLoai)
        {
            string result = "";
            if (idPhanLoai == 1)
            {
                result = "Bắt Buộc";
            }
            else if(idPhanLoai == 2)
            {
                result = "Tự Chọn";
            }
            return result;
        }

        public string getTrangThai(Boolean trangthai)
        {
            string result = "";
            if(trangthai == true)
            {
                result = "Đã Đăng Kí";
            }
            else
            {
                result = "Chưa Đăng Kí";
            }
            return result;
        }

        public List<SinhVienDangKiKeHoachHocTapDTO> LayDanhSachKetQuaLapKeHoachDangKiSinhVien(int idKhoaDT,int idlop, int idHocKi, int idSinhVien)
        {
            using(KetQuaLapKeHoachDangKiSinhVienBusiness bs = new KetQuaLapKeHoachDangKiSinhVienBusiness())
            {
                return bs.LayDanhSachKetQuaLapKeHoachDangKiSinhVien(idKhoaDT,idlop, idHocKi, idSinhVien);
            }
        }

        public List<KhoaDaoTaoDTO> LayDanhSachKhoaDaoTao()
        {
            using(KhoaDaoTaoBusiness bs = new KhoaDaoTaoBusiness())
            {
                return bs.LayDanhSachKhoaDaoTao();
            }
        }

        public List<HocKiDTO> LayDanhSachHocKi()
        {
            using (HocKiBusiness bs = new HocKiBusiness())
            {
                return bs.LayDanhSachHocKi();
            }
        }

        public List<AccountDTO> LayDanhSachSinhVienTheoLopHoc(int idlop)
        {
            using(TaiKhoanBusiness bs = new TaiKhoanBusiness())
            {
                return bs.LayDanhSachSinhVienTheoLopHoc(idlop);
            }
        }
        public List<LopHocDTO> LayDanhSachLopHocTheoKhoaDaoTaoTrongKetQuaLapKeHoachDangKi(int idkhoa)
        {
            using(LopHocBusiness bs = new LopHocBusiness())
            {
                return bs.LayDanhSachLopHocTheoKhoaDaoTaoTrongKetQuaLapKeHoachDangKi(idkhoa);
            }
        }

        public List<AccountDTO> LayDanhSachTaiKhoan()
        {
            using(TaiKhoanBusiness bs = new TaiKhoanBusiness())
            {
                return bs.LayDanhSachTaiKhoan();
            }
        }


    }
}