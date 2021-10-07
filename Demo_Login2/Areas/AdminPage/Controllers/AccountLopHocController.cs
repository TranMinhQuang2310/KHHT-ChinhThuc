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
    public class AccountLopHocController : Controller
    {
        //Get: LayDanhSachChuNhiem
        public ActionResult Index()
        {
            var lstacclop = this.LayDanhSachAccountLopHocTheoKhoaDT(0);
            ViewBag.tengiaovien = LayDanhSachTaiKhoan();
            ViewBag.phanloaitaikhoan = LayDanhSachTaiKhoan();
            ViewBag.phanloailophoc = LayDanhSachLopHoc();

            var listkhoaDT = LayDanhSachKhoaDaoTao();
            listkhoaDT.Insert(0, new KhoaDaoTaoDTO
            {
                ID = 0,
                TenKhoaDaoTao = "Tất Cả Khóa"
            });
            ViewData["khoaDT"] = new SelectList(listkhoaDT, "ID", "TenKhoaDaoTao");
            
            return View(lstacclop);
        }

        //Post :LayDanhSachChuNhiem
        [HttpPost]
        public ActionResult Index(int id)
        {
            var lstacclop = this.LayDanhSachAccountLopHocTheoKhoaDT(id);
            ViewBag.tengiaovien = LayDanhSachTaiKhoan();
            ViewBag.phanloaitaikhoan = LayDanhSachTaiKhoan();
            ViewBag.phanloailophoc = LayDanhSachLopHoc();
            var listkhoaDT = this.LayDanhSachKhoaDaoTao();
            listkhoaDT.Insert(0, new KhoaDaoTaoDTO
            {
                ID = 0,
                TenKhoaDaoTao = "Tất cả Khóa"
            });
            ViewData["khoaDT"] = new SelectList(listkhoaDT, "ID", "TenKhoaDaoTao");
            return View(lstacclop);
        }

            public List<KhoaDaoTaoDTO> LayDanhSachKhoaDaoTao()
        {
            using (KhoaDaoTaoBusiness bs = new KhoaDaoTaoBusiness())
            {
                return bs.LayDanhSachKhoaDaoTao();
            }
        }
        public List<AccountLopHocDTO> LayDanhSachAccountLopHocTheoKhoaDT(int id)
        {
            using (AccountLopHocBusiness bs = new AccountLopHocBusiness())
            {
                return bs.LayDanhSachAccountLopHocTheoKhoaDT(id);
            }
        }
        public List<AccountLopHocDTO> LayDanhSachAccountLopHoc()
        {
            using (AccountLopHocBusiness bs = new AccountLopHocBusiness())
            {
                return bs.LayDanhSachAccountLopHoc();
            }
        }

        public List<AccountDTO> LayDanhSachTaiKhoan_GiangVienDeChon(int id)
        {
            using (TaiKhoanBusiness bs = new TaiKhoanBusiness())
            {
                return bs.LayDanhSachTaiKhoan_GiangVienDeChon(id);
            }
        }

        public ActionResult Create_ThayDoiTaiKhoan(int id)
        {
            HttpCookie idAccount = HttpContext.Request.Cookies.Get("idAccount");
            HttpCookie idLopHoc = HttpContext.Request.Cookies.Get("idLopHoc");

            ViewData["tengiaovien"] = new SelectList(LayDanhSachTaiKhoan_GiangVien(), "ID", "HoVaTen", idAccount.Value);
            ViewData["lophoc"] = new SelectList(LayDanhSachLopHoc(), "ID", "TenLop", idLopHoc.Value);
            ViewData["account"] = new SelectList(LayDanhSachTaiKhoan_GiangVienDeChon(id), "ID", "MailVL");

            return View("Create");

        }

        //Get : TaoChuNhiem
        public ActionResult Create()
        {
            var lstAccount = LayDanhSachTaiKhoan_GiangVien();

            ViewData["tengiaovien"] = new SelectList(LayDanhSachTaiKhoan_GiangVien(), "ID", "HoVaTen");
            ViewData["account"] = new SelectList(LayDanhSachTaiKhoan_GiangVienDeChon(lstAccount[0].ID), "ID", "MailVL");
            ViewData["lophoc"] = new SelectList(LayDanhSachLopHoc(), "ID", "TenLop");
            return View();
        }
        //Post :TaoChuNhiem
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AccountLopHocDTO acclop)
        {

            
            var findlop = LayLopHocDaTonTai(acclop.IDLopHoc);
            var findgv = LayGiangVienDaTonTai(acclop.IDAccount);
            Response.Cookies.Remove("idAccount");
            Response.Cookies.Remove("idLopHoc");
            
            if (findgv > 0)
            {
                ViewBag.Errorgv = "Giảng viên đã là chủ nhiệm lớp khác";               
                ViewData["tengiaovien"] = new SelectList(LayDanhSachTaiKhoan_GiangVien(), "ID", "HoVaTen");
                ViewData["account"] = new SelectList(LayDanhSachTaiKhoan_GiangVienDeChon(Convert.ToInt32(acclop.IDAccount)), "ID", "MailVL");
                ViewData["lophoc"] = new SelectList(LayDanhSachLopHoc(), "ID", "TenLop");
                return View();
            }else if(findlop > 0)
            {
                ViewBag.Errorlop = "Lớp đã có chủ nhiệm";
                ViewData["tengiaovien"] = new SelectList(LayDanhSachTaiKhoan_GiangVien(), "ID", "HoVaTen");
                ViewData["account"] = new SelectList(LayDanhSachTaiKhoan_GiangVienDeChon(Convert.ToInt32(acclop.IDAccount)), "ID", "MailVL");
                ViewData["lophoc"] = new SelectList(LayDanhSachLopHoc(), "ID", "TenLop");
                return View();
            }
            else
            {
                ThemAccountLopHoc(acclop);
                return RedirectToAction("Index");
            }
            
        }

        public int LayGiangVienDaTonTai(int? id)
        {
            using(AccountLopHocBusiness bs = new AccountLopHocBusiness())
            {
                return bs.LayGiangVienDaTonTai(id);
            }
        }

        public int LayLopHocDaTonTai(int? id)
        {
            using (AccountLopHocBusiness bs = new AccountLopHocBusiness())
            {
                return bs.LayLopHocDaTonTai(id);
            }
        }
        public List<AccountDTO> LayDanhSachTaiKhoan_GiangVien()
        {
            using(TaiKhoanBusiness bs = new TaiKhoanBusiness())
            {
                return bs.LayDanhSachTaiKhoan_GiangVien();
            }
        }
        public List<AccountDTO> LayDanhSachTaiKhoan()
        {
            using (TaiKhoanBusiness bs = new TaiKhoanBusiness())
            {
                return bs.LayDanhSachTaiKhoan();
            }
        }

        public List<LopHocDTO> LayDanhSachLopHoc()
        {
            using (LopHocBusiness bs = new LopHocBusiness())
            {
                return bs.LayDanhSachLopHoc();
            }
        }

        public bool ThemAccountLopHoc(AccountLopHocDTO acclop)
        {
            using (AccountLopHocBusiness bs = new AccountLopHocBusiness())
            {
                return bs.ThemAccountLopHoc(acclop);
            }
        }

        public ActionResult Edit_ThayDoiTaiKhoan(int id)
        {
            HttpCookie idAccount = HttpContext.Request.Cookies.Get("idAccount");
            HttpCookie idLopHoc = HttpContext.Request.Cookies.Get("idLopHoc");

            ViewData["tengiaovien"] = new SelectList(LayDanhSachTaiKhoan_GiangVien(), "ID", "HoVaTen", idAccount.Value);
            ViewData["lophoc"] = new SelectList(LayDanhSachLopHoc(), "ID", "TenLop", idLopHoc.Value);
            ViewData["account"] = new SelectList(LayDanhSachTaiKhoan_GiangVienDeChon(id), "ID", "MailVL");
            return View("Edit");
        }

        //Get: SuaChuNhiem
        public ActionResult Edit(int id)
        {
            HttpCookie idEdit = new HttpCookie("idEdit");
            idEdit.Value = id.ToString();
            idEdit.Expires = DateTime.Now.AddDays(1);
            Response.SetCookie(idEdit);

            AccountLopHocDTO acclop = LayAccountLopHoc(id);
            ViewData["tengiaovien"] = new SelectList(LayDanhSachTaiKhoan_GiangVien(), "ID", "HoVaTen");
            ViewData["account"] = new SelectList(LayDanhSachTaiKhoan_GiangVienDeChon(Convert.ToInt32(acclop.IDAccount)), "ID", "MailVL");
            ViewData["lophoc"] = new SelectList(LayDanhSachLopHoc(), "ID", "TenLop");
            
            return View(acclop);
        }

        //Post : SuaChuNhiem
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(AccountLopHocDTO acclop)
        {
            var findlop = LayLopHocDaTonTai(acclop.IDLopHoc);
            var findgv = LayGiangVienDaTonTai(acclop.IDAccount);
            Response.Cookies.Remove("idAccount");
            Response.Cookies.Remove("idLopHoc");
            if ((findgv == acclop.ID || findgv == 0) && (findlop == acclop.ID || findlop == 0))
            {
                SuaAccountLopHoc(acclop);
                return RedirectToAction("Index");
            }
            else
            {
                if (findgv != acclop.ID && findgv > 0)
                {
                    ViewBag.Errorgv = "Giảng viên đã là chủ nhiệm lớp khác";
                    ViewData["tengiaovien"] = new SelectList(LayDanhSachTaiKhoan_GiangVien(), "ID", "HoVaTen");
                    ViewData["account"] = new SelectList(LayDanhSachTaiKhoan_GiangVienDeChon(Convert.ToInt32(acclop.IDAccount)), "ID", "MailVL");
                    ViewData["lophoc"] = new SelectList(LayDanhSachLopHoc(), "ID", "TenLop");
                    return View();
                }
                else if (findlop != acclop.ID && findlop > 0)
                {
                    ViewBag.Errorlop = "Lớp đã có chủ nhiệm";
                    ViewData["tengiaovien"] = new SelectList(LayDanhSachTaiKhoan_GiangVien(), "ID", "HoVaTen");
                    ViewData["account"] = new SelectList(LayDanhSachTaiKhoan_GiangVienDeChon(Convert.ToInt32(acclop.IDAccount)), "ID", "MailVL");
                    ViewData["lophoc"] = new SelectList(LayDanhSachLopHoc(), "ID", "TenLop");
                    return View();
                }
                else
                {
                    return View();
                }
            }
        }

        public AccountLopHocDTO LayAccountLopHoc(int id)
        {
            using (AccountLopHocBusiness bs = new AccountLopHocBusiness())
            {
                return bs.LayAccountLopHoc(id);
            }
        }
        public bool SuaAccountLopHoc(AccountLopHocDTO acclop)
        {
            using (AccountLopHocBusiness bs = new AccountLopHocBusiness())
            {
                return bs.SuaAccountLopHoc(acclop);
            }
        }

        //XoaChuNhiem
        public async Task<ActionResult> Delete(int id)
        {
            var output = XoaAccountLopHoc(id);
            if (output)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View("fail");
            }
        }

        public bool XoaAccountLopHoc(int id)
        {
            using (AccountLopHocBusiness bs = new AccountLopHocBusiness())
            {
                return bs.XoaAccountLopHoc(id);
            }
        }

        //Xemchitiet
        public ActionResult Details(int id)
        {
            var acclop = LayAccountLopHoc(id);
            ViewBag.tengiaovien = LayDanhSachTaiKhoan();
            ViewBag.taikhoan = LayDanhSachTaiKhoan();
            ViewBag.lophoc = LayDanhSachLopHoc();
            return View(acclop);
        }

    }
}