using bank2.Models;
using Castle.ActiveRecord.Queries;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace bank2.Controllers
{
    public class LoginController : Controller
    {
        log4net.ILog Log = log4net.LogManager.GetLogger(typeof(LoginController));

        #region 安全验证
        /// <summary>
        /// 查询用户是否有权限
        /// 有true
        /// </summary>
        /// <returns></returns>
        public AdminInfo authrize()
        {
            #region 用于测试可删除的

            //AdminInfo user1 = AdminInfo.Find(1);
            //时效
            HttpContext.Session.Timeout = 60 * 24 * 10;
            //密码正确 设置session
            //HttpContext.Session["UserInfo"] = user1;

            #endregion
            if (HttpContext.Session["UserInfo"] != null)
            {
                AdminInfo UserInfo = (AdminInfo)HttpContext.Session["UserInfo"];
                if (UserInfo == null)
                {
                    return null;
                }
                return UserInfo;
            }
            else
            {
                return null;
            }
        }
        #endregion

        // GET: Login
        public ActionResult Login()
        {
            return View();
        }
        /// <summary>
        /// 登录的方法
        /// </summary>
        /// <returns>
        ///     OK 登录成功
        ///     NOTFOUNT 登录失败
        /// </returns>
        [HttpPost]
        public string LoginEvn(FormCollection fc) {

            JObject msg = new JObject();
            #region 获取数据

            string username = fc["username"];
            string password = fc["password"];

            #endregion
            try
            {
                #region 查询数据
                //
                string hql = "SELECT NEW AdminInfo(a.id,a.Username,a.DptID,a.CmpID,a.WorkID,a.Password,a.RoleCmp,a.Role,a.RoleConfig,a.NickName,a.Userid,a.SiteConfig) FROM AdminInfo a WHERE a.Username=? and a.Password=?";
                SimpleQuery<AdminInfo> AdQuery = new SimpleQuery<AdminInfo>(hql, username, password);
                AdminInfo[] AdBeans = AdQuery.Execute();

                #endregion

                #region 返回数据

                if (AdBeans.Length > 0)
                {
                    //时效
                    HttpContext.Session.Timeout = 60 * 24 * 10;
                    //密码正确 设置session
                    HttpContext.Session["UserInfo"] = AdBeans[0];
                    msg.Add("msg","OK");
                }
                else
                {
                    //密码错误 
                    msg.Add("msg", "NOTFOUNT");
                }

                #endregion

                return msg.ToString();
            }
            catch (Exception _e)
            {
                Log.Error("Login LoginEvn 出错", _e);
                throw;
            }
        }
        /// <summary>
        /// 退出登录
        /// </summary>
        /// <param name="fc"></param>
        /// <returns></returns>
        [HttpPost]
        public string LoginOut(FormCollection fc) {
            HttpContext.Session.Clear();
            JObject msg = new JObject();
            msg.Add("msg","OK");
            return msg.ToString();
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string PasEdit(FormCollection fc) {
            JObject msg = new JObject();
            AdminInfo user = authrize();
            #region 验证
            if (user == null)
            {
                msg.Add("msg", "refuse");
                return msg.ToString();
            }
            #endregion
            try
            {
                #region 获取数据
                string Pas_Password = fc["Pas_Password"];
                string Pas_Password2 = fc["Pas_Password2"];
                string Pas_Password3 = fc["Pas_Password3"];
                #endregion
                #region 转化数据
                #endregion
                #region 处理请求
                if (!user.Password.Equals(Pas_Password))
                {
                    msg.Add("msg", "NOTFOUNT");
                }
                else
                {
                    user.Password = Pas_Password2;
                    user.UpdateAndFlush();
                    msg.Add("msg", "OK");
                }
                #endregion
                #region 返回数据
                //JsonConvert.SerializeObject(bean).ToString().Replace("\r\n", "")
                return msg.ToString();
                #endregion
            }
            catch (Exception _e)
            {
                Log.Error("Login/PasEdit", _e);
                msg.Add("msg", "error");
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fc"></param>
        /// <returns></returns>
        [HttpPost]
        public string getUserInfo(FormCollection fc)
        {
            JObject msg = new JObject();
            AdminInfo user = authrize();
            #region 验证
            if (user == null)
            {
                msg.Add("msg", "refuse");
                return msg.ToString();
            }
            #endregion
            try
            {
                #region 获取数据
                #endregion
                #region 转化数据
                #endregion
                #region 处理请求
                #endregion
                #region 返回数据
                //JsonConvert.SerializeObject(bean).ToString().Replace("\r\n", "")
                msg.Add("Username", user.Username);
                msg.Add("WorkID", user.WorkID);
                msg.Add("NickName", user.NickName);
                msg.Add("msg", "OK");
                return msg.ToString();
                #endregion
            }
            catch (Exception _e)
            {
                Log.Error("Login/getUserInfo", _e);
                msg.Add("msg", "error");
                throw;
            }

        }

    }
}