using Demo_Login2.Areas.AdminPage.Business;
using Demo_Login2.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Demo_Login2.Areas.SinhVienPage.Controllers
{
    public class ChuongTrinhDaoTaoController : Controller
    {
        // GET: SinhVienPage/ChuongTrinhDaoTao
        public ActionResult Index()
        {
            HttpCookie IDKhoa = HttpContext.Request.Cookies.Get("idKhoaDaoTao");
            var idKhoaDT = Convert.ToInt32(IDKhoa.Value);
            var lstctrdaotao = this.LayDanhSachChuongTrinhDaoTaoTheoKhoa(idKhoaDT);
            ViewBag.HocKi = LayDanhSachHocKi();
            ViewBag.PhanLoaiMonHoc = LayDanhSachPhanLoaiMonHoc();
            ViewBag.MonHoc = LayDanhSachMonHoc();

            ViewData["khoaDT"] = new SelectList(LayDanhSachKhoaDaoTao_SinhVien(idKhoaDT), "ID", "TenKhoaDaoTao");
            return View(lstctrdaotao);
        }

        public List<KhoaDaoTaoDTO> LayDanhSachKhoaDaoTao_SinhVien(int khoadt)
        {
            using (KhoaDaoTaoBusiness bs = new KhoaDaoTaoBusiness())
            {
                return bs.LayDanhSachKhoaDaoTao_SinhVien(khoadt);
            }
        }

        public List<ChuongTrinhDaoTaoDTO> LayDanhSachChuongTrinhDaoTaoTheoKhoa(int id)
        {
            using (ChuongTrinhDaoTaoBusiness bs = new ChuongTrinhDaoTaoBusiness())
            {
                return bs.LayDanhSachChuongTrinhDaoTaoTheoKhoa(id);
            }
        }

        public List<HocKiDTO> LayDanhSachHocKi()
        {
            using (HocKiBusiness bs = new HocKiBusiness())
            {
                return bs.LayDanhSachHocKi();
            }
        }

        public List<PhanLoaiMonHocDTO> LayDanhSachPhanLoaiMonHoc()
        {
            using (PhanLoaiMonHocBusiness bs = new PhanLoaiMonHocBusiness())
            {
                return bs.LayDanhSachPhanLoaiMonHoc();
            }
        }

        public List<MonHocDTO> LayDanhSachMonHoc()
        {
            using (MonHocBusiness bs = new MonHocBusiness())
            {
                return bs.LayDanhSachMonHoc();
            }
        }

        public List<KhoaDaoTaoDTO> LayDanhSachKhoaDaoTao()
        {
            using (KhoaDaoTaoBusiness bs = new KhoaDaoTaoBusiness())
            {
                return bs.LayDanhSachKhoaDaoTao();
            }
        }
    }
}