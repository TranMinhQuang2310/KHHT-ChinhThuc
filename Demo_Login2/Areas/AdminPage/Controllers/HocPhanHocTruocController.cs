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
    public class HocPhanHocTruocController : Controller
    {
        //LayDanhSachHocPhanHocTruoc
        public ActionResult Index()
        {
            var lsthocphanHT = LayDanhSachHocPhanHocTruoc();
            ViewBag.monhoc = LayDanhSachMonHoc();
            return View(lsthocphanHT);
        }
        public List<MonHocDTO> LayDanhSachMonHoc()
        {
            using (MonHocBusiness bs = new MonHocBusiness())
            {
                return bs.LayDanhSachMonHoc();
            }
        }

        public List<HocPhanHocTruocDTO> LayDanhSachHocPhanHocTruoc()
        {
            using (HocPhanHocTruocBusiness bs = new HocPhanHocTruocBusiness())
            {
                return bs.LayDanhSachHocPhanHocTruoc();
            }
        }

        public ActionResult Create_ThayDoiMonHoc(int id)
        {
            HttpCookie idMonHoc = HttpContext.Request.Cookies.Get("idMonHoc");
            ViewData["monhoc"] = new SelectList(LayDanhSachMonHoc(), "ID", "TenMonHoc", idMonHoc.Value);
            ViewData["monhochoctruoc"] = new SelectList(LayDanhSachMonHocHocTruoc(id), "ID", "TenMonHoc");
            return View("Create");
        }

        //Get : TaoHocPhanHocTruoc
        public ActionResult Create()
        {
            var lstmonhoc = LayDanhSachMonHoc();
            ViewData["monhoc"] = new SelectList(LayDanhSachMonHoc(), "ID", "TenMonHoc");
            ViewData["monhochoctruoc"] = new SelectList(LayDanhSachMonHocHocTruoc(lstmonhoc[0].ID), "ID", "TenMonHoc");
            return View();
        }

        //Post : TaoHocPhanHocTruoc
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(HocPhanHocTruocDTO hocphanHT)
        {
            var findmonhoc = CheckLoiMonHocHocTruocTheoMonHocDaTonTai(hocphanHT.IDMonHoc, hocphanHT.IDMonHocHocTruoc);
            if(findmonhoc > 0)
            {
                ViewBag.Error = "Môn Học Học Trước đã trong Môn Học này";
                var lstmonhoc = LayDanhSachMonHoc();
                ViewData["monhoc"] = new SelectList(LayDanhSachMonHoc(), "ID", "TenMonHoc");
                ViewData["monhochoctruoc"] = new SelectList(LayDanhSachMonHocHocTruoc(Convert.ToInt32(hocphanHT.IDMonHoc)), "ID", "TenMonHoc");
                return View();

            }else
            {
                ThemHocPhanHocTruoc(hocphanHT);
                return RedirectToAction("Index");
            }
        }

        public int CheckLoiMonHocHocTruocTheoMonHocDaTonTai(int? idMonHoc,int? idMonHocHocTruoc)
        {
            using (MonHocBusiness bs = new MonHocBusiness())
            {
                return bs.CheckLoiMonHocHocTruocTheoMonHocDaTonTai(idMonHoc, idMonHocHocTruoc);
            }
        }

        public List<MonHocDTO> LayDanhSachMonHocHocTruoc(int id)
        {
            using(MonHocBusiness bs = new MonHocBusiness())
            {
                return bs.LayDanhSachMonHocHocTruoc(id);
            }
        }

        public int LayHocPhanHocTruocDaTonTai(int? id)
        {
            using (HocPhanHocTruocBusiness bs = new HocPhanHocTruocBusiness())
            {
                return bs.LayHocPhanHocTruocDaTonTai(id);
            }
        }
        public bool ThemHocPhanHocTruoc(HocPhanHocTruocDTO hocphanHT)
        {
            using(HocPhanHocTruocBusiness bs = new HocPhanHocTruocBusiness())
            {
                return bs.ThemHocPhanHocTruoc(hocphanHT);
            }
        }

        public ActionResult Edit_ThayDoiMonHoc(int id)
        {
            ViewData["monhoc"] = new SelectList(LayDanhSachMonHoc(), "ID", "TenMonHoc", id);
            ViewData["monhochoctruoc"] = new SelectList(LayDanhSachMonHocHocTruoc(id), "ID", "TenMonHoc");
            return View("Edit");
        }

        //Get : SuaHocPhanHocTruoc
        public ActionResult Edit(int id)
        {
            HttpCookie idEdit = new HttpCookie("idEdit");
            idEdit.Value = id.ToString();
            idEdit.Expires = DateTime.Now.AddDays(1);
            Response.SetCookie(idEdit);

            HocPhanHocTruocDTO hocphanHT = LayHocPhanHocTruoc(id);
            ViewData["monhoc"] = new SelectList(LayDanhSachMonHoc(), "ID", "TenMonHoc");
            ViewData["monhochoctruoc"] = new SelectList(LayDanhSachMonHocHocTruoc(Convert.ToInt32(hocphanHT.IDMonHoc)), "ID", "TenMonHoc");
            return View(hocphanHT);
        }

        //Post : SuaHocPhanHocTruoc
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(HocPhanHocTruocDTO hocphanHT)
        {
            var findmonhoc = CheckLoiMonHocHocTruocTheoMonHocDaTonTai(hocphanHT.IDMonHoc, hocphanHT.IDMonHocHocTruoc);
            if (findmonhoc == hocphanHT.ID || findmonhoc == 0)
            {
                SuaHocPhanHocTruoc(hocphanHT);
                return RedirectToAction("Index");
            }
            else if (findmonhoc != hocphanHT.ID && findmonhoc > 0)
            {
                ViewBag.Error = "Môn Học Học Trước đã trong Môn Học này";
                var lstmonhoc = LayDanhSachMonHoc();
                ViewData["monhoc"] = new SelectList(LayDanhSachMonHoc(), "ID", "TenMonHoc");
                ViewData["monhochoctruoc"] = new SelectList(LayDanhSachMonHocHocTruoc(Convert.ToInt32(hocphanHT.IDMonHoc)), "ID", "TenMonHoc");
                return View();
            }
            else
            {
                return View();
            }
        }

        public HocPhanHocTruocDTO LayHocPhanHocTruoc(int id)
        {
            using(HocPhanHocTruocBusiness bs = new HocPhanHocTruocBusiness())
            {
                return bs.LayHocPhanHocTruoc(id);
            }
        }

        public bool SuaHocPhanHocTruoc(HocPhanHocTruocDTO hocphanHT)
        {
            using(HocPhanHocTruocBusiness bs = new HocPhanHocTruocBusiness())
            {
                return bs.SuaHocPhanHocTruoc(hocphanHT);
            }
        }

        //XoaHocPhanHocTruoc
        public async Task<ActionResult> Delete(int id)
        {
            var findhocphan = CheckLoiHocPhanHocTruocDaTonTai(id);
            if (findhocphan > 0)
            {
                TempData["error"] = "lỗi";
                return RedirectToAction("Index");
            }
            else
            {
                var output = XoaHocPhanHocTruoc(id);
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

        public int CheckLoiHocPhanHocTruocDaTonTai(int? id)
        {
            using (HocPhanHocTruocBusiness bs = new HocPhanHocTruocBusiness())
            {
                return bs.CheckLoiHocPhanHocTruocDaTonTai(id);
            }
        }

        public bool XoaHocPhanHocTruoc(int id)
        {
            using(HocPhanHocTruocBusiness bs = new HocPhanHocTruocBusiness())
            {
                return bs.XoaHocPhanHocTruoc(id);
            }
        }

        //XemChiTietHocPhanHocTruoc
        public ActionResult Details(int id)
        {
            var hocphanHT = LayHocPhanHocTruoc(id);
            ViewBag.monhoc = LayDanhSachMonHoc();
            return View(hocphanHT);
        }
    }
}