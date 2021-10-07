using Demo_Login2.Areas.AdminPage.Business;
using Demo_Login2.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Demo_Login2.Areas.AdminPage.Controllers
{
    public class MonHocController : Controller
    {
        //LayDanhSachMonHoc
        public ActionResult Index()
        {
            var lstmonhoc = this.LayDanhSachMonHoc();
            ViewBag.phanloaikhoabm = LayDanhSachKhoaBoMon();
            return View(lstmonhoc);
        }

        public List<MonHocDTO> LayDanhSachMonHoc()
        {
            using(MonHocBusiness bs = new MonHocBusiness())
            {
                return bs.LayDanhSachMonHoc();
            }
        }
        //Get : TaoMonHoc
        public ActionResult Create()
        {
            ViewData["phanloaikhoabm"] = new SelectList(LayDanhSachKhoaBoMon(), "ID", "TenKhoaBoMon");          
            return View();
        }

        //Post : TaoMonHoc
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(MonHocDTO monhoc)
        {
            var findMa = LayMaMonHocDaTonTai(monhoc.MaMonHoc);
            var findTen = LayTenMonHocDaTonTai(monhoc.TenMonHoc);
            if(findMa > 0)
            {
                ViewBag.ErrorMa = "Mã Môn Học đã tồn tại";
                ViewData["phanloaikhoabm"] = new SelectList(LayDanhSachKhoaBoMon(), "ID", "TenKhoaBoMon");
                return View();
            }else if(findTen > 0)
            {
                ViewBag.ErrorTen = "Tên Môn Học đã tồn tại";
                ViewData["phanloaikhoabm"] = new SelectList(LayDanhSachKhoaBoMon(), "ID", "TenKhoaBoMon");
                return View();
            }
            else if (monhoc.SoTietLyThuyet < 1 && monhoc.SoTietThucHanh < 1)
            {
                ViewBag.ErrorSoTiet = "Một trong hai số tiết không được bé hơn 0";
                ViewData["phanloaikhoabm"] = new SelectList(LayDanhSachKhoaBoMon(), "ID", "TenKhoaBoMon");
                return View();
            }
            else
            {
                ThemMonHoc(monhoc);
                return RedirectToAction("Index");
            }
        }

        //Upload Danh sach Excel 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upload(FormCollection formCollection)
        {
            if(Request != null)
            {
                HttpPostedFileBase file = Request.Files["UploadedFile"];
                if((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                {
                    OfficeOpenXml.ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                    using (var package = new OfficeOpenXml.ExcelPackage(file.InputStream))
                    {
                        var currentSheet = package.Workbook.Worksheets;
                        var workSheet = currentSheet.First();
                        var noOfCol = workSheet.Dimension.End.Column;
                        var noOfRow = workSheet.Dimension.End.Row;
                        ViewBag.Error = "";
                        List<MonHocDTO> monhocList = new List<MonHocDTO>();
                        for(int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                        {
                            var monhoc = new MonHocDTO();
                            monhoc.MaMonHoc = Convert.ToString(workSheet.Cells[rowIterator, 1].Value).Trim();
                            monhoc.TenMonHoc = Convert.ToString(workSheet.Cells[rowIterator, 2].Value).Trim();
                            monhoc.IDKhoaBoMon = Convert.ToInt32(workSheet.Cells[rowIterator, 3].Value);
                            monhoc.SoTietLyThuyet = Convert.ToInt32(workSheet.Cells[rowIterator, 4].Value);
                            monhoc.SoTietThucHanh = Convert.ToInt32(workSheet.Cells[rowIterator, 5].Value);
                            monhoc.SoTinChi = Convert.ToInt32(workSheet.Cells[rowIterator, 6].Value);
                            monhoc.GhiChu = Convert.ToString(workSheet.Cells[rowIterator, 7].Value);

                            var findMaMonHoc = LayMaMonHocDaTonTai(monhoc.MaMonHoc);
                            var findTenMonHoc = LayTenMonHocDaTonTai(monhoc.TenMonHoc);
                            if (findMaMonHoc > 0)
                            {
                                ViewBag.trungdata = String.Format("Dữ liệu bị trùng ở hàng {0} cột Mã Môn Học trong file Excel", rowIterator);
                                var lstmonhoc = this.LayDanhSachMonHoc();
                                ViewBag.phanloaikhoabm = LayDanhSachKhoaBoMon();
                                return View("Index",lstmonhoc);
                            }
                            else if (findTenMonHoc > 0)
                            {
                                ViewBag.trungdata = String.Format("Dữ liệu bị trùng ở hàng {0} cột Tên Môn Học trong file Excel", rowIterator);
                                var lstmonhoc = this.LayDanhSachMonHoc();
                                ViewBag.phanloaikhoabm = LayDanhSachKhoaBoMon();
                                return View("Index", lstmonhoc);
                            }else if(monhoc.MaMonHoc.Length > 7)
                            {
                                ViewBag.Errordodaima = String.Format("Dữ liệu bị lỗi ở hàng {0} do mã lớn hơn 7 kí tự tại cột Mã Môn Học trong file Excel", rowIterator);
                                var lstmonhoc = this.LayDanhSachMonHoc();
                                ViewBag.phanloaikhoabm = LayDanhSachKhoaBoMon();
                                return View("Index", lstmonhoc);
                            }
                            else
                            {
                                monhocList.Add(monhoc);
                            }
                        }
                        foreach(var item in monhocList)
                        {
                            ThemMonHoc(item);
                        }
                    }
                }
                else
                {
                    ViewBag.Error = "Chưa có File";
                    var lstmonhoc = this.LayDanhSachMonHoc();
                    ViewBag.phanloaikhoabm = LayDanhSachKhoaBoMon();
                    return View("Index", lstmonhoc);

                }
            }
            ViewBag.Success = "Thành công";
            var lstmh = this.LayDanhSachMonHoc();
            ViewBag.phanloaikhoabm = LayDanhSachKhoaBoMon();
            return View("Index", lstmh);
        }


        public int LayMaMonHocDaTonTai(string mamonhoc)
        {
            using(MonHocBusiness bs = new MonHocBusiness())
            {
                return bs.LayMaMonHocDaTonTai(mamonhoc);
            }
        }

        public int LayTenMonHocDaTonTai(string tenmonhoc)
        {
            using(MonHocBusiness bs = new MonHocBusiness())
            {
                return bs.LayTenMonHocDaTonTai(tenmonhoc);
            }
        }
        public List<HocPhanTienQuyetDTO> LayDanhSachHocPhanTienQuyet()
        {
            using(HocPhanTienQuyetBusiness bs = new HocPhanTienQuyetBusiness())
            {
                return bs.LayDanhSachHocPhanTienQuyet();
            }
        }

        public List<HocPhanHocTruocDTO> LayDanhSachHocPhanHocTruoc()
        {
            using (HocPhanHocTruocBusiness bs = new HocPhanHocTruocBusiness())
            {
                return bs.LayDanhSachHocPhanHocTruoc();
            }
        }

        public List<KhoaBoMonDTO> LayDanhSachKhoaBoMon()
        {
            using(KhoaBoMonBusiness bs = new KhoaBoMonBusiness())
            {
                return bs.LayDanhSachKhoaBoMon();
            }
        }
        
        public bool ThemMonHoc(MonHocDTO monhoc)
        {
            using(MonHocBusiness bs = new MonHocBusiness())
            {
                return bs.ThemMonHoc(monhoc);
            }
        }

        //Get : SuaMonHoc
        public ActionResult Edit(int id)
        {
            ViewData["phanloaikhoabm"] = new SelectList(LayDanhSachKhoaBoMon(), "ID", "TenKhoaBoMon");
            MonHocDTO monhoc = LayMonHoc(id);
            return View(monhoc);
        }

        //Post : SuaMonHoc
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(MonHocDTO monhoc)
        {
            var findMa = LayMaMonHocDaTonTai(monhoc.MaMonHoc);
            var findTen = LayTenMonHocDaTonTai(monhoc.TenMonHoc);
            if((findMa == monhoc.ID || findMa == 0) && (findTen == monhoc.ID || findTen == 0) &&(monhoc.SoTietLyThuyet >1 || monhoc.SoTietThucHanh > 1))
            {
                SuaMonHoc(monhoc);
                return RedirectToAction("Index");
            }
            else
            {
                if(findMa != monhoc.ID && findMa > 0)
                {
                    ViewBag.ErrorMa = "Mã Môn Học đã tồn tại";
                    ViewData["phanloaikhoabm"] = new SelectList(LayDanhSachKhoaBoMon(), "ID", "TenKhoaBoMon");
                    return View();
                }
                else if (monhoc.SoTietLyThuyet < 1 || monhoc.SoTietThucHanh < 1)
                {
                    ViewBag.ErrorSoTiet = "Một trong hai số tiết không được bé hơn 0";
                    ViewData["phanloaikhoabm"] = new SelectList(LayDanhSachKhoaBoMon(), "ID", "TenKhoaBoMon");
                    return View();
                }
                else
                {
                    ViewBag.ErrorTen = "Tên Môn Học đã tồn tại";
                    ViewData["phanloaikhoabm"] = new SelectList(LayDanhSachKhoaBoMon(), "ID", "TenKhoaBoMon");
                    return View();
                }
            }
        }

        public MonHocDTO LayMonHoc(int id)
        {
            using(MonHocBusiness bs = new MonHocBusiness())
            {
                return bs.LayMonHoc(id);
            }
        }

        public bool SuaMonHoc(MonHocDTO monhoc)
        {
            using(MonHocBusiness bs = new MonHocBusiness())
            {
                return bs.SuaMonHoc(monhoc);
            }
        }

        //XoaMonHoc
        public async Task<ActionResult> Delete(int id)
        {
            var findmonhoc = CheckLoiMonHocDaTonTai(id);
            var findketquahoctap = CheckLoiKetQuaHocTapDaTonTai(id);
            if (findmonhoc > 0)
            {
                TempData["error"] = "lỗi";
                ViewBag.phanloaikhoabm = LayDanhSachKhoaBoMon();               
                return RedirectToAction("Index");
            }else if(findketquahoctap > 0)
            {
                TempData["error"] = "lỗi";
                ViewBag.phanloaikhoabm = LayDanhSachKhoaBoMon();
                return RedirectToAction("Index");
            }
            else
            {
                var output = XoaMonHoc(id);
                if (output)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View("Fail");
                }
            }
            
        }

        public bool XoaMonHoc(int id)
        {
            using(MonHocBusiness bs = new MonHocBusiness())
            {
                return bs.XoaMonHoc(id);
            }
        }

        public int CheckLoiMonHocDaTonTai(int? id)
        {
            using(MonHocBusiness bs = new MonHocBusiness())
            {
                return bs.CheckLoiMonHocDaTonTai(id);
            }
        }

        public int CheckLoiKetQuaHocTapDaTonTai(int? id)
        {
            using (MonHocBusiness bs = new MonHocBusiness())
            {
                return bs.CheckLoiKetQuaHocTapDaTonTai(id);
            }
        }

        //XemChiTietMonHoc
        public ActionResult Details(int id)
        {
            var hocki = LayMonHoc(id);
            ViewBag.phanloaikhoabm = LayDanhSachKhoaBoMon();
            return View(hocki);
        }
    }
}