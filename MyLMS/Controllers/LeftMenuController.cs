using MyLMS.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UtilityClass;

namespace MyLMS.Controllers
{
    public class LeftMenuController : Controller
    {
        // GET: LeftMenu
        dynamic VarSubMenuList;
        public ActionResult LeftMenu()
        {
            return View();
        }

        [HttpGet]
        public string GetMenus()
        {
            List<MenuSubMenu> MenuSubMenuObj = new List<MenuSubMenu>();

            SqlParameter[] MenuParam = new SqlParameter[1];
            MenuParam[0] = new SqlParameter("@RoleID", SqlDbType.Int);
            MenuParam[0].Value = Convert.ToInt32(Session["RoleID"]);
            DataTable MenusDT = DAL.GetDataTable("GetMenu", MenuParam);

            SqlParameter[] SParam = new SqlParameter[2];
            List<Menu> MenuList = new List<Menu>();


            for (int i = 0; i < MenusDT.Rows.Count; i++)
            {
                Menu MenuObj = new Menu();
                MenuObj.MenuID = Convert.ToInt32(MenusDT.Rows[i]["MenuID"]);
                MenuObj.MenuName = MenusDT.Rows[i]["MenuName"].ToString();
                MenuObj.MenuIconClass = MenusDT.Rows[i]["MenuIconClass"].ToString();
                MenuObj.MenuURL = MenusDT.Rows[i]["MenuURL"].ToString();
                MenuObj.MenuOrder = Convert.ToInt32(MenusDT.Rows[i]["MenuOrder"]);
                MenuList.Add(MenuObj);

                SParam[0] = new SqlParameter("@MenuID", SqlDbType.Int);
                SParam[0].Value = Convert.ToInt32(MenusDT.Rows[i]["MenuID"]);
                SParam[1] = new SqlParameter("@RoleID", SqlDbType.Int);
                SParam[1].Value = Convert.ToInt32(Session["RoleID"]);

                DataTable SubMenuDT = DAL.GetDataTable("GetSubMenu", SParam);
                List<SubMenu> SubMenuList = new List<SubMenu>();
                for (int j = 0; j < SubMenuDT.Rows.Count; j++)
                {
                    SubMenu SubMenuObj = new SubMenu();
                    SubMenuObj.SubMenuID = Convert.ToInt32(Convert.IsDBNull(SubMenuDT.Rows[j]["SubMenuID"]) ? "0" : SubMenuDT.Rows[j]["SubMenuID"]);
                    SubMenuObj.SubMenuName = SubMenuDT.Rows[j]["SubMenuName"].ToString();
                    SubMenuObj.SubMenuIconClass = SubMenuDT.Rows[j]["SubMenuIconClass"].ToString();
                    SubMenuObj.SubMenuURL = SubMenuDT.Rows[j]["SubMenuURL"].ToString();
                    SubMenuObj.SubMenuOrder = Convert.ToInt32(Convert.IsDBNull(SubMenuDT.Rows[j]["SubMenuOrder"]) ? "0" : SubMenuDT.Rows[j]["SubMenuOrder"]);
                    SubMenuList.Add(SubMenuObj);

                    VarSubMenuList = SubMenuList;
                }

                MenuSubMenuObj.Add(new MenuSubMenu
                {
                    Menu = MenuObj,
                    SubMenu = VarSubMenuList
                });
            }

            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(MenuSubMenuObj);
            return JSONString;
        }
    }
}