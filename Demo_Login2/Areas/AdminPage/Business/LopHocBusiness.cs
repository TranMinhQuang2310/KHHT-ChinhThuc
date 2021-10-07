using Demo_Login2.Models.DTO;
using Demo_Login2.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Demo_Login2.Areas.AdminPage.Business
{
    public class LopHocBusiness : BaseBusiness
    {
        public LopHocDTO LayLopHoc(int id)
        {
            try
            {
                var lophoc = model.LopHocs.Where(s => s.ID == id).Select(s => new LopHocDTO
                {
                    ID = s.ID,
                    IDKhoaDaoTao = s.IDKhoaDaoTao,
                    TenLop = s.TenLop,
                    GhiChu = s.GhiChu
                }).FirstOrDefault();
                return lophoc;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public int LayLopHocDaTonTai(string tenlop)
        {
            try
            {
                return model.LopHocs.Where(s => s.TenLop == tenlop).Select(s => s.ID).FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int CheckLoiLopHocDaTonTai(int? id)
        {
            try
            {
                return model.AccountLopHocs.Where(s => s.IDLopHoc == id).Select(s => s.ID).FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<LopHocDTO> LayDanhSachLopHoc()
        {
            try
            {
                var listlophoc = model.LopHocs.Select(s => new LopHocDTO
                {
                    ID = s.ID,
                    IDKhoaDaoTao = s.IDKhoaDaoTao,
                    TenLop = s.TenLop,
                    GhiChu = s.GhiChu
                }).ToList();
                return listlophoc;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<LopHocDTO> LayDanhSachLopHocTheoKhoaDaoTao(int id)
        {
            try
            {
                if(id == 0)
                {
                    var listlophoc = model.LopHocs.Select(s => new LopHocDTO
                    {
                        ID = s.ID,
                        IDKhoaDaoTao = s.IDKhoaDaoTao,
                        TenLop = s.TenLop,
                        GhiChu = s.GhiChu
                    }).ToList();
                    return listlophoc;
                    
                }else
                {
                    var listlophoc = model.LopHocs.Where(s => s.KhoaDaoTao.ID == id).Select(s => new LopHocDTO
                    {
                        ID = s.ID,
                        IDKhoaDaoTao = s.IDKhoaDaoTao,
                        TenLop = s.TenLop,
                        GhiChu = s.GhiChu
                    }).ToList();
                    return listlophoc;
                }
            }catch(Exception ex)
            {
                throw ex;
            }
        }

        public bool ThemLopHoc(LopHocDTO lophoc)
        {
            try
            {
                var newlophoc = new LopHoc();
                newlophoc.ID = lophoc.ID;
                newlophoc.IDKhoaDaoTao = lophoc.IDKhoaDaoTao;
                newlophoc.TenLop = lophoc.TenLop;
                newlophoc.GhiChu = lophoc.GhiChu;

                model.LopHocs.Add(newlophoc);
                model.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool XoaLopHoc(int id)
        {
            try
            {
                var lophoc = model.LopHocs.Where(s => s.ID == id).FirstOrDefault();
                model.LopHocs.Remove(lophoc);
                model.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SuaLopHoc(LopHocDTO lophoc)
        {
            try
            {
                var lophocs = model.LopHocs.Where(s => s.ID == lophoc.ID).FirstOrDefault();
                lophocs.ID = lophoc.ID;
                lophocs.IDKhoaDaoTao = lophoc.IDKhoaDaoTao;
                lophocs.TenLop = lophoc.TenLop;
                lophocs.GhiChu = lophoc.GhiChu;

                model.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}