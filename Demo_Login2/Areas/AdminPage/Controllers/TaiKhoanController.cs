using Demo_Login2.Areas.AdminPage.Business;
using Demo_Login2.Models.DTO;
using Demo_Login2.Models.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Demo_Login2.Areas.AdminPage.Controllers
{
    public class TaiKhoanController : Controller
    {
        //Get: LayDanhSachTaiKhoan
        public ActionResult Index()
        {
            var lstAccount = this.LayDanhSachTaiKhoan();
            ViewBag.phanloaitaikhoan = LayDanhSachPhanLoaiTaiKhoan();
            return View(lstAccount);
        }
        public List<AccountDTO> LayDanhSachTaiKhoan()
        {
            using (TaiKhoanBusiness bs = new TaiKhoanBusiness())
            {
                return bs.LayDanhSachTaiKhoan();
            }
        }
        public List<PhanLoaiTaiKhoanDTO> LayDanhSachPhanLoaiTaiKhoan()
        {
            using (PhanLoaiTaiKhoanBusiness bs = new PhanLoaiTaiKhoanBusiness())
            {
                return bs.LayDanhSachPhanLoaiTaiKhoan();
            }
        }
        //Get: TaoTaiKhoan
        public ActionResult Create()
        {
            ViewData["phanloaitaikhoan"] = new SelectList(LayDanhSachPhanLoaiTaiKhoan(), "ID", "LoaiTaiKhoan");
            return View();
        }

        //Post :TaoTaiKhoan
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AccountDTO account)
        {
            var findMail = LayMailDaTonTai(account.MailVL);
            var findMa = LayMaDaTonTai(account.Ma);
            if(findMa > 0)
            {
                ViewBag.ErrorMa = "Mã đã tồn tại";
                ViewData["phanloaitaikhoan"] = new SelectList(LayDanhSachPhanLoaiTaiKhoan(), "ID", "LoaiTaiKhoan");
                return View();
            }else if(findMail > 0)
            {
                ViewBag.ErrorMail = "Mail đã tồn tại";
                ViewData["phanloaitaikhoan"] = new SelectList(LayDanhSachPhanLoaiTaiKhoan(), "ID", "LoaiTaiKhoan");
                return View();
            }
            else
            {
                ThemTaiKhoan(account);
                return RedirectToAction("Index");
            }
        }

        //Upload Danh sach Excel 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upload(FormCollection formCollection)
        {
            if (Request != null)
            {
                HttpPostedFileBase file = Request.Files["UploadedFile"];
                if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                {
                    OfficeOpenXml.ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                    using (var package = new OfficeOpenXml.ExcelPackage(file.InputStream))
                    {
                        var currentSheet = package.Workbook.Worksheets;
                        var workSheet = currentSheet.First();
                        var noOfCol = workSheet.Dimension.End.Column;
                        var noOfRow = workSheet.Dimension.End.Row;
                        ViewBag.Error = "";
                        List<AccountDTO> accountList = new List<AccountDTO>();
                        for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                        {
                            
                            var acc = new AccountDTO();
                            acc.Ma = Convert.ToString(workSheet.Cells[rowIterator, 1].Value).Trim();
                            acc.HoVaTen = Convert.ToString(workSheet.Cells[rowIterator, 2].Value).Trim();
                            acc.MailVL = Convert.ToString(workSheet.Cells[rowIterator, 3].Value).Trim();
                            acc.PhanLoai = Convert.ToInt32(workSheet.Cells[rowIterator, 4].Value);
                            acc.DaXem = Convert.ToBoolean(workSheet.Cells[rowIterator, 5].Value);
                            acc.GhiChu = Convert.ToString(workSheet.Cells[rowIterator, 6].Value).Trim();

                            var findMail = LayMailDaTonTai(acc.MailVL);
                            var findMa = LayMaDaTonTai(acc.Ma);

                            if(findMa > 0 )
                            {
                                ViewBag.trungdata = String.Format("Dữ liệu bị trùng ở hàng {0} cột Mã trong file Excel", rowIterator);
                                var lstAccount = this.LayDanhSachTaiKhoan();
                                ViewBag.phanloaitaikhoan = LayDanhSachPhanLoaiTaiKhoan();
                                return View("Index", lstAccount);
                            }
                            if (findMail > 0)
                            {
                                ViewBag.trungdata = String.Format("Dữ liệu bị trùng ở hàng {0} cột Mailvl trong file Excel", rowIterator);
                                var lstAccount = this.LayDanhSachTaiKhoan();
                                ViewBag.phanloaitaikhoan = LayDanhSachPhanLoaiTaiKhoan();
                                return View("Index", lstAccount);
                            }
                            else
                            {
                                accountList.Add(acc);
                            }
                        }
                        foreach(var item in accountList){
                            ThemTaiKhoan(item);
                        }

                    }
                }
                else
                {
                    ViewBag.Error = "Chưa có File";
                    var lstAccount = this.LayDanhSachTaiKhoan();
                    ViewBag.phanloaitaikhoan = LayDanhSachPhanLoaiTaiKhoan();
                    return View("Index", lstAccount);

                }
            }
            ViewBag.Success = "Thành công";
            var lstAcc = this.LayDanhSachTaiKhoan();
            ViewBag.phanloaitaikhoan = LayDanhSachPhanLoaiTaiKhoan();
            return View("Index", lstAcc);
        }

        public int LayMailDaTonTai(string mailvl)
        {
            using(TaiKhoanBusiness bs = new TaiKhoanBusiness())
            {
                return bs.LayMailDaTonTai(mailvl);
            }
        }

        public int LayMaDaTonTai(string ma)
        {
            using(TaiKhoanBusiness bs = new TaiKhoanBusiness())
            {
                return bs.LayMaDaTonTai(ma);
            }
        }

        public List<KhoaDaoTaoDTO> LayDanhSachKhoaDaoTao()
        {
            using (KhoaDaoTaoBusiness bs = new KhoaDaoTaoBusiness())
            {
                return bs.LayDanhSachKhoaDaoTao();
            }
        }
        public bool ThemTaiKhoan(AccountDTO taikhoan)
        {
            using (TaiKhoanBusiness bs = new TaiKhoanBusiness())
            {
                return bs.ThemTaiKhoan(taikhoan);
            }
        }

        //Get: Suataikhoan
        public ActionResult Edit(int id)
        {
            ViewData["phanloaitaikhoan"] = new SelectList(LayDanhSachPhanLoaiTaiKhoan(), "ID", "LoaiTaiKhoan");
            AccountDTO account = LayTaiKhoan(id);
            return View(account);
        }
        //Post : SuaTaiKhoan
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(AccountDTO account)
        {
            var findMa = LayMaDaTonTai(account.Ma);
            var findMail = LayMailDaTonTai(account.MailVL);
            if((findMa == account.ID || findMa == 0) && (findMail == account.ID || findMail == 0)){
                SuaTaiKhoan(account);
                return RedirectToAction("Index");
            }
            else
            {
                if(findMa != account.ID && findMa > 0)
                {
                    ViewBag.ErrorMa = "Mã đã tồn tại";
                    ViewData["phanloaitaikhoan"] = new SelectList(LayDanhSachPhanLoaiTaiKhoan(), "ID", "LoaiTaiKhoan");
                    return View();
                }
                else
                {
                    ViewBag.ErrorMail = "Mail đã tồn tại";
                    ViewData["phanloaitaikhoan"] = new SelectList(LayDanhSachPhanLoaiTaiKhoan(), "ID", "LoaiTaiKhoan");
                    return View();
                }
            }
        }

        public AccountDTO LayTaiKhoan(int id)
        {
            using(TaiKhoanBusiness bs = new TaiKhoanBusiness())
            {
                return bs.LayTaiKhoan(id);
            }
        }
        public bool SuaTaiKhoan(AccountDTO taikhoan)
        {
            using (TaiKhoanBusiness bs = new TaiKhoanBusiness())
            {
                return bs.SuaTaiKhoan(taikhoan);
            }
        }

        //Xoataikhoan
        public async Task<ActionResult> Delete(int id)
        {
            var findgv = CheckLoiGiangVienDaTonTai(id);
            var findsv = CheckLoiSinhVienDaTonTai(id);
            var findsv_ketquahoctap = CheckLoiSinhVienDaTonTai_KetQuaHocTap(id);
            if (findgv > 0)
            {
                TempData["error"] = "lỗi";
                ViewBag.phanloaitaikhoan = LayDanhSachPhanLoaiTaiKhoan();
                return RedirectToAction("Index");
            }else if(findsv > 0){ 
                TempData["error"] = "lỗi";
                ViewBag.phanloaitaikhoan = LayDanhSachPhanLoaiTaiKhoan();
                return RedirectToAction("Index");
            }else if(findsv_ketquahoctap > 0)
            {
                TempData["error"] = "lỗi";
                ViewBag.phanloaitaikhoan = LayDanhSachPhanLoaiTaiKhoan();
                return RedirectToAction("Index");
            }            
            else {
                var output = XoaTaiKhoan(id);
                if (output)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View("fail");
                }
            }

            
        }

        public bool XoaTaiKhoan(int id)
        {
            using (TaiKhoanBusiness bs = new TaiKhoanBusiness())
            {
                return bs.XoaTaiKhoan(id);
            }
        }

        //Xemchitiet
        public ActionResult Details(int id)
        {
           
            var account = LayTaiKhoan(id);
            ViewBag.taikhoan = LayDanhSachTaiKhoan();
            ViewBag.phanloaitaikhoan = LayDanhSachPhanLoaiTaiKhoan();
            return View(account);
        }

        public int CheckLoiGiangVienDaTonTai(int? idaccount)
        {
            using (TaiKhoanBusiness bs = new TaiKhoanBusiness())
            {
                return bs.CheckLoiGiangVienDaTonTai(idaccount);
            }
        }

        public int CheckLoiSinhVienDaTonTai(int? idaccount)
        {
            using(TaiKhoanBusiness bs = new TaiKhoanBusiness())
            {
                return bs.CheckLoiSinhVienDaTonTai(idaccount);
            }
        }

        public int CheckLoiSinhVienDaTonTai_KetQuaHocTap(int? idaccount)
        {
            using (TaiKhoanBusiness bs = new TaiKhoanBusiness())
            {
                return bs.CheckLoiSinhVienDaTonTai_KetQuaHocTap(idaccount);
            }
        }

        //public ActionResult LayDanhSachTaiKhoan(dynamic dynamic)
        //{
        //    using (TaiKhoanBusiness bs = new TaiKhoanBusiness())
        //    {
        //        return View("Index", bs.LayDanhSachTaiKhoan());
        //    }

        //    using (KhoaDaoTaoBusiness bs = new KhoaDaoTaoBusiness())
        //    {
        //        return View("Index", bs.LayDanhSachKhoaDaoTao());
        //    }
        //}

        //public ActionResult LayTaiKhoan(int id)
        //{
        //    using (TaiKhoanBusiness bs = new TaiKhoanBusiness())
        //    {
        //        AccountDTO acc = bs.LayTaiKhoan(id);
        //        return View("Index",acc);
        //    }
        //}


        //public ActionResult ThemTaiKhoan(AccountDTO taikhoan)
        //{
        //    using (TaiKhoanBusiness bs = new TaiKhoanBusiness())
        //    {
        //        return View("Index",bs.ThemTaiKhoan(taikhoan));
        //    }
        //}

        //public ActionResult XoaTaiKhoan(int id)
        //{
        //    using (TaiKhoanBusiness bs = new TaiKhoanBusiness())
        //    {
        //        var result = bs.XoaTaiKhoan(id);
        //        if (result)
        //            return View("Successed");
        //        else
        //            return View("Failed");
        //    }
        //}

        //public ActionResult SuaTaiKhoan(AccountDTO taikhoan)
        //{
        //    using (TaiKhoanBusiness bs = new TaiKhoanBusiness())
        //    {
        //        return View("Index", bs.SuaTaiKhoan(taikhoan));
        //    }
        //}
    }
}