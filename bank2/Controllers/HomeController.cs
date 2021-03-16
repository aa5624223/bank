using bank2.Models;
using Castle.ActiveRecord.Queries;
using Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace bank2.Controllers
{
    public class HomeController : Controller
    {
        log4net.ILog Log = log4net.LogManager.GetLogger(typeof(HomeController));

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

        public AdminInfo RefuseRedirect()
        {
            AdminInfo user = authrize();
            if (user == null)
            {
                //HttpContext.Response.Redirect("~/Login/Login");
                return null;
            }
            else
            {
                return user;
            }
        }

        /// <summary>
        /// 获取用户权限
        /// 并且设置的sideBar 方便框架layout使用
        /// ViewBag.SideBar
        /// </summary>
        /// <returns></returns>
        public List<SiteFunction> getUserAuth()
        {
            AdminInfo user = (AdminInfo)HttpContext.Session["UserInfo"];
            //用户的权限
            List<SiteFunction> beans = SiteFunction.getUserRole(user.SiteConfig).ToList();
            
            //获取侧边栏树
            List<SiteFunction> beans2 = SiteFunction.getSideBar(beans);
            ViewBag.SideBar = JsonConvert.SerializeObject(beans2).ToString().Replace("\r\n", ""); ;
            return beans;
        }

        #endregion

        #region 返回页面

        /// <summary>
        /// 主页
        /// </summary> 
        /// <returns></returns>
        public ActionResult Index()
        {
            #region 验证

            AdminInfo UserInfo = RefuseRedirect();
            if (UserInfo == null)
            {
                return View("../Login/Login");
            }

            #endregion

            #region 获取权限
            //获取用户权限
            List<SiteFunction> auth = getUserAuth();
            #endregion

            #region 设置数据

            #endregion

            //根据用户的Session 权限显示对应的页面
            return View();
        }

        /// <summary>
        /// 我的桌面
        /// </summary>
        /// <re turns></returns>
        public ActionResult Destop()
        {
            #region 验证

            AdminInfo UserInfo = RefuseRedirect();
            if (UserInfo == null)
            {
                return View("../Login/Login");
            }

            #endregion

            #region 获取权限

            //获取用户权限
            List<SiteFunction> auth = getUserAuth();
            #endregion
            return View();

        }

        /// <summary>
        /// 企业信息
        /// </summary>
        /// <returns></returns>
        public ActionResult CompanyInfo()
        {

            #region 验证
            AdminInfo UserInfo = RefuseRedirect();
            if (UserInfo == null)
            {
                return View("../Login/Login");
            }
            #endregion

            #region 获取权限

            //获取用户权限
            List<SiteFunction> auth = getUserAuth();

            #endregion

            #region 查询数据
            //查找公司类别
            string hql = "SELECT NEW TypeInfo(a.id,a.TableName,a.TypeName,a.ParentID,a.SortID,a.Userid) FROM TypeInfo a WHERE a.TableName='CompanyInfo'";
            SimpleQuery<TypeInfo> Query = new SimpleQuery<TypeInfo>(hql);
            TypeInfo[] TBeans = Query.Execute();
            #endregion

            #region 整理数据

            #endregion

            #region 设置数据

            ViewBag.TypeList = TBeans;

            #endregion

            return View();
        }

        /// <summary>
        /// 银行信息
        /// </summary>
        /// <returns></returns>
        public ActionResult BankInfo()
        {
            #region 验证
            AdminInfo UserInfo = RefuseRedirect();
            if (UserInfo == null)
            {
                return View("../Login/Login");
            }
            #endregion
            #region 获取权限
            //获取用户权限
            List<SiteFunction> auth = getUserAuth();
            #endregion
            #region 查询数据

            //查找公司类别
            string hql = "SELECT NEW TypeInfo(a.id,a.TableName,a.TypeName,a.ParentID,a.SortID,a.Userid) FROM TypeInfo a WHERE a.TableName='BankInfo'";
            SimpleQuery<TypeInfo> Query = new SimpleQuery<TypeInfo>(hql);
            TypeInfo[] TBeans = Query.Execute();

            #endregion
            #region 整理数据

            #endregion

            #region 设置数据

            ViewBag.TypeList = TBeans;

            #endregion

            return View();
        }

        /// <summary>
        /// 银行授信
        /// </summary>
        /// <returns></returns>
        public ActionResult BankRightInfo()
        {
            #region 验证
            AdminInfo UserInfo = RefuseRedirect();
            if (UserInfo == null)
            {
                return View("../Login/Login");
            }
            #endregion

            #region 获取权限
            //获取用户权限
            List<SiteFunction> auth = getUserAuth();
            #endregion

            #region 查询数据

            //查找公司 id 名字
            //string hql = "SELECT NEW CompanyInfo(a.id,a.Company) FROM CompanyInfo a,TypeInfo b WHERE a.TypeID=b.id  and a.id in (" + UserInfo.RoleCmp + ") ORDER BY a.SortID";
            string hql = "SELECT NEW CompanyInfo(a.id,a.Company) FROM CompanyInfo a,TypeInfo b WHERE a.TypeID=b.id and a.id in (" + UserInfo.RoleCmp + ") ORDER BY a.SortID";
            
            SimpleQuery<CompanyInfo> CmQuery = new SimpleQuery<CompanyInfo>(hql);
            CompanyInfo[] CmBeans = CmQuery.Execute();
            //查询银行 id 名字
            hql = "SELECT NEW BankInfo(a.id,a.Bank) FROM BankInfo a,TypeInfo b WHERE a.TypeID=b.id ";
            SimpleQuery<BankInfo> Query = new SimpleQuery<BankInfo>(hql);
            BankInfo[] BkBeans = Query.Execute();

            #endregion

            #region 设置数据

            ViewBag.ComList = CmBeans;
            ViewBag.BankList = BkBeans;

            #endregion

            return View();
        }

        /// <summary>
        /// 贷款业务
        /// </summary>
        /// <returns></returns>
        public ActionResult BankServer1()
        {
            #region 验证
            AdminInfo UserInfo = RefuseRedirect();
            if (UserInfo == null)
            {
                return View("../Login/Login");
            }
            #endregion

            #region 获取权限
            //获取用户权限
            List<SiteFunction> auth = getUserAuth();
            #endregion

            #region

            #region 查询数据

            //查找公司 id 名字
            //string hql = "SELECT NEW CompanyInfo(a.id,a.Company) FROM CompanyInfo a,TypeInfo b WHERE a.TypeID=b.id  and a.id in (" + UserInfo.RoleCmp + ") ORDER BY a.SortID";
            string hql = "SELECT NEW CompanyInfo(a.id,a.Company) FROM CompanyInfo a,TypeInfo b WHERE a.TypeID=b.id and a.id in (" + UserInfo.RoleCmp + ") ORDER BY a.SortID";

            SimpleQuery<CompanyInfo> CmQuery = new SimpleQuery<CompanyInfo>(hql);
            CompanyInfo[] CmBeans = CmQuery.Execute();
            //查询银行 id 名字
            hql = "SELECT NEW BankInfo(a.id,a.Bank) FROM BankInfo a,TypeInfo b WHERE a.TypeID=b.id ";
            SimpleQuery<BankInfo> Query = new SimpleQuery<BankInfo>(hql);
            BankInfo[] BkBeans = Query.Execute();

            //查询类别
            string TableName = "CompanyInfo";
            hql = "SELECT NEW TypeInfo(a.id,a.TableName,a.TypeName,a.ParentID,a.SortID,a.Userid) FROM TypeInfo a WHERE a.TableName='" + TableName + "'";
            SimpleQuery<TypeInfo> TQuery = new SimpleQuery<TypeInfo>(hql);
            TypeInfo[] TBeans = TQuery.Execute();


            #endregion

            #endregion

            #region 设置数据

            ViewBag.ComList = CmBeans;
            ViewBag.BankList = BkBeans;
            ViewBag.TypeList = TBeans;

            #endregion

            return View();
        }

        /// <summary>
        /// 承兑业务
        /// </summary>
        /// <returns></returns>
        public ActionResult BankServer2()
        {
            #region 验证

            AdminInfo UserInfo = RefuseRedirect();
            if (UserInfo == null)
            {
                return View("../Login/Login");
            }

            #endregion

            #region 获取权限

            //获取用户权限
            List<SiteFunction> auth = getUserAuth();

            #endregion

            #region 查询数据

            //查找公司 id 名字
            //string hql = "SELECT NEW CompanyInfo(a.id,a.Company) FROM CompanyInfo a,TypeInfo b WHERE a.TypeID=b.id  and a.id in (" + UserInfo.RoleCmp + ") ORDER BY a.SortID";
            string hql = "SELECT NEW CompanyInfo(a.id,a.Company) FROM CompanyInfo a,TypeInfo b WHERE a.TypeID=b.id and a.id in (" + UserInfo.RoleCmp + ") ORDER BY a.SortID";

            SimpleQuery<CompanyInfo> CmQuery = new SimpleQuery<CompanyInfo>(hql);
            CompanyInfo[] CmBeans = CmQuery.Execute();
            //查询银行 id 名字
            hql = "SELECT NEW BankInfo(a.id,a.Bank) FROM BankInfo a,TypeInfo b WHERE a.TypeID=b.id ";
            SimpleQuery<BankInfo> Query = new SimpleQuery<BankInfo>(hql);
            BankInfo[] BkBeans = Query.Execute();

            //查询类别
            string TableName = "CompanyInfo";
            hql = "SELECT NEW TypeInfo(a.id,a.TableName,a.TypeName,a.ParentID,a.SortID,a.Userid) FROM TypeInfo a WHERE a.TableName='" + TableName + "'";
            SimpleQuery<TypeInfo> TQuery = new SimpleQuery<TypeInfo>(hql);
            TypeInfo[] TBeans = TQuery.Execute();

            #endregion

            #region 设置数据

            ViewBag.ComList = CmBeans;
            ViewBag.BankList = BkBeans;
            ViewBag.TypeList = TBeans;

            #endregion

            return View();
        }

        /// <summary>
        /// 授信记录
        /// </summary>
        /// <returns></returns>
        public ActionResult ServerRec1()
        {
            #region 验证

            AdminInfo UserInfo = RefuseRedirect();
            if (UserInfo == null)
            {
                return View("../Login/Login");
            }

            #endregion

            #region 获取权限

            //获取用户权限
            List<SiteFunction> auth = getUserAuth();

            #endregion

            return View();
        }

        /// <summary>
        /// 到期提醒
        /// </summary>
        /// <returns></returns>
        public ActionResult ServerRec2()
        {
            #region 验证

            AdminInfo UserInfo = RefuseRedirect();
            if (UserInfo == null)
            {
                return View("../Login/Login");
            }

            #endregion

            #region 获取权限
            
            //获取用户权限
            List<SiteFunction> auth = getUserAuth();

            #endregion

            #region 查询数据

            //查找公司类别
            string hql = "SELECT NEW TypeInfo(a.id,a.TableName,a.TypeName,a.ParentID,a.SortID,a.Userid) FROM TypeInfo a WHERE a.TableName='CompanyInfo'";
            SimpleQuery<TypeInfo> Query = new SimpleQuery<TypeInfo>(hql);
            TypeInfo[] TBeans = Query.Execute();
            string method = Request.QueryString["method"];
            #endregion

            #region 设置数据
            ViewBag.TypeList = TBeans;
            ViewBag.method = method;
            #endregion

            return View();
        }

        /// <summary>
        /// 借款记录
        /// </summary>
        /// <returns></returns>
        public ActionResult ServerRec3()
        {
            #region 验证

            AdminInfo UserInfo = RefuseRedirect();
            if (UserInfo == null)
            {
                return View("../Login/Login");
            }

            #endregion

            #region 获取数据
            string Company = Request.QueryString["Company"];
            string Methods = Request.QueryString["Methods"];
            #endregion

            #region

            #endregion

            #region 获取权限

            //获取用户权限
            List<SiteFunction> auth = getUserAuth();

            #endregion

            #region 查询数据

            //查找公司 id 名字
            //UserInfo
            //string hql = "SELECT NEW CompanyInfo(a.id,a.Company) FROM CompanyInfo a,TypeInfo b WHERE a.TypeID=b.id  and a.id in (" + UserInfo.RoleCmp + ") ORDER BY a.SortID";
            string hql = "SELECT NEW CompanyInfo(a.id,a.Company) FROM CompanyInfo a,TypeInfo b WHERE a.TypeID=b.id and a.id in (" + UserInfo.RoleCmp + ") ORDER BY a.SortID";

            SimpleQuery<CompanyInfo> CmQuery = new SimpleQuery<CompanyInfo>(hql);
            CompanyInfo[] CmBeans = CmQuery.Execute();
            //查询银行 id 名字
            hql = "SELECT NEW BankInfo(a.id,a.Bank) FROM BankInfo a,TypeInfo b WHERE a.TypeID=b.id ";
            SimpleQuery<BankInfo> Query = new SimpleQuery<BankInfo>(hql);
            BankInfo[] BkBeans = Query.Execute();
            //查询类别
            string TableName = "CompanyInfo";
            hql = "SELECT NEW TypeInfo(a.id,a.TableName,a.TypeName,a.ParentID,a.SortID,a.Userid) FROM TypeInfo a WHERE a.TableName='" + TableName + "'";
            SimpleQuery<TypeInfo> TQuery = new SimpleQuery<TypeInfo>(hql);
            TypeInfo[] TBeans = TQuery.Execute();
            #endregion

            #region 整理数据

            #endregion

            #region 设置数据
            ViewBag.ComList = CmBeans;
            ViewBag.BankList = BkBeans;
            ViewBag.TypeList = TBeans;
            if (string.IsNullOrEmpty(Company))
            {
                ViewBag.Company = "";
            }
            else
            {
                ViewBag.Company = Company;
            }
            if (string.IsNullOrEmpty(Methods))
            {
                ViewBag.Methods = "";
            }
            else
            {
                ViewBag.Methods = Methods;
            }

            #endregion

            return View();
        }

        /// <summary>
        /// 承兑记录
        /// </summary>
        /// <returns></returns>
        public ActionResult ServerRec4()
        {
            #region 验证

            AdminInfo UserInfo = RefuseRedirect();
            if (UserInfo==null) {
                return View("../Login/Login");
            }
            #endregion

            #region 获取数据

            string Company = Request.QueryString["Company"];
            string Methods = Request.QueryString["Methods"];
            //
            #endregion

            #region 获取权限

            //获取用户权限
            List<SiteFunction> auth = getUserAuth();

            #endregion

            #region 查询数据

            //查找公司 id 名字
            //string hql = "SELECT NEW CompanyInfo(a.id,a.Company) FROM CompanyInfo a,TypeInfo b WHERE a.TypeID=b.id  and a.id in (" + UserInfo.RoleCmp + ") ORDER BY a.SortID";
            string hql = "SELECT NEW CompanyInfo(a.id,a.Company) FROM CompanyInfo a,TypeInfo b WHERE a.TypeID=b.id and a.id in (" + UserInfo.RoleCmp + ") ORDER BY a.SortID";

            SimpleQuery<CompanyInfo> CmQuery = new SimpleQuery<CompanyInfo>(hql);
            CompanyInfo[] CmBeans = CmQuery.Execute();
            //查询银行 id 名字
            hql = "SELECT NEW BankInfo(a.id,a.Bank) FROM BankInfo a,TypeInfo b WHERE a.TypeID=b.id ";
            SimpleQuery<BankInfo> Query = new SimpleQuery<BankInfo>(hql);
            BankInfo[] BkBeans = Query.Execute();
            //查询类别
            string TableName = "CompanyInfo";
            hql = "SELECT NEW TypeInfo(a.id,a.TableName,a.TypeName,a.ParentID,a.SortID,a.Userid) FROM TypeInfo a WHERE a.TableName='" + TableName + "'";
            SimpleQuery<TypeInfo> TQuery = new SimpleQuery<TypeInfo>(hql);
            TypeInfo[] TBeans = TQuery.Execute();
            #endregion

            #region 整理数据

            #endregion

            #region 设置数据

            ViewBag.ComList = CmBeans;
            ViewBag.BankList = BkBeans;
            ViewBag.TypeList = TBeans;
            if (string.IsNullOrEmpty(Company)) {
                ViewBag.Company = "";
            }
            else
            {
                ViewBag.Company = Company;
            }
            if (string.IsNullOrEmpty(Methods)) {
                ViewBag.Methods = "";
            }
            else
            {
                ViewBag.Methods = Methods;
            }
            

            #endregion

            return View();
        }

        /// <summary>
        /// 到期承兑
        /// </summary>
        /// <returns></returns>
        public ActionResult ServerRec5()
        {
            AdminInfo UserInfo = RefuseRedirect();
            if (UserInfo == null)
            {
                return View("../Login/Login");
            }
            #region 获取权限

            //获取用户权限
            List<SiteFunction> auth = getUserAuth();

            #endregion
            return View();
        }

        /// <summary>
        /// 到期贷款
        /// </summary>
        /// <returns></returns>
        public ActionResult ServerRec6()
        {
            AdminInfo UserInfo = RefuseRedirect();
            if (UserInfo == null)
            {
                return View("../Login/Login");
            }
            #region 获取权限

            //获取用户权限
            List<SiteFunction> auth = getUserAuth();

            #endregion

            return View();
        }

        /// <summary>
        /// 承兑记录查询
        /// </summary>
        /// <returns></returns>
        public ActionResult ServerRec7()
        {
            AdminInfo UserInfo = RefuseRedirect();
            if (UserInfo == null)
            {
                return View("../Login/Login");
            }

            #region 获取权限

            //获取用户权限
            List<SiteFunction> auth = getUserAuth();

            #endregion

            return View();
        }

        /// <summary>
        /// 借款记录查询
        /// </summary>
        /// <returns></returns>
        public ActionResult ServerRec8()
        {
            AdminInfo UserInfo = RefuseRedirect();
            if (UserInfo == null)
            {
                return View("../Login/Login");
            }

            #region 获取权限

            //获取用户权限
            List<SiteFunction> auth = getUserAuth();

            #endregion



            return View();
        }

        /// <summary>
        /// 用户管理
        /// </summary>
        /// <returns></returns>
        public ActionResult UserConfig()
        {
            #region 验证

            AdminInfo UserInfo = RefuseRedirect();
            if (UserInfo == null)
            {
                return View("../Login/Login");
            }

            #endregion

            #region 获取权限

            //获取用户权限
            List<SiteFunction> auth = getUserAuth();

            #endregion

            JObject msg = new JObject();
            //查找CmpInfo表
            //查找DepartmentInfo表
            //创建json 格式
            /*
             * [{//CmpInfo
             *  id:Id
             *  SName:公司名称
             *  UserId:
             *  SortId:
             *  deps:[
             *  {//DepartmentInfo
             *    id:
             *    SName:部门名称
             *    SortID
             *  },
             *  {
             *  },
             *  ...
             *  ]
             * },
             * {
             * },...]
             * 
             */
            try
            {
                #region 查询信息

                //查找公司信息
                string hql = "SELECT NEW CmpInfo(a.id,a.Company,a.SortID,a.Userid) FROM CmpInfo a";
                SimpleQuery<CmpInfo> CmpQuery = new SimpleQuery<CmpInfo>(hql);
                CmpInfo[] CmpBeans = CmpQuery.Execute();

                //查找部门信息
                hql = "SELECT NEW DepartmentInfo(a.id,a.CmpID,a.DptName,a.SortID,a.Userid) FROM DepartmentInfo a";
                SimpleQuery<DepartmentInfo> DepQuery = new SimpleQuery<DepartmentInfo>(hql);
                DepartmentInfo[] DepBeans = DepQuery.Execute();
                //查找公司信息2
                hql = "SELECT NEW CompanyInfo(a.id,a.Company,a.TypeID) FROM CompanyInfo a";
                SimpleQuery<CompanyInfo> CmQuery = new SimpleQuery<CompanyInfo>(hql);
                CompanyInfo[] CmBeans = CmQuery.Execute();
                //查询所有的人员信息
                hql = "SELECT NEW AdminInfo(a.id,a.DptID,a.CmpID,a.Username,a.NickName,a.RoleCmp,a.RoleConfig,a.SiteConfig) FROM AdminInfo a";
                SimpleQuery<AdminInfo> AdQuery = new SimpleQuery<AdminInfo>(hql);
                AdminInfo[] AdBeans = AdQuery.Execute();
                //查询所有的权限信息
                List<SiteFunction> SiteBeans = SiteFunction.getFunctionList(0, 0);
                List<SiteFunction> SiteBeans2 = SiteFunction.getFunctionList(1, 2);
                #endregion

                #region 处理信息

                //处理公司信息和部门信息
                JArray Cmpja = new JArray();
                if (CmpBeans.Length == 0)
                {
                    msg.Add("msg", "error");
                }
                else
                {
                    //将公司信息和部门信息合并
                    foreach (CmpInfo bean in CmpBeans)
                    {
                        JObject jobCmp = new JObject();
                        jobCmp.Add("Id", bean.id);
                        jobCmp.Add("SName", bean.Company);
                        jobCmp.Add("UserId", bean.Userid);
                        jobCmp.Add("SortId", bean.SortID);
                        JArray JaDeps = new JArray();
                        foreach (DepartmentInfo bean2 in DepBeans)
                        {
                            if (bean2.CmpID == bean.id)
                            {
                                JObject JobDep = new JObject();
                                JobDep.Add("Id", bean2.id);
                                JobDep.Add("SName", bean2.DptName);
                                JobDep.Add("UserId", bean2.Userid);
                                JobDep.Add("SortId", bean2.SortID);
                                JaDeps.Add(JobDep);
                                continue;
                            }
                        }
                        jobCmp.Add("deps", JaDeps);
                        Cmpja.Add(jobCmp);
                    }
                    msg.Add("msg", "OK");
                    msg.Add("data", Cmpja);
                }


                #endregion

                #region 整合信息

                //整合公司信息和部门信息
                ViewBag.Companys = Cmpja.ToString().Replace("\r\n", "");
                ViewBag.Companys2 = Cmpja;
                ViewBag.People = JsonConvert.SerializeObject(AdBeans).ToString().Replace("\r\n", "");
                ViewBag.Companys3 = CmBeans;
                ViewBag.Companys4 = JsonConvert.SerializeObject(CmBeans).ToString().Replace("\r\n", "");
                ViewBag.Roles = SiteBeans;
                ViewBag.Roles2 = JsonConvert.SerializeObject(SiteBeans2).ToString().Replace("\r\n", "");
                #endregion

                return View();
            }
            catch (Exception _e)
            {
                Log.Error("Home/UserConfig", _e);
                throw;
            }

        }

        #endregion

        #region 添加请求
        /// <summary>
        /// 添加公司
        /// </summary>
        /// <param name="fc"></param>
        /// <returns></returns>
        public string Add_Company(FormCollection fc)
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

                string Company = fc["Company"];
                string TypeID = fc["TypeID"];
                string SortID = fc["SortID"];
                string Remarks = fc["Remarks"];

                #endregion

                #region 转化数据

                int I_TypeId = int.Parse(TypeID==""?"0": TypeID);

                #endregion

                #region 处理数据

                CompanyInfo bean = new CompanyInfo();
                bean.Company = Company;
                bean.TypeID = I_TypeId;
                bean.SortID = SortID;
                bean.Remarks = Remarks;
                bean.Userid = (int)user.id;
                bean = (CompanyInfo)bean.SaveCopyAndFlush();
                user = AdminInfo.Find(user.id);
                if (string.IsNullOrEmpty(user.RoleCmp)) {
                    user.RoleCmp = bean.id + "";
                }
                else
                {
                    user.RoleCmp += "," + bean.id;
                }
                user.UpdateAndFlush();
                //更新session的内容
                HttpContext.Session["UserInfo"] = user;
                #endregion

                #region 返回数据
                msg.Add("msg", "OK");
                msg.Add("data", JsonConvert.SerializeObject(bean).ToString().Replace("\r\n", ""));
                return msg.ToString();
                #endregion
            }
            catch (Exception _e)
            {
                msg.Add("msg", "error");
                Log.Error("Home/Add_Company", _e);
                return msg.ToString();

                throw;
            }
        }
        /// <summary>
        /// 添加员工的企业
        /// </summary>
        /// <param name="fc"></param>
        /// <returns></returns>
        public string Add_Company2(FormCollection fc) {
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
                string Company = fc["Company"];
                string SortID = fc["SortID"];
                #endregion
                #region 转化数据
                //验证

                string hql = "SELECT NEW CmpInfo(a.id,a.Company,a.SortID,a.Userid) FROM CmpInfo a WHERE a.SortID = ? or a.Company = ?";
                SimpleQuery<CmpInfo> CmpQuery = new SimpleQuery<CmpInfo>(hql, SortID, Company);
                CmpInfo[] CmpBeans = CmpQuery.Execute();

                if (CmpBeans.Length>0) {
                    msg.Add("msg","exist");
                    return msg.ToString();
                }

                #endregion
                #region 处理请求
                CmpInfo bean = new CmpInfo();
                bean.Company = Company;
                bean.SortID = SortID;
                bean.Userid = (int)user.id;
                bean.SaveAndFlush();
                #endregion
                #region 返回数据
                //JsonConvert.SerializeObject(bean).ToString().Replace("\r\n", "")
                msg.Add("msg", "OK");
                return msg.ToString();
                #endregion
            }
            catch (Exception _e)
            {
                Log.Error("Home/Add_Company2", _e);
                msg.Add("msg", "error");
                throw;
            }

        }

        /// <summary>
        /// 添加部门
        /// </summary>
        /// <param name="fc"></param>
        /// <returns></returns>
        public string Add_DepartmentInfo(FormCollection fc) {
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

                string CmpID = fc["CmpID"];
                string DptName = fc["DptName"];
                string SortID = fc["SortID"];

                #endregion

                #region 转化数据

                int int_CmpID = int.Parse(CmpID);

                #endregion

                #region 验证

                string hql = "SELECT NEW DepartmentInfo(a.id,a.CmpID,a.DptName,a.SortID,a.Userid)FROM DepartmentInfo a WHERE a.SortID=? or ( a.CmpID=? AND a.DptName = ? )";
                SimpleQuery<DepartmentInfo> Query = new SimpleQuery<DepartmentInfo>(hql,SortID,CmpID,DptName);
                DepartmentInfo[] beans = Query.Execute();
                if (beans.Length>0) {
                    msg.Add("msg", "exist");
                    return msg.ToString();
                }
                #endregion


                #region 处理请求

                DepartmentInfo bean = new DepartmentInfo();
                bean.CmpID = int_CmpID;
                bean.DptName = DptName;
                bean.SortID = SortID;
                bean.Userid = (int)user.id;
                bean.SaveAndFlush();
                #endregion
                #region 返回数据
                //JsonConvert.SerializeObject(bean).ToString().Replace("\r\n", "")
                msg.Add("msg", "OK");
                return msg.ToString();
                #endregion
            }
            catch (Exception _e)
            {
                Log.Error("Home/Add_DepartmentInfo", _e);
                msg.Add("msg", "error");
                throw;
            }

        }

        /// <summary>
        /// 添加员工
        /// </summary>
        /// <returns></returns>
        public string Add_Person(FormCollection fc) {
            JObject msg = new JObject();
            #region 验证
            if (authrize() == null)
            {
                msg.Add("msg", "refuse");
                return msg.ToString();
            }
            #endregion
            try
            {
                #region 获取数据
                string Username = fc["Username"];
                string DptID = fc["DptID"];
                string CmpID = fc["CmpID"];
                string WorkID = fc["WorkID"];
                string Password = fc["Password"];
                //string RoleCmp = fc["RoleCmp"];
                //string Role = fc["Role"];//是否管理员
                string NickName = fc["NickName"];

                #endregion
                #region 转化数据
                int Int_DptID = int.Parse(DptID);
                int Int_CmpID = int.Parse(CmpID);
                //int Int_Role = int.Parse(Role);
                #endregion
                #region 处理请求

                AdminInfo bean = new AdminInfo();
                bean.Username = Username;
                //查找是否用重复的Username
                string hql = "SELECT NEW AdminInfo(a.id,a.DptID,a.CmpID,a.Username,a.NickName,a.RoleCmp,a.RoleConfig,a.SiteConfig) FROM AdminInfo a WHERE a.Username = ?";
                SimpleQuery<AdminInfo> AdQuery = new SimpleQuery<AdminInfo>(hql,bean.Username);
                AdminInfo[] Beans = AdQuery.Execute();
                if (Beans.Length>0) {
                    msg.Add("msg", "exist");
                    return msg.ToString();
                }
                bean.DptID = Int_DptID;
                bean.CmpID = Int_CmpID;
                bean.WorkID = WorkID;
                bean.Password = Password;
                //bean.Role = Int_Role;
                bean.NickName = NickName;
                bean.SaveAndFlush();
                #endregion
                #region 返回数据
                //JsonConvert.SerializeObject(bean).ToString().Replace("\r\n", "")
                msg.Add("msg", "OK");
                return msg.ToString();
                #endregion
            }
            catch (Exception _e)
            {
                Log.Error("Home/Add_Person", _e);
                msg.Add("msg", "error");
                throw;
            }
        }

        /// <summary>
        /// 添加类别
        /// </summary>
        /// <param name="fc"></param>
        /// <returns></returns>
        [HttpPost]
        public string Add_CompanyType(FormCollection fc) {
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
                //AdminInfo user = (AdminInfo)HttpContext.Session["userInfo"];
                string TypeName = fc["SName"];
                string TableName = fc["TableName"];
                //string TableName = "CompanyInfo";
                //生成SortId
                string SortId;
                string hql = "SELECT NEW TypeInfo(a.id,a.TableName,a.TypeName,a.ParentID,a.SortID,a.Userid) FROM TypeInfo a WHERE a.TableName='"+ TableName + "'";
                SimpleQuery<TypeInfo> Query = new SimpleQuery<TypeInfo>(hql);
                TypeInfo[] TBeans = Query.Execute();
                int findMax;
                if (TBeans.Length==0 || TBeans==null) {
                    findMax = 1;
                }
                else
                {
                    findMax = TBeans.Length + 1;
                }
                if (findMax>=10) {
                    SortId = findMax + "";
                }
                else
                {
                    SortId = "0" + findMax;
                }
                //查找是否出现重复主键
                foreach (TypeInfo item in TBeans) {
                    if (item.TypeName== TypeName && item.TableName == TableName) {
                        msg.Add("msg", "EXIST");
                        return msg.ToString();
                    }
                }
                #endregion
                #region 转化数据
                #endregion
                #region 处理数据
                TypeInfo bean = new TypeInfo();
                bean.TableName = TableName;
                bean.TypeName = TypeName;
                bean.ParentID = 1;
                bean.SortID = SortId;
                bean.Userid = (int)user.id;
                bean = (TypeInfo)bean.SaveCopyAndFlush();
                #endregion
                #region 返回数据
                msg.Add("msg", "OK");
                msg.Add("data", JsonConvert.SerializeObject(bean).ToString().Replace("\r\n", ""));
                return msg.ToString();
                #endregion
            }
            catch (Exception _e)
            {
                Log.Error("Home/Del_Company", _e);
                msg.Add("msg", "error");
                    throw;
            }
        }
        /// <summary>
        /// 添加银行信息
        /// </summary>
        /// <param name="fc"></param>
        /// <returns></returns>
        [HttpPost]
        public string Add_Bank(FormCollection fc)
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

                string Bank = fc["Bank"];
                string Remarks = fc["Remarks"];
                string TypeID = fc["TypeID"];
                string SortID = fc["SortID"];
                int Userid = (int)user.id;
                DateTime Now = DateTime.Now;
                
                #endregion
                #region 转化数据

                int i_TypeID = int.Parse(TypeID==""?"0": TypeID);
                
                #endregion
                #region 处理请求

                BankInfo bean = new BankInfo();
                bean.Bank = Bank;
                bean.TypeID = i_TypeID;
                bean.BuildDate = Now;
                bean.Remarks = Remarks;
                bean.SortID = SortID;
                bean.Userid = Userid;
                bean = (BankInfo)bean.SaveCopyAndFlush();
                #endregion
                #region 返回数据
                //更新session的内容
                //JsonConvert.SerializeObject(bean).ToString().Replace("\r\n", "")
                msg.Add("msg", "OK");
                return msg.ToString();
                #endregion
            }
            catch (Exception _e)
            {
                Log.Error("Home/Add_Bank", _e);
                msg.Add("msg", "error");
                return msg.ToString();
                throw;
            }
        }

        /// <summary>
        /// 添加授信记录
        /// </summary>
        /// <param name="fc"></param>
        /// <returns></returns>
        public string Add_CreditInfo(FormCollection fc) {
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

                //企业名称
                string Company = fc["Company"];
                //银行名称
                string Bank = fc["Bank"];
                //信用额度
                string Credit = fc["Credit"];
                //承兑额度
                string Acceptance = fc["Acceptance"];
                //贷款额度
                string Loans = fc["Loans"];
                //贷款利率
                string Rates = fc["Rates"];
                //欠款总额
                string Arrears = fc["Arrears"];
                //备注
                string Remarks = fc["Remarks"];
                //创建日期
                DateTime now = DateTime.Now;
                #endregion
                #region 转化数据
                decimal de_Credit = decimal.Parse(Credit);
                decimal de_Acceptance = decimal.Parse(Acceptance);
                decimal de_Loans = decimal.Parse(Loans);
                decimal de_Arrears = decimal.Parse(Arrears);
                #endregion
                #region 处理请求

                CreditInfo bean = new CreditInfo();
                bean.Company = Company;
                bean.Bank = Bank;
                bean.Credit = de_Credit;
                bean.Acceptance = de_Acceptance;
                bean.Loans = de_Loans;
                bean.Rates = Rates;
                bean.Arrears = de_Arrears;
                bean.Remarks = Remarks;
                bean.Builddate = now;
                //其他要计算的
                bean.Acceptanced = de_Acceptance;
                bean.Loansed = 0;
                bean.Userid = (int)user.id;

                bean.SaveAndFlush();
                #endregion
                #region 返回数据
                //JsonConvert.SerializeObject(bean).ToString().Replace("\r\n", "")
                msg.Add("msg", "OK");
                return msg.ToString();
                #endregion
            }
            catch (Exception _e)
            {
                Log.Error("Home/Add_CreditInfo", _e);
                msg.Add("msg", "error");
                throw;
            }

        }

        /// <summary>
        /// 添加贷款记录
        /// </summary>
        /// <param name="fc"></param>
        /// <returns></returns>
        [HttpPost]
        public string Add_BankServer1(FormCollection fc) {
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

                string Company = fc["Company"];
                string Bank = fc["Bank"];
                string Abstract = fc["Abstract"];
                string Rates = fc["Rates"];
                string LoanAmount = fc["LoanAmount"];
                string OccDate = fc["OccDate"];
                string EndDate = fc["EndDate"];
                string Remarks = fc["Remarks"];

                #endregion
                #region 转化数据

                DateTime D_OccDate = DateTime.Parse(OccDate);
                DateTime D_EndDate = DateTime.Parse(EndDate);
                decimal de_LoanAmount = decimal.Parse(LoanAmount);

                #endregion

                #region 处理请求

                CreditBusinessInfo bean = new CreditBusinessInfo();
                bean.Company = Company;
                bean.Bank = Bank;
                bean.Abstract = Abstract;
                bean.Rates = Rates;
                bean.LoanAmount = de_LoanAmount;
                bean.OccDate = D_OccDate;
                bean.Remarks = Remarks;
                bean.BuildDate = DateTime.Now;
                bean.EndDate = D_EndDate;

                bean.Type = "贷";
                bean.Flag = "未清";
                bean.Userid = user.id;
                bean.Balance = de_LoanAmount;
                bean.Save();
                #endregion
                #region 返回数据
                //JsonConvert.SerializeObject(bean).ToString().Replace("\r\n", "")
                msg.Add("msg", "OK");
                return msg.ToString();
                #endregion
            }
            catch (Exception _e)
            {
                Log.Error("Home/Add_BankServer1", _e);
                msg.Add("msg", "error");
                throw;
            }
        }

        /// <summary>
        /// 添加还款记录
        /// </summary>
        /// <param name="fc"></param>
        /// <returns></returns>
        [HttpPost]
        public string Add_BankServer2(FormCollection fc)
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

                string Company = fc["Company"];
                string Bank = fc["Bank"];
                string Abstract = fc["Abstract"];
                string OccDate = fc["OccDate"];
                string Remarks = fc["Remarks"];
                string Repayrecord = fc["Repayrecord"];
                string RepayAmount = fc["RepayAmount"];
                #endregion
                #region 转化数据
                DateTime D_OccDate = DateTime.Parse(OccDate);
                decimal de_RepayAmount = decimal.Parse(RepayAmount);

                #endregion

                #region 处理请求
                //更新副表
                //拆分 取出 id 金额
                string new_Repayrecord = Repayrecord.Substring(0, Repayrecord.Length-1);
                string[] strs = new_Repayrecord.Split('/');
                List<CreditBusinessInfo> beans = new List<CreditBusinessInfo>();
                for (int i=0;i<strs.Length;i++) {
                    string[] str = strs[i].Split(',');
                    long id = long.Parse(str[0]);
                    decimal s_RepayAmount = decimal.Parse(str[1]);//还款金额
                    CreditBusinessInfo bean = CreditBusinessInfo.Find(id);
                    //余额
                    bean.Balance -= s_RepayAmount;
                    //Status = 1 有还款记录
                    bean.Status = 1;
                    //Repayed+ 已还金额
                    bean.Repayed += s_RepayAmount;
                    //Flag 判断是否未清/已清 Balance
                    if (bean.Balance<=0) {
                        bean.Flag = "已清";
                    }
                    beans.Add(bean);
                }
                //更新还款主表
                CreditBusinessInfo bean2 = new CreditBusinessInfo();
                bean2.Company = Company;
                bean2.Bank = Bank;
                bean2.Abstract = Abstract;
                bean2.OccDate = D_OccDate;
                bean2.Remarks = Remarks;
                bean2.Repayrecord = Repayrecord;
                bean2.RepayAmount = de_RepayAmount;
                bean2.Type = "还";
                bean2.BuildDate = DateTime.Now;
                bean2.Userid = user.id;

                bean2.Save();
                foreach (CreditBusinessInfo item in beans) {
                    if (item.Repayrecord=="") {
                        item.Repayrecord +=  bean2.id;
                    }
                    else
                    {
                        item.Repayrecord += "," + bean2.id;
                    }
                    
                    item.SaveAndFlush();    
                }
                #endregion
                #region 返回数据
                //JsonConvert.SerializeObject(bean).ToString().Replace("\r\n", "")
                msg.Add("msg", "OK");
                return msg.ToString();
                #endregion
            }
            catch (Exception _e)
            {
                Log.Error("Home/Add_BankServer1", _e);
                msg.Add("msg", "error");
                throw;
            }
        }

        /// <summary>
        /// 添加承兑记录
        /// </summary>
        /// <param name="fc"></param>
        /// <returns></returns>
        [HttpPost]
        public string Add_BankServer2_1(FormCollection fc)
        {
            JObject msg = new JObject();
            AdminInfo User = authrize();
            #region 验证
            if (User == null)
            {
                msg.Add("msg", "refuse");
                return msg.ToString();
            }
            #endregion
            try
            {
                #region 获取数据
                string Company = fc["Company"];
                string Bank = fc["Bank"];
                string Abstract = fc["Abstract"];
                string LoanAmount = fc["LoanAmount"];
                string Margin = fc["Margin"];
                string OccDate = fc["OccDate"];
                string EndDate = fc["EndDate"];
                string Remarks = fc["Remarks"];
                #endregion
                #region 转化数据

                decimal de_LoanAmount = decimal.Parse(LoanAmount);
                decimal de_Margin = decimal.Parse(Margin);
                DateTime D_OccDate = DateTime.Parse(OccDate);
                DateTime D_EndDate = DateTime.Parse(EndDate);

                #endregion

                #region 处理请求

                AcceptancesInfo bean = new AcceptancesInfo();
                bean.Company = Company;
                bean.Bank = Bank;
                bean.Abstract = Abstract;
                bean.LoanAmount = de_LoanAmount;
                bean.Margin = de_Margin;
                bean.OccDate = D_OccDate;
                bean.EndDate = D_EndDate;
                bean.Remarks = Remarks;
                //自动生成的
                bean.BuildDate = DateTime.Now;
                bean.Type = "贷";
                bean.Flag = "未清";
                bean.Userid = (int)User.id;
                bean.Balance = de_LoanAmount- de_Margin;

                bean.SaveAndFlush();
                #endregion
                #region 返回数据
                //JsonConvert.SerializeObject(bean).ToString().Replace("\r\n", "")
                msg.Add("msg", "OK");
                return msg.ToString();
                #endregion
            }
            catch (Exception _e)
            {
                Log.Error("Home/Add_BankServer2_1", _e);
                msg.Add("msg", "error");
                throw;
            }
        }

        /// <summary>
        /// 添加承兑记录 还款
        /// </summary>
        /// <param name="fc"></param>
        /// <returns></returns>
        [HttpPost]
        public string Add_BankServer2_2(FormCollection fc)
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

                string Company = fc["Company"];
                string Bank = fc["Bank"];
                string Abstract = fc["Abstract"];
                string OccDate = fc["OccDate"];
                string Remarks = fc["Remarks"];
                string Repayrecord = fc["Repayrecord"];
                string RepayAmount = fc["RepayAmount"];
                #endregion
                #region 转化数据
                DateTime D_OccDate = DateTime.Parse(OccDate);
                decimal de_RepayAmount = decimal.Parse(RepayAmount);

                #endregion

                #region 处理请求
                //更新副表
                //拆分 取出 id 金额
                string new_Repayrecord = Repayrecord.Substring(0, Repayrecord.Length - 1);
                string[] strs = new_Repayrecord.Split('/');
                List<AcceptancesInfo> beans = new List<AcceptancesInfo>();
                for (int i = 0; i < strs.Length; i++)
                {
                    string[] str = strs[i].Split(',');
                    long id = long.Parse(str[0]);
                    decimal s_RepayAmount = decimal.Parse(str[1]);//还款金额
                    AcceptancesInfo bean = AcceptancesInfo.Find(id);
                    //余额
                    bean.Balance -= s_RepayAmount;
                    //Status = 1 有还款记录
                    bean.Status = 1;
                    //Repayed+ 已还金额
                    bean.Repayed += s_RepayAmount;
                    //Flag 判断是否未清/已清 Balance
                    if (bean.Balance <= 0)
                    {
                        bean.Flag = "已清";
                    }
                    beans.Add(bean);
                }
                //更新还款主表
                AcceptancesInfo bean2 = new AcceptancesInfo();
                bean2.Company = Company;
                bean2.Bank = Bank;
                bean2.Abstract = Abstract;
                bean2.OccDate = D_OccDate;
                bean2.Remarks = Remarks;
                bean2.Repayrecord = Repayrecord;
                bean2.RepayAmount = de_RepayAmount;
                bean2.Type = "还";
                bean2.BuildDate = DateTime.Now;
                bean2.Userid = (int)user.id;
                bean2.Save();
                foreach (AcceptancesInfo item in beans)
                {
                    if (item.Repayrecord == "")
                    {
                        item.Repayrecord += bean2.id;
                    }
                    else
                    {
                        item.Repayrecord += "," + bean2.id;
                    }

                    item.SaveAndFlush();
                }
                #endregion
                #region 返回数据
                //JsonConvert.SerializeObject(bean).ToString().Replace("\r\n", "")
                msg.Add("msg", "OK");
                return msg.ToString();
                #endregion
            }
            catch (Exception _e)
            {
                Log.Error("Home/Add_BankServer1", _e);
                msg.Add("msg", "error");
                throw;
            }

        }

        #endregion

        #region 删除请求
        /// <summary>
        /// 删除企业信息
        /// </summary>
        /// <param name="fc"></param>
        /// <returns></returns>
        [HttpPost]
        public string Del_Company(FormCollection fc)
        {
            JObject msg = new JObject();
            #region 验证
            if (authrize() == null)
            {
                msg.Add("msg", "refuse");
                return msg.ToString();
            }
            #endregion
            try
            {
                #region 获取数据

                string id = fc["id"];

                #endregion
                #region 转化数据

                long l_id = long.Parse(id);

                #endregion
                #region 处理数据

                CompanyInfo bean = new CompanyInfo()
                {
                    id = l_id
                };
                bean.DeleteAndFlush();
                #endregion
                #region 返回数据
                //JsonConvert.SerializeObject(bean).ToString().Replace("\r\n", "")
                msg.Add("msg", "OK");
                msg.Add("data", bean.id);
                return msg.ToString();
                #endregion
            }
            catch (Exception _e)
            {
                Log.Error("Home/Del_Company", _e);
                msg.Add("msg", "error");
                throw;
            }
        }
        /// <summary>
        /// 删除用户相关的数据
        /// </summary>
        /// <param name="fc"></param>
        /// <returns></returns>
        public string Del_ComAndDpt(FormCollection fc) {
            JObject msg = new JObject();
            #region 验证
            if (authrize() == null)
            {
                msg.Add("msg", "refuse");
                return msg.ToString();
            }
            #endregion
            try
            {
                #region 获取数据

                string id = fc["Id"];
                string DelTab = fc["DelTab"];
                #endregion
                #region 转化数据

                long l_id = long.Parse(id);

                #endregion
                #region 处理数据
                if (DelTab == "Cmp") {
                    CmpInfo bean = new CmpInfo();
                    bean.id = l_id;
                    //查找是否有下属部门
                    string hql = "SELECT NEW DepartmentInfo(a.id,a.CmpID,a.DptName,a.SortID,a.Userid) FROM DepartmentInfo a where a.CmpID=?";
                    SimpleQuery<DepartmentInfo> DepQuery = new SimpleQuery<DepartmentInfo>(hql, l_id);
                    DepartmentInfo[] Beans = DepQuery.Execute();
                    if (Beans.Length>0) {
                        msg.Add("msg", "exist1");
                        return msg.ToString();
                    }
                    bean.Delete();
                } else if (DelTab == "Dpt") {
                    DepartmentInfo bean = new DepartmentInfo();
                    bean.id = l_id;
                    string hql = "SELECT NEW AdminInfo(a.id,a.DptID,a.CmpID,a.Username,a.NickName,a.RoleCmp,a.RoleConfig,a.SiteConfig) FROM AdminInfo a WHERE a.DptID = ?";
                    SimpleQuery<AdminInfo> AdQuery = new SimpleQuery<AdminInfo>(hql, l_id);
                    AdminInfo[] Beans = AdQuery.Execute();
                    if (Beans.Length > 0)
                    {
                        msg.Add("msg", "exist2");
                        return msg.ToString();
                    }
                    bean.Delete();
                } else if (DelTab == "Per") {
                    AdminInfo bean = new AdminInfo();
                    bean.id = l_id;

                    string hql = "SELECT NEW AcceptancesInfo(a.id,a.Type,a.OccDate,a.Abstract,a.Rates,a.Company,a.Bank,a.LoanAmount,a.EndDate,a.Remarks,a.BuildDate,a.Flag,a.RepayAmount,a.Balance,a.Margin,a.Status,a.Repayed,a.Repayrecord,a.Userid,c.NickName,b.id,d.id,d.TypeName)FROM AcceptancesInfo a,CompanyInfo b,AdminInfo c,TypeInfo d WHERE  a.Userid = c.id AND a.Company=b.Company AND b.TypeID=d.id AND c.id=? ORDER BY a.Company,a.Bank,a.Type,a.OccDate";
                    SimpleQuery<AcceptancesInfo> query = new SimpleQuery<AcceptancesInfo>(hql,l_id);
                    AcceptancesInfo[] Abeans = query.Execute();

                    hql = "SELECT NEW CreditBusinessInfo(a.id,a.Type,a.OccDate,a.Abstract,a.Rates,a.Company,a.Bank,a.LoanAmount,a.EndDate,a.Remarks,a.BuildDate,a.Flag,a.RepayAmount,a.Balance,a.Status,a.Repayed,a.Repayrecord,a.Userid,c.NickName,b.id,d.id,d.TypeName)FROM CreditBusinessInfo a,CompanyInfo b,AdminInfo c,TypeInfo d WHERE  a.Userid = c.id AND a.Company=b.Company AND b.TypeID=d.id AND c.id=? ORDER BY a.Company,a.Bank,a.Type,a.OccDate";
                    SimpleQuery<CreditBusinessInfo> query1 = new SimpleQuery<CreditBusinessInfo>(hql,l_id);
                    CreditBusinessInfo[] Cbeans = query1.Execute();

                    if (Abeans.Length>0 || Cbeans.Length> 0) {
                        msg.Add("msg", "exist3");
                        return msg.ToString();
                    }

                    bean.Delete();
                }
                else
                {
                    msg.Add("msg","error");
                    return msg.ToString();
                }
                #endregion
                #region 返回数据
                //JsonConvert.SerializeObject(bean).ToString().Replace("\r\n", "")
                msg.Add("msg", "OK");
                msg.Add("data", l_id);
                return msg.ToString();
                #endregion
            }
            catch (Exception _e)
            {
                Log.Error("Home/Del_Company", _e);
                msg.Add("msg", "error");
                throw;
            }
        }

        

        /// <summary>
        /// 删除银行信息
        /// </summary>
        /// <param name="fc"></param>
        /// <returns></returns>
        [HttpPost]
        public string Del_Bank(FormCollection fc)
        {
            JObject msg = new JObject();
            #region 验证
            if (authrize() == null)
            {
                msg.Add("msg", "refuse");
                return msg.ToString();
            }
            #endregion
            try
            {
                #region 获取数据
                string id = fc["id"];
                #endregion
                #region 转化数据

                long l_id = long.Parse(id);

                #endregion

                #region 处理请求

                BankInfo bean = new BankInfo();
                bean.id = l_id;
                bean.DeleteAndFlush();

                #endregion
                #region 返回数据

                //JsonConvert.SerializeObject(bean).ToString().Replace("\r\n", "")
                msg.Add("msg", "OK");
                msg.Add("data",bean.id);
                return msg.ToString();

                #endregion
            }
            catch (Exception _e)
            {
                Log.Error("Home/Del_Bank", _e);
                msg.Add("msg", "error");
                throw;
            }
        }

        /// <summary>
        /// 删除银行信贷
        /// </summary>
        /// <param name="fc"></param>
        /// <returns></returns>
        [HttpPost]
        public string Del_CreditInfo(FormCollection fc)
        {
            JObject msg = new JObject();
            #region 验证
            if (authrize() == null)
            {
                msg.Add("msg", "refuse");
                return msg.ToString();
            }
            #endregion
            try
            {
                #region 获取数据

                string id = fc["id"];

                #endregion
                #region 转化数据

                long l_id = long.Parse(id);

                #endregion

                #region 处理请求

                CreditInfo bean = new CreditInfo();
                bean.id = l_id;
                bean.DeleteAndFlush();

                #endregion

                #region 返回数据
                //JsonConvert.SerializeObject(bean).ToString().Replace("\r\n", "")
                msg.Add("msg", "OK");

                return msg.ToString();
                #endregion
            }
            catch (Exception _e)
            {
                Log.Error("Home/Del_CreditInfo", _e);
                msg.Add("msg", "error");
                throw;
            }
        }

        /// <summary>
        /// 删除还款记录
        /// </summary>
        /// <param name="fc"></param>
        /// <returns></returns>
        [HttpPost]
        public string Del_BankServer1(FormCollection fc) {
            JObject msg = new JObject();

            #region 验证
            if (authrize() == null)
            {
                msg.Add("msg", "refuse");
                return msg.ToString();
            }
            #endregion
            try
            {
                #region 获取数据
                string id = fc["id"];
                #endregion
                #region 转化数据

                long l_id = long.Parse(id);

                #endregion

                #region 处理请求

                CreditBusinessInfo bean = CreditBusinessInfo.Find(l_id);

                //撤回数据
                if (!string.IsNullOrEmpty(bean.Repayrecord))
                {
                    string new_Repayrecord = bean.Repayrecord.Substring(0, bean.Repayrecord.Length - 1);
                    string[] strs = new_Repayrecord.Split('/');
                    for (int i = 0; i < strs.Length; i++)
                    {
                        string[] str = strs[i].Split(',');
                        long Old_id = long.Parse(str[0]);
                        decimal s_RepayAmount = decimal.Parse(str[1]);//还款金额
                        CreditBusinessInfo bean2 = CreditBusinessInfo.Find(Old_id);
                        //余额
                        bean2.Balance += s_RepayAmount;
                        //Status = 1 有还款记录
                        string[] strs2 = bean2.Repayrecord.Split(',');
                        string new_Repayrecord2 = "";
                        for (int j=0;j< strs2.Length; j++) {
                            if (strs2[j]== l_id+"") {
                                continue;
                            }
                            else
                            {
                                new_Repayrecord2 += strs2[j]+",";
                            }
                        }
                        if (new_Repayrecord2=="") {//从有还款记录到无还款记录
                            bean2.Repayrecord = "";
                            bean2.Status = 0;
                        }
                        else
                        {
                            new_Repayrecord2 = new_Repayrecord2.Substring(0, new_Repayrecord2.Length - 1);
                            bean2.Repayrecord = new_Repayrecord2;
                        }
                        //Repayed+ 已还金额
                        bean2.Repayed -= s_RepayAmount;
                        //Flag 判断是否未清/已清 Balance
                        if (bean2.Balance > 0)
                        {
                            bean2.Flag = "未清";
                        }
                        bean2.SaveAndFlush();
                    }
                }

                //业务处理。。。
                bean.Delete();

                #endregion
                #region 返回数据

                //JsonConvert.SerializeObject(bean).ToString().Replace("\r\n", "")
                msg.Add("msg", "OK");
                return msg.ToString();
                #endregion
            }
            catch (Exception _e)
            {
                Log.Error("Home/Del_BankServer1", _e);
                msg.Add("msg", "error");
                throw;
            }

        }
        /// <summary>
        /// 删除贷款记录
        /// </summary>
        /// <param name="fc"></param>
        /// <returns></returns>
        [HttpPost]
        public string Del_BankServer1_2(FormCollection fc)
        {
            JObject msg = new JObject();
            #region 验证
            if (authrize() == null)
            {
                msg.Add("msg", "refuse");
                return msg.ToString();
            }
            #endregion
            try
            {
                #region 获取数据
                string id = fc["id"];
                #endregion
                #region 转化数据
                long l_id = long.Parse(id);
                #endregion
                #region 处理请求
                CreditBusinessInfo bean = CreditBusinessInfo.Find(l_id);
                if (bean.Status==1) {
                    msg.Add("msg", "Fail");
                    return msg.ToString();
                }
                else
                {
                    bean.Delete();
                }
                #endregion
                #region 返回数据
                //JsonConvert.SerializeObject(bean).ToString().Replace("\r\n", "")
                msg.Add("msg", "OK");
                return msg.ToString();
                #endregion
            }
            catch (Exception _e)
            {
                Log.Error("Home/Del_BankServer1_2", _e);
                msg.Add("msg", "error");
                throw;
            }
        }

        /// <summary>
        /// 删除承兑记录 还款记录
        /// </summary>
        /// <param name="fc"></param>
        /// <returns></returns>
        [HttpPost]
        public string Del_BankServer2_1(FormCollection fc) {
            JObject msg = new JObject();

            #region 验证
            if (authrize() == null)
            {
                msg.Add("msg", "refuse");
                return msg.ToString();
            }
            #endregion
            try
            {
                #region 获取数据
                string id = fc["id"];
                #endregion
                #region 转化数据

                long l_id = long.Parse(id);

                #endregion

                #region 处理请求

                AcceptancesInfo bean = AcceptancesInfo.Find(l_id);

                //撤回数据
                if (!string.IsNullOrEmpty(bean.Repayrecord))
                {
                    string new_Repayrecord = bean.Repayrecord.Substring(0, bean.Repayrecord.Length - 1);
                    string[] strs = new_Repayrecord.Split('/');
                    for (int i = 0; i < strs.Length; i++)
                    {
                        string[] str = strs[i].Split(',');
                        long Old_id = long.Parse(str[0]);
                        decimal s_RepayAmount = decimal.Parse(str[1]);//还款金额
                        AcceptancesInfo bean2 = AcceptancesInfo.Find(Old_id);
                        //余额
                        bean2.Balance += s_RepayAmount;
                        //Status = 1 有还款记录
                        string[] strs2 = bean2.Repayrecord.Split(',');
                        string new_Repayrecord2 = "";
                        for (int j = 0; j < strs2.Length; j++)
                        {
                            if (strs2[j] == l_id + "")
                            {
                                continue;
                            }
                            else
                            {
                                new_Repayrecord2 += strs2[j] + ",";
                            }
                        }
                        if (new_Repayrecord2 == "")
                        {//从有还款记录到无还款记录
                            bean2.Repayrecord = "";
                            bean2.Status = 0;
                        }
                        else
                        {
                            new_Repayrecord2 = new_Repayrecord2.Substring(0, new_Repayrecord2.Length - 1);
                            bean2.Repayrecord = new_Repayrecord2;
                        }
                        //Repayed+ 已还金额
                        bean2.Repayed -= s_RepayAmount;
                        //Flag 判断是否未清/已清 Balance
                        if (bean2.Balance > 0)
                        {
                            bean2.Flag = "未清";
                        }
                        bean2.SaveAndFlush();
                    }
                }

                //业务处理。。。
                bean.Delete();

                #endregion
                #region 返回数据

                //JsonConvert.SerializeObject(bean).ToString().Replace("\r\n", "")
                msg.Add("msg", "OK");
                return msg.ToString();
                #endregion
            }
            catch (Exception _e)
            {
                Log.Error("Home/Del_BankServer2_1", _e);
                msg.Add("msg", "error");
                throw;
            }
        }

        /// <summary>
        /// 删除承兑记录 贷
        /// </summary>
        /// <param name="fc"></param>
        /// <returns></returns>
        [HttpPost]
        public string Del_BankServer2_2(FormCollection fc)
        {
            JObject msg = new JObject();
            #region 验证
            if (authrize() == null)
            {
                msg.Add("msg", "refuse");
                return msg.ToString();
            }
            #endregion
            try
            {
                #region 获取数据
                string id = fc["id"];
                #endregion
                #region 转化数据
                long l_id = long.Parse(id);
                #endregion
                #region 处理请求
                AcceptancesInfo bean = AcceptancesInfo.Find(l_id);
                if (bean.Status == 1)
                {
                    msg.Add("msg", "Fail");
                    return msg.ToString();
                }
                else
                {
                    bean.Delete();
                }
                #endregion
                #region 返回数据
                //JsonConvert.SerializeObject(bean).ToString().Replace("\r\n", "")
                msg.Add("msg", "OK");
                return msg.ToString();
                #endregion
            }
            catch (Exception _e)
            {
                Log.Error("Home/Del_BankServer2_2", _e);
                msg.Add("msg", "error");
                throw;
            }
        }

        #endregion

        #region 修改的请求
        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="fc"></param>
        /// <returns></returns>
        [HttpPost]
        public string Edit_AdminInfo_Role(FormCollection fc)
        {
            JObject msg = new JObject();

            #region 验证
            if (authrize() == null)
            {
                msg.Add("msg", "refuse");
                return msg.ToString();
            }
            #endregion
            try
            {
                #region 获取数据
                string User_Id = fc["User_Id"];
                string DepSel = fc["DepSel"];
                string Cmp1 = fc["Cmp1"];
                string Site1 = fc["Site1"];
                //转化数据为int
                int DptId = int.Parse(DepSel);
                long UserId = int.Parse(User_Id);

                #endregion

                #region 修改处理
                //查询
                AdminInfo bean = AdminInfo.Find(UserId);
                DepartmentInfo dptbean = DepartmentInfo.Find(DptId);
                bean.DptID = DptId;
                bean.CmpID = dptbean.CmpID;
                bean.RoleCmp = Cmp1;
                bean.SiteConfig = Site1;
                bean.SaveAndFlush();

                #endregion

                #region 返回信息

                msg.Add("msg", "OK");
                return msg.ToString();
                #endregion
            }
            catch (Exception _e)
            {
                Log.Error("Home/Edit_AdminInfo_Role", _e);
                msg.Add("msg", "error");
                return msg.ToString();
                throw;
            }
        }
        /// <summary>
        /// 修改公司信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string Edit_Company(FormCollection fc)
        {
            JObject msg = new JObject();
            #region 验证
            if (authrize()==null)
            {
                msg.Add("msg", "refuse");
                return msg.ToString();
            }
            #endregion
            try
            {
                #region 获取数据

                string Company = fc["Company"];
                string SortID = fc["SortID"];
                string TypeID = fc["TypeID"];
                string Remarks = fc["Remarks"];
                string id = fc["id"];

                #endregion

                #region 转化数据

                long l_id = long.Parse(id);

                #endregion

                #region 处理请求

                CompanyInfo bean = Models.CompanyInfo.Find(l_id);
                string connStr = System.Configuration.ConfigurationManager.AppSettings["connStr"];
                SqlConnection conn = new SqlConnection(connStr);
                SqlTransaction tr = null;

                #region 更新关联表
                try
                {
                    conn.Open();
                    tr = conn.BeginTransaction();
                    SqlCommand scmd = new SqlCommand();
                    scmd.Connection = conn;
                    scmd.Transaction = tr;
                    string sql = "UPDATE CreditBusinessInfo SET Company='"+ Company + "' WHERE Company='"+bean.Company+"'";
                    scmd.CommandText = sql;
                    int i = scmd.ExecuteNonQuery();
                    sql = "UPDATE AcceptancesInfo SET Company='" + Company + "' WHERE Company='"+bean.Company+"'";
                    scmd.CommandText = sql;
                    i = scmd.ExecuteNonQuery();
                    tr.Commit();
                }
                catch (Exception _e2)
                {
                    if (tr != null)
                    {
                        tr.Rollback();
                    }
                    Log.Error("Home/Edit_Company", _e2);
                    msg.Add("msg", "error");
                    return msg.ToString();
                    throw;
                }
                finally
                {
                    if (conn!=null && conn.State != System.Data.ConnectionState.Closed) {
                        conn.Close();
                    }
                }
                #endregion

                bean.Company = Company;
                bean.SortID = SortID;
                bean.TypeID = int.Parse(TypeID);
                bean.Remarks = Remarks;
                bean.SaveAndFlush();

                #endregion

                #region 返回数据
                //JsonConvert.SerializeObject(bean).ToString().Replace("\r\n", "")
                msg.Add("msg", "OK");
                return msg.ToString();
                #endregion
            }
            catch (Exception _e)
            {
                Log.Error("Home/Edit_Company", _e);
                msg.Add("msg", "error");
                return msg.ToString();
                throw;
            }

        }

        /// <summary>
        /// 修改银行信息
        /// </summary>
        /// <param name="fc"></param>
        /// <returns></returns>
        [HttpPost]
        public string Edit_Bank(FormCollection fc)
        {
            JObject msg = new JObject();
            #region 验证
            if (authrize() == null)
            {
                msg.Add("msg", "refuse");
                return msg.ToString();
            }
            #endregion
            try
            {
                #region 获取数据
                string id = fc["id"];
                string Bank = fc["Bank"];
                string Remarks = fc["Remarks"];
                string TypeID = fc["TypeID"];
                string SortID = fc["SortID"];
                //int Userid = (int)user.id;
                //DateTime Now = DateTime.Now;


                #endregion
                #region 转化数据

                long l_id = long.Parse(id);
                int i_TypeID = int.Parse(TypeID);
                #endregion

                #region 处理请求

                BankInfo bean = Models.BankInfo.Find(l_id);

                #region 更新关联表

                string connStr = System.Configuration.ConfigurationManager.AppSettings["connStr"];
                SqlConnection conn = new SqlConnection(connStr);
                SqlTransaction tr = null;
                try
                {
                    conn.Open();
                    tr = conn.BeginTransaction();
                    SqlCommand scmd = new SqlCommand();
                    scmd.Connection = conn;
                    scmd.Transaction = tr;
                    string sql = "UPDATE CreditBusinessInfo SET Bank='" + Bank + "' WHERE Bank='" + bean.Bank + "'";
                    scmd.CommandText = sql;
                    int i = scmd.ExecuteNonQuery();
                    sql = "UPDATE AcceptancesInfo SET Bank='" + Bank + "' WHERE Bank='" + bean.Bank+"'";
                    scmd.CommandText = sql;
                    i = scmd.ExecuteNonQuery();
                    tr.Commit();
                }
                catch (Exception _e2)
                {
                    if (tr != null)
                    {
                        tr.Rollback();
                    }
                    Log.Error("Home/Edit_Bank", _e2);
                    msg.Add("msg", "error");
                    return msg.ToString();
                    throw;
                }
                finally
                {
                    if (conn != null && conn.State != System.Data.ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                }
                #endregion

                bean.Bank = Bank;
                bean.Remarks = Remarks;
                bean.TypeID = i_TypeID;
                bean.SortID = SortID;
                bean.UpdateAndFlush();
                #endregion
                #region 返回数据
                //JsonConvert.SerializeObject(bean).ToString().Replace("\r\n", "")
                msg.Add("msg", "OK");
                msg.Add("data",bean.id);
                return msg.ToString();
                #endregion
            }
            catch (Exception _e)
            {
                Log.Error("Home/Edit_Bank", _e);
                msg.Add("msg", "error");
                return msg.ToString();
                //throw;
            }
        }

        [HttpPost]
        public string Edit_CreditInfo(FormCollection fc)
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
                //id
                string id = fc["id"];
                //企业名称
                string Company = fc["Company"];
                //银行名称
                string Bank = fc["Bank"];
                //信用额度
                string Credit = fc["Credit"];
                //承兑额度
                string Acceptance = fc["Acceptance"];
                //贷款额度
                string Loans = fc["Loans"];
                //贷款利率
                string Rates = fc["Rates"];
                //欠款总额
                string Arrears = fc["Arrears"];
                //备注
                string Remarks = fc["Remarks"];
                //创建日期
                DateTime now = DateTime.Now;
                #endregion
                #region 转化数据
                decimal de_Credit = decimal.Parse(Credit);
                decimal de_Acceptance = decimal.Parse(Acceptance);
                decimal de_Loans = decimal.Parse(Loans);
                decimal de_Arrears = decimal.Parse(Arrears);
                long l_id = long.Parse(id);
                #endregion
                #region 处理请求

                CreditInfo bean = CreditInfo.Find(l_id);

                bean.Company = Company;
                bean.Bank = Bank;
                bean.Credit = de_Credit;
                bean.Acceptance = de_Acceptance;
                bean.Loans = de_Loans;
                bean.Rates = Rates;
                bean.Arrears = de_Arrears;
                bean.Remarks = Remarks;
                bean.Builddate = now;
                //其他要计算的
                //bean.Acceptanced = de_Acceptance;
                //bean.Loansed = 0;
                bean.Userid = (int)user.id;

                bean.UpdateAndFlush();
                #endregion
                #region 返回数据
                //JsonConvert.SerializeObject(bean).ToString().Replace("\r\n", "")
                msg.Add("msg", "OK");
                return msg.ToString();
                #endregion

            }
            catch (Exception _e)
            {
                Log.Error("Home/Del_Company", _e);
                msg.Add("msg", "error");
                throw;
            }
        }

        /// <summary>
        /// 修改贷款业务
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string Edit_BankServer1(FormCollection fc) {
            JObject msg = new JObject();
            #region 验证
            if (authrize() == null)
            {
                msg.Add("msg", "refuse");
                return msg.ToString();
            }
            #endregion
            try
            {
                #region 获取数据
                string id = fc["id"];
                string Company = fc["Company"];
                string Bank = fc["Bank"];
                string Abstract = fc["Abstract"];
                string Rates = fc["Rates"];
                string LoanAmount = fc["LoanAmount"];
                string OccDate = fc["OccDate"];
                string EndDate = fc["EndDate"];
                string Remarks = fc["Remarks"];
                #endregion
                #region 转化数据

                long l_id = long.Parse(id);
                decimal de_LoanAmount = decimal.Parse(LoanAmount);
                DateTime D_OccDate = DateTime.Parse(OccDate);
                DateTime D_EndDate = DateTime.Parse(EndDate);

                #endregion
                #region 处理请求

                CreditBusinessInfo bean = CreditBusinessInfo.Find(l_id);
                bean.Company = Company;
                bean.Bank = Bank;
                bean.Abstract = Abstract;
                bean.Rates = Rates;
                bean.LoanAmount = de_LoanAmount;
                bean.OccDate = D_OccDate;
                bean.EndDate = D_EndDate;
                bean.Remarks = Remarks;
                bean.Balance = de_LoanAmount - bean.Repayed;
                bean.SaveAndFlush();

                #endregion
                #region 返回数据
                //JsonConvert.SerializeObject(bean).ToString().Replace("\r\n", "")
                msg.Add("msg", "OK");
                return msg.ToString();
                #endregion
            }
            catch (Exception _e)
            {
                Log.Error("Home/Edit_BankServer1", _e);
                msg.Add("msg", "error");
                throw;
            }

        }

        /// <summary>
        /// 修改承兑记录
        /// </summary>
        /// <param name="fc"></param>
        /// <returns></returns>
        public string Edit_BankServer2(FormCollection fc)
        {
            JObject msg = new JObject();
            #region 验证
            if (authrize() == null)
            {
                msg.Add("msg", "refuse");
                return msg.ToString();
            }
            #endregion
            try
            {
                #region 获取数据
                string id = fc["id"];
                string Company = fc["Company"];
                string Bank = fc["Bank"];
                string Abstract = fc["Abstract"];
                string Margin = fc["Margin"];
                string LoanAmount = fc["LoanAmount"];
                string OccDate = fc["OccDate"];
                string EndDate = fc["EndDate"];
                string Remarks = fc["Remarks"];
                #endregion
                #region 转化数据

                long l_id = long.Parse(id);
                decimal de_LoanAmount = decimal.Parse(LoanAmount);//承兑金额
                decimal de_Margin = decimal.Parse(Margin);
                DateTime D_OccDate = DateTime.Parse(OccDate);
                DateTime D_EndDate = DateTime.Parse(EndDate);

                #endregion
                #region 处理请求

                AcceptancesInfo bean = AcceptancesInfo.Find(l_id);
                bean.Company = Company;
                bean.Bank = Bank;
                bean.Abstract = Abstract;
                bean.Margin = de_Margin;//保证金
                bean.LoanAmount = de_LoanAmount;
                bean.OccDate = D_OccDate;
                bean.EndDate = D_EndDate;
                bean.Remarks = Remarks;
                bean.Balance = bean.LoanAmount - bean.Margin - bean.Repayed;
                bean.SaveAndFlush();

                #endregion
                #region 返回数据
                //JsonConvert.SerializeObject(bean).ToString().Replace("\r\n", "")
                msg.Add("msg", "OK");
                return msg.ToString();
                #endregion
            }
            catch (Exception _e)
            {
                Log.Error("Home/Edit_BankServer2", _e);
                msg.Add("msg", "error");
                throw;
            }
        }

        #endregion

        #region 查询请求

        /// <summary>
        /// 查询公司信息
        /// </summary>
        /// <param name="fc"></param>
        /// <returns></returns>
        [HttpPost]
        public string Search_Company(FormCollection fc)
        {
            JObject msg = new JObject();
            AdminInfo user = authrize();
            if (user == null)
            {
                msg.Add("msg", "refuse");
                return msg.ToString();
            }
            AdminInfo UserInfo = (AdminInfo)HttpContext.Session["UserInfo"];
            try
            {
                #region 查询信息

                string hql = "SELECT NEW CompanyInfo(a.id,a.Company,a.TypeID,a.Builddate,a.Remarks,a.SortID,a.Userid,b.id,b.TypeName) FROM CompanyInfo a,TypeInfo b WHERE a.TypeID=b.id  and a.id in (" + UserInfo.RoleCmp + ") ORDER BY a.SortID";
                SimpleQuery<CompanyInfo> CmQuery = new SimpleQuery<CompanyInfo>(hql);
                CompanyInfo[] CmBeans = CmQuery.Execute();

                #endregion

                #region 处理请求

                string CopInfo = JsonConvert.SerializeObject(CmBeans).ToString().Replace("\r\n", "");

                #endregion

                #region 返回信息

                msg.Add("msg", "OK");
                msg.Add("data", CopInfo);
                return msg.ToString();

                #endregion

            }
            catch (Exception _e)
            {
                Log.Error("Home/Search_Company", _e);
                throw;
            }
        }

        /// <summary>
        /// 
        /// 查询银行信息
        /// 
        /// </summary>
        /// <param name="fc"></param>
        /// <returns></returns>
        [HttpPost]
        public string Search_Bank(FormCollection fc)
        {
            JObject msg = new JObject();
            if (authrize()==null)
            {
                msg.Add("msg", "refuse");
                return msg.ToString();
            }
            AdminInfo UserInfo = (AdminInfo)HttpContext.Session["UserInfo"];
            try
            {
                #region 查询数据

                string hql = "SELECT NEW BankInfo(a.id,a.Bank,a.TypeID,a.BuildDate,a.Remarks,a.SortID,a.Userid,b.id,b.TypeName) FROM BankInfo a,TypeInfo b WHERE a.TypeID=b.id ORDER BY a.SortID";
                SimpleQuery<BankInfo> Query = new SimpleQuery<BankInfo>(hql);
                BankInfo[] Beans = Query.Execute();

                #endregion

                #region 处理请求

                string BankInfo = JsonConvert.SerializeObject(Beans).ToString().Replace("\r\n", "");

                #endregion

                #region 返回信息

                msg.Add("msg", "OK");
                msg.Add("data", BankInfo);
                return msg.ToString();

                #endregion
            }
            catch (Exception _e)
            {
                Log.Error("Home/Search_Bank", _e);
                throw;
            }
        }

        /// <summary>
        /// 查询借款的下拉联动
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string Search_Type_Company_Bank(FormCollection fc) {
            JObject msg = new JObject();
            AdminInfo user = authrize();
            #region 验证
            if (User == null)
            {
                msg.Add("msg", "refuse");
                return msg.ToString();
            }
            #endregion
            try
            {
                #region 获取数据
                string Type = fc["Type"];
                #endregion
                #region 转化数据
                #endregion
                #region 处理请求
                string hql = "";
                //查询出业务表
                AcceptancesInfo[] Abeans = null;
                CreditBusinessInfo[] Cbeans = null;
                if (Type == "AcceptancesInfo") {
                    hql = "SELECT NEW AcceptancesInfo(a.id,a.Type,a.OccDate,a.Abstract,a.Rates,a.Company,a.Bank,a.LoanAmount,a.EndDate,a.Remarks,a.BuildDate,a.Flag,a.RepayAmount,a.Balance,a.Margin,a.Status,a.Repayed,a.Repayrecord,a.Userid,c.NickName,b.id,d.id,d.TypeName)FROM AcceptancesInfo a,CompanyInfo b,AdminInfo c,TypeInfo d WHERE b.id in (" + user.RoleCmp + ") AND a.Userid = c.id AND a.Company=b.Company AND b.TypeID=d.id ORDER BY a.Company,a.Bank,a.Type,a.OccDate";
                    SimpleQuery<AcceptancesInfo> query = new SimpleQuery<AcceptancesInfo>(hql);
                    Abeans = query.Execute();
                }
                else if (Type == "CreditBusinessInfo") {
                    hql = "SELECT NEW CreditBusinessInfo(a.id,a.Type,a.OccDate,a.Abstract,a.Rates,a.Company,a.Bank,a.LoanAmount,a.EndDate,a.Remarks,a.BuildDate,a.Flag,a.RepayAmount,a.Balance,a.Status,a.Repayed,a.Repayrecord,a.Userid,c.NickName,b.id,d.id,d.TypeName)FROM CreditBusinessInfo a,CompanyInfo b,AdminInfo c,TypeInfo d WHERE b.id in (" + user.RoleCmp + ") AND a.Userid = c.id AND a.Company=b.Company AND b.TypeID=d.id ORDER BY a.Company,a.Bank,a.Type,a.OccDate";
                    SimpleQuery<CreditBusinessInfo> query = new SimpleQuery<CreditBusinessInfo>(hql);
                    Cbeans = query.Execute();
                }
                //查找公司 id 名字
                //string hql = "SELECT NEW CompanyInfo(a.id,a.Company) FROM CompanyInfo a,TypeInfo b WHERE a.TypeID=b.id  and a.id in (" + UserInfo.RoleCmp + ") ORDER BY a.SortID";
                hql = "SELECT NEW CompanyInfo(a.id,a.Company,b.id,b.TypeName) FROM CompanyInfo a,TypeInfo b WHERE a.TypeID=b.id and a.id in (" + user.RoleCmp + ") ORDER BY a.SortID";
                SimpleQuery<CompanyInfo> CmQuery = new SimpleQuery<CompanyInfo>(hql);
                CompanyInfo[] CmBeans = CmQuery.Execute();

                //查询类别
                string TableName = "CompanyInfo";
                hql = "SELECT NEW TypeInfo(a.id,a.TableName,a.TypeName,a.ParentID,a.SortID,a.Userid) FROM TypeInfo a WHERE a.TableName='" + TableName + "'";
                SimpleQuery<TypeInfo> TQuery = new SimpleQuery<TypeInfo>(hql);
                TypeInfo[] TBeans = TQuery.Execute();
                List<TypeInfo> Tbeans2 = TBeans.ToList();
                foreach (TypeInfo bean in Tbeans2) {
                    foreach (CompanyInfo cmp in CmBeans) {
                        if (bean.TypeName == cmp.SType.TypeName) {
                            if (bean.CompanyList == null) {
                                bean.CompanyList = new List<CompanyInfo>();
                                bean.CompanyList.Add(cmp);
                            }
                            else
                            {
                                bean.CompanyList.Add(cmp);
                            }
                        }
                    }
                }
                //从业务表内找公司和银行的关系
                if (Type == "AcceptancesInfo") {
                    foreach (AcceptancesInfo bean in Abeans) {
                        foreach (TypeInfo Tbean in Tbeans2)
                        {
                            if (bean.cmp.SType.TypeName == Tbean.TypeName) {
                                foreach (CompanyInfo cbean in Tbean.CompanyList) {
                                    if (bean.Company == cbean.Company) {
                                        if (cbean.BankList == null)
                                        {
                                            cbean.BankList = new List<string>();
                                            cbean.BankList.Add(bean.Bank);
                                        }
                                        else {
                                            if (!cbean.BankList.Exists(x => x == bean.Bank)) {
                                                cbean.BankList.Add(bean.Bank);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                } else if (Type == "CreditBusinessInfo") {
                    foreach (CreditBusinessInfo bean in Cbeans)
                    {
                        foreach (TypeInfo Tbean in Tbeans2)
                        {
                            if (bean.cmp.SType.TypeName == Tbean.TypeName)
                            {
                                foreach (CompanyInfo cbean in Tbean.CompanyList)
                                {
                                    if (bean.Company == cbean.Company)
                                    {
                                        if (cbean.BankList == null)
                                        {
                                            cbean.BankList = new List<string>();
                                            cbean.BankList.Add(bean.Bank);
                                        }
                                        else
                                        {
                                            if (!cbean.BankList.Exists(x => x == bean.Bank))
                                            {
                                                cbean.BankList.Add(bean.Bank);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }


                #endregion

                #region 返回数据
                //JsonConvert.SerializeObject(bean).ToString().Replace("\r\n", "")
                msg.Add("msg", "OK");
                msg.Add("data", JsonConvert.SerializeObject(Tbeans2).ToString().Replace("\r\n", ""));
                return msg.ToString();
                #endregion
            }
            catch (Exception _e)
            {
                Log.Error("Home/Search_Type_Company_Bank", _e);
                msg.Add("msg", "error");
                throw;
            }
        }

        /// <summary>
        /// 查询授信记录
        /// </summary>
        /// <param name="fc"></param>
        /// <returns></returns>
        [HttpPost]
        public string Search_CreditInfo(FormCollection fc)
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

                string hql = "SELECT NEW CreditInfo(a.id,a.Company,a.Bank,a.Credit,a.Acceptance,a.Loans,a.Arrears,a.Remarks,a.Builddate,a.Rates,a.Acceptanced,a.Loansed,a.Total,a.Userid,b.TypeID,c.TypeName) FROM CreditInfo a,CompanyInfo b,TypeInfo c WHERE a.Company=b.Company and b.TypeID = c.id AND b.id in ("+user.RoleCmp+") ORDER BY a.Company";
                SimpleQuery<CreditInfo> Query = new SimpleQuery<CreditInfo>(hql);
                CreditInfo[] Beans = Query.Execute();

                #endregion

                #region 返回数据
                
                msg.Add("msg", "OK");
                msg.Add("data", JsonConvert.SerializeObject(Beans).ToString().Replace("\r\n", ""));
                return msg.ToString();

                #endregion
            }
            catch (Exception _e)
            {
                Log.Error("Home/Del_Company", _e);
                msg.Add("msg", "error");
                throw;
            }
        }

        [HttpPost]
        public string Search_BankServer1(FormCollection fc)
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

                string hql = "SELECT NEW CreditBusinessInfo(a.id,a.Type,a.OccDate,a.Abstract,a.Rates,a.Company,a.Bank,a.LoanAmount,a.EndDate,a.Remarks,a.BuildDate,a.Flag,a.RepayAmount,a.Balance,a.Status,a.Repayed,a.Repayrecord,a.Userid,c.NickName,b.id,d.id,d.TypeName)FROM CreditBusinessInfo a,CompanyInfo b,AdminInfo c,TypeInfo d WHERE b.id in (" + user.RoleCmp+ ") AND a.Userid = c.id AND a.Company=b.Company AND b.TypeID=d.id ORDER BY a.Company,a.Bank,a.OccDate";
                SimpleQuery<CreditBusinessInfo> query = new SimpleQuery<CreditBusinessInfo>(hql);
                CreditBusinessInfo[] beans = query.Execute();
                #endregion
                #region 返回数据
                //JsonConvert.SerializeObject(bean).ToString().Replace("\r\n", "")
                msg.Add("msg", "OK");
                msg.Add("data", JsonConvert.SerializeObject(beans).ToString().Replace("\r\n", ""));
                return msg.ToString();
                #endregion
            }
            catch (Exception _e)
            {
                Log.Error("Home/Search_BankServer1", _e);
                msg.Add("msg", "error");
                throw;
            }
        }
        /// <summary>
        /// 查询承兑业务
        /// </summary>
        /// <param name="fc"></param>
        /// <returns></returns>
        public string Search_BankServer2(FormCollection fc) {
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

                string hql = "SELECT NEW AcceptancesInfo(a.id,a.Type,a.OccDate,a.Abstract,a.Rates,a.Company,a.Bank,a.LoanAmount,a.EndDate,a.Remarks,a.BuildDate,a.Flag,a.RepayAmount,a.Balance,a.Margin,a.Status,a.Repayed,a.Repayrecord,a.Userid,c.NickName,b.id,d.id,d.TypeName)FROM AcceptancesInfo a,CompanyInfo b,AdminInfo c,TypeInfo d WHERE b.id in (" + user.RoleCmp + ") AND a.Userid = c.id AND a.Company=b.Company AND b.TypeID=d.id ORDER BY a.Company,a.Bank,a.OccDate";
                SimpleQuery<AcceptancesInfo> query = new SimpleQuery<AcceptancesInfo>(hql);
                AcceptancesInfo[] beans = query.Execute();
                #endregion
                #region 返回数据
                //JsonConvert.SerializeObject(bean).ToString().Replace("\r\n", "")
                msg.Add("msg", "OK");
                msg.Add("data", JsonConvert.SerializeObject(beans).ToString().Replace("\r\n", ""));
                return msg.ToString();
                #endregion
            }
            catch (Exception _e)
            {
                Log.Error("Home/Search_BankServer2", _e);
                msg.Add("msg", "error");
                throw;
            }
        }

        /// <summary>
        /// 到期提醒 贷款查询
        /// </summary>
        /// <param name="fc"></param>
        /// <returns></returns>
        public string Search_SercerRec2_1(FormCollection fc) {
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
                string connStr = System.Configuration.ConfigurationManager.AppSettings["connStr"];
                SqlConnection conn = new SqlConnection(connStr);
                List<CreditBusinessInfo> beans2 = null;
                string d1 = DateTime.Now.AddYears(-1).ToString("yyyy-MM-dd");
                string d2 = Lw_Utils.getMonthLast(DateTime.Now.AddMonths(1), "yyyy-MM-dd");
                try
                {
                    conn.Open();
                    string sql = "SELECT c.TypeName,a.Company,sum(a.Balance) AS 'Sum_Balance' ," +
                                 "SUM(" +
                                     "case" +
                                         " when a.EndDate >= '" + d1 + "' and a.EndDate <= '" + d2 + "' then a.Balance else 0 " +
                                     "end" +
                                 ") AS 'LM_Balance' " +
                                 "FROM CreditBusinessInfo a,CompanyInfo b, TypeInfo c WHERE a.Company = b.Company and b.TypeID = c.id and c.TableName = 'CompanyInfo' and b.id in (" + user.RoleCmp + ") Group by a.Company,c.TypeName,b.SortID ORDER BY c.TypeName,b.SortID desc";
                    SqlCommand cmd2 = new SqlCommand(sql, conn);
                    SqlDataReader rdr2 = cmd2.ExecuteReader();
                    beans2 = new List<CreditBusinessInfo>();
                    while (rdr2.Read())
                    {
                        CreditBusinessInfo bean = new CreditBusinessInfo();
                        bean.cmp = new CompanyInfo()
                        {
                            Company = (string)rdr2["Company"],
                            SType = new TypeInfo()
                            {
                                TypeName = (string)rdr2["TypeName"]
                            }
                        };
                        bean.Balance = Convert.ToDecimal(rdr2["Sum_Balance"]);
                        bean.LM_Balance = Convert.ToDecimal(rdr2["LM_Balance"]);
                        beans2.Add(bean);
                    }

                }
                catch (Exception _e2)
                {
                    Log.Error("Home/Search_SercerRec2_1", _e2);
                }
                finally
                {
                    if (conn != null && conn.State != System.Data.ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                }

                #endregion
                #region 返回数据

                //JsonConvert.SerializeObject(bean).ToString().Replace("\r\n", "")
                msg.Add("msg", "OK");
                msg.Add("data2", JsonConvert.SerializeObject(beans2).ToString().Replace("\r\n", ""));
                return msg.ToString();

                #endregion
            }
            catch (Exception _e)
            {
                Log.Error("Home/Search_SercerRec2_1", _e);
                msg.Add("msg", "error");
                throw;
            }
        }

        /// <summary>
        /// 到期提醒 承兑查询
        /// </summary>
        /// <returns></returns>
        public string Search_SercerRec2_2(FormCollection fc) {
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
                string connStr = System.Configuration.ConfigurationManager.AppSettings["connStr"];
                SqlConnection conn = new SqlConnection(connStr);
                List<AcceptancesInfo> beans = null ;
                string d1 = DateTime.Now.AddYears(-1).ToString("yyyy-MM-dd");
                string d2 = Lw_Utils.getMonthLast(DateTime.Now.AddMonths(1), "yyyy-MM-dd");
                try {
                    conn.Open();
                    string sql = "SELECT c.TypeName,a.Company,sum(a.Balance) AS 'Sum_Balance', " +
                                "SUM(" +
                                    "case" +
                                        " when a.EndDate >= '"+d1+"' and a.EndDate <= '"+d2+"' then a.Balance else 0 " +
                                    "end" +
                                ") AS 'LM_Balance' " +
                                "FROM AcceptancesInfo a,CompanyInfo b, TypeInfo c WHERE a.Company = b.Company and b.TypeID = c.id and c.TableName = 'CompanyInfo' and b.id in (" + user.RoleCmp + ") Group by a.Company,c.TypeName,b.SortID ORDER BY c.TypeName,b.SortID desc";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    beans = new List<AcceptancesInfo>();
                    while (rdr.Read()) {
                        AcceptancesInfo bean = new AcceptancesInfo();
                        bean.cmp = new CompanyInfo() {
                            Company = (string)rdr["Company"],
                            SType = new TypeInfo() {
                                TypeName = (string)rdr["TypeName"]
                            }
                        };
                        bean.Balance = Convert.ToDecimal(rdr["Sum_Balance"]);
                        bean.LM_Balance = Convert.ToDecimal(rdr["LM_Balance"]);
                        beans.Add(bean);
                    }

                }
                catch (Exception _e2) {
                    Log.Error("Home/Search_SercerRec2_2",_e2);
                }
                finally
                {
                    if (conn != null && conn.State != System.Data.ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                }
                
                #endregion
                #region 返回数据

                //JsonConvert.SerializeObject(bean).ToString().Replace("\r\n", "")
                msg.Add("msg", "OK");
                msg.Add("data1", JsonConvert.SerializeObject(beans).ToString().Replace("\r\n", ""));
                return msg.ToString();

                #endregion
            }
            catch (Exception _e)
            {
                Log.Error("Home/Search_SercerRec2_2", _e);
                msg.Add("msg", "error");
                throw;
            }
        }

        /// <summary>
        /// 借款汇总查询
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string Search_ServerRec3(FormCollection fc) {
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

                string TypeName = fc["TypeName"];
                string Company = fc["Company"];
                string Bank = fc["Bank"];
                string Flag = fc["Flag"];
                string Ser_Begin = fc["Ser_Begin"];
                string Ser_End = fc["Ser_End"];
                string Model = fc["Model"];
                string DateModel = fc["DateModel"];//0.Ser_Begin的之前 1.Ser_Begin的当月
                string End_Begin = fc["End_Begin"];
                string End_End = fc["End_End"];
                #endregion
                #region 转化数据

                //根据条件 传值
                string Where = "";
                List<string> Wheres = new List<string>();
                List<Object> paraList = new List<Object>();
                if (!string.IsNullOrEmpty(TypeName)) {
                    Wheres.Add("c.TypeName=?");
                    paraList.Add(TypeName);
                }
                if (!string.IsNullOrEmpty(Company)) {
                    Wheres.Add("a.Company = ?");
                    paraList.Add(Company);
                }

                if (!string.IsNullOrEmpty(Flag))
                {
                    Wheres.Add("a.Flag = ?");
                    paraList.Add(Flag);
                }
                if (!string.IsNullOrEmpty(Bank))
                {
                    Wheres.Add("a.Bank = ?");
                    paraList.Add(Bank);
                }
                //Ser_Begin的之前
                //if (!string.IsNullOrEmpty(Ser_Begin) && DateModel == "0") {
                //    DateTime dt = DateTime.Parse(Ser_Begin);
                //    DateTime end = Lw_Utils.getMonthFirstDt(dt);
                //    Wheres.Add("a.OccDate<=?");
                //    paraList.Add(end);
                //} else if (!string.IsNullOrEmpty(Ser_Begin) && DateModel == "1") {
                //    //Ser_Begin 的当月
                //    DateTime dt = DateTime.Parse(Ser_Begin);
                //    DateTime be = Lw_Utils.getMonthFirstDt(dt);
                //    DateTime end = Lw_Utils.getMonthLastDt(dt);
                //    Wheres.Add("a.OccDate>=?");
                //    Wheres.Add("a.OccDate<=?");
                //    paraList.Add(be);
                //    paraList.Add(end);
                //}

                if (!string.IsNullOrEmpty(Ser_Begin) && !string.IsNullOrEmpty(Ser_End))
                {
                    //区间内
                    DateTime be = DateTime.Parse(Ser_Begin);
                    DateTime end = DateTime.Parse(Ser_End);
                    Wheres.Add("a.OccDate>=?");
                    Wheres.Add("a.OccDate<=?");
                    paraList.Add(be);
                    paraList.Add(end);
                }
                else if (!string.IsNullOrEmpty(Ser_Begin) && string.IsNullOrEmpty(Ser_End))
                {
                    //右区间
                    DateTime be = DateTime.Parse(Ser_Begin);
                    Wheres.Add("a.OccDate>=?");
                    paraList.Add(be);
                }
                else if (string.IsNullOrEmpty(End_Begin) && !string.IsNullOrEmpty(Ser_End))
                {
                    //左区间
                    DateTime end = DateTime.Parse(Ser_End);
                    Wheres.Add("a.OccDate<=?");
                    paraList.Add(end);
                }


                if (!string.IsNullOrEmpty(End_Begin) && !string.IsNullOrEmpty(End_End)) {
                    //区间内
                    DateTime be = DateTime.Parse(End_Begin);
                    DateTime end = DateTime.Parse(End_End);
                    Wheres.Add("a.EndDate>=?");
                    Wheres.Add("a.EndDate<=?");
                    paraList.Add(be);
                    paraList.Add(end);
                }else if (!string.IsNullOrEmpty(End_Begin) && string.IsNullOrEmpty(End_End)) {
                    //右区间
                    DateTime be = DateTime.Parse(End_Begin);
                    Wheres.Add("a.EndDate>=?");
                    paraList.Add(be);
                }else if (string.IsNullOrEmpty(End_Begin) && !string.IsNullOrEmpty(End_End))
                {
                    //左区间
                    DateTime end = DateTime.Parse(End_End);
                    Wheres.Add("a.EndDate<=?");
                    paraList.Add(end);
                }

                if (Wheres.Count>0) {
                    //连接条件
                    Where = string.Join(" AND ",Wheres.ToArray());
                }

                #endregion

                #region 处理请求
                string hql = "";
                if (Model=="0") {
                    if (Wheres.Count > 0)
                    {
                        hql = "SELECT NEW CreditBusinessInfo(c.TypeName,SUM(a.LoanAmount),SUM(a.Repayed),SUM(a.Balance)) FROM CreditBusinessInfo a,CompanyInfo b,TypeInfo c WHERE  a.Company=b.Company AND b.TypeID=c.id AND " + Where + " AND b.id in(" +user.RoleCmp+ ") GROUP BY c.TypeName";
                    }
                    else
                    {
                        hql = "SELECT NEW CreditBusinessInfo(c.TypeName,SUM(a.LoanAmount),SUM(a.Repayed),SUM(a.Balance)) FROM CreditBusinessInfo a,CompanyInfo b,TypeInfo c WHERE  a.Company=b.Company AND b.TypeID=c.id AND b.id in ("+ user.RoleCmp + ")  GROUP BY c.TypeName";
                    }
                }
                else if(Model=="1")
                {
                    if (Wheres.Count > 0)
                    {
                        hql = "SELECT NEW CreditBusinessInfo(a.Company,SUM(a.LoanAmount),SUM(a.Repayed),SUM(a.Balance)) FROM CreditBusinessInfo a,CompanyInfo b,TypeInfo c WHERE  a.Company=b.Company AND b.TypeID=c.id AND a.Flag='未清' AND " + Where + " AND b.id in (" + user.RoleCmp + ") GROUP BY a.Company";
                    }
                    else
                    {
                        hql = "SELECT NEW CreditBusinessInfo(a.Company,SUM(a.LoanAmount),SUM(a.Repayed),SUM(a.Balance)) FROM CreditBusinessInfo a,CompanyInfo b,TypeInfo c WHERE a.Company=b.Company AND b.TypeID=c.id AND a.Flag='未清' AND b.id in (" + user.RoleCmp + ") GROUP BY a.Company";
                    }
                }
                else if (Model == "2")
                {
                    if (Wheres.Count > 0) {
                        hql = "SELECT NEW CreditBusinessInfo(a.id,a.Type,a.OccDate,a.Abstract,a.Rates,a.Company,a.Bank,a.LoanAmount,a.EndDate,a.Remarks,a.BuildDate,a.Flag,a.RepayAmount,a.Balance,a.Status,a.Repayed,a.Repayrecord,a.Userid,d.NickName,b.id,c.id,c.TypeName)FROM CreditBusinessInfo a,CompanyInfo b,AdminInfo d,TypeInfo c WHERE  a.Userid = d.id AND a.Company=b.Company AND b.TypeID=c.id AND "+ Where + " AND b.id in(" + user.RoleCmp + ")  ORDER BY a.Company, a.Bank , a.OccDate";

                    }
                    else
                    {
                        hql = "SELECT NEW CreditBusinessInfo(a.id,a.Type,a.OccDate,a.Abstract,a.Rates,a.Company,a.Bank,a.LoanAmount,a.EndDate,a.Remarks,a.BuildDate,a.Flag,a.RepayAmount,a.Balance,a.Status,a.Repayed,a.Repayrecord,a.Userid,d.NickName,b.id,c.id,c.TypeName)FROM CreditBusinessInfo a,CompanyInfo b,AdminInfo d,TypeInfo c WHERE  a.Userid = d.id AND a.Company=b.Company AND b.TypeID=c.id AND b.id in(" + user.RoleCmp + ") ORDER BY a.Company, a.Bank, a.OccDate";
                    }

                }


                SimpleQuery<CreditBusinessInfo> Query = new SimpleQuery<CreditBusinessInfo>(hql, paraList.ToArray());
                CreditBusinessInfo[] Beans = Query.Execute();

                #endregion

                #region 返回数据

                //JsonConvert.SerializeObject(bean).ToString().Replace("\r\n", "")
                msg.Add("msg", "OK");
                msg.Add("data", JsonConvert.SerializeObject(Beans).ToString().Replace("\r\n", ""));
                return msg.ToString();
                #endregion

            }
            catch (Exception _e)
            {
                Log.Error("Home/Search_ServerRec3", _e);
                msg.Add("msg", "error");
                throw;
            }

        }

        /// <summary>
        /// 承兑汇总查询
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string Search_ServerRec4(FormCollection fc)
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

                string TypeName = fc["TypeName"];
                string Company = fc["Company"];
                string Bank = fc["Bank"];
                string Flag = fc["Flag"];
                string Ser_Begin = fc["Ser_Begin"];
                string Ser_End = fc["Ser_End"];
                string Model = fc["Model"];
                string DateModel = fc["DateModel"];//0.Ser_Begin的之前 1.Ser_Begin的当月
                string End_Begin = fc["End_Begin"];
                string End_End = fc["End_End"];
                #endregion
                #region 转化数据

                //根据条件 传值
                string Where = "";
                List<string> Wheres = new List<string>();
                List<Object> paraList = new List<Object>();
                if (!string.IsNullOrEmpty(TypeName))
                {
                    Wheres.Add("c.TypeName=?");
                    paraList.Add(TypeName);
                }
                if (!string.IsNullOrEmpty(Company))
                {
                    Wheres.Add("a.Company = ?");
                    paraList.Add(Company);
                }

                if (!string.IsNullOrEmpty(Flag))
                {
                    Wheres.Add("a.Flag = ?");
                    paraList.Add(Flag);
                }
                if (!string.IsNullOrEmpty(Bank))
                {
                    Wheres.Add("a.Bank = ?");
                    paraList.Add(Bank);
                }
                //Ser_Begin的之前
                //if (!string.IsNullOrEmpty(Ser_Begin) && DateModel == "0") {
                //    DateTime dt = DateTime.Parse(Ser_Begin);
                //    DateTime end = Lw_Utils.getMonthFirstDt(dt);
                //    Wheres.Add("a.OccDate<=?");
                //    paraList.Add(end);
                //} else if (!string.IsNullOrEmpty(Ser_Begin) && DateModel == "1") {
                //    //Ser_Begin 的当月
                //    DateTime dt = DateTime.Parse(Ser_Begin);
                //    DateTime be = Lw_Utils.getMonthFirstDt(dt);
                //    DateTime end = Lw_Utils.getMonthLastDt(dt);
                //    Wheres.Add("a.OccDate>=?");
                //    Wheres.Add("a.OccDate<=?");
                //    paraList.Add(be);
                //    paraList.Add(end);
                //}

                if (!string.IsNullOrEmpty(Ser_Begin) && !string.IsNullOrEmpty(Ser_End))
                {
                    //区间内
                    DateTime be = DateTime.Parse(Ser_Begin);
                    DateTime end = DateTime.Parse(Ser_End);
                    Wheres.Add("a.OccDate>=?");
                    Wheres.Add("a.OccDate<=?");
                    paraList.Add(be);
                    paraList.Add(end);
                }
                else if (!string.IsNullOrEmpty(Ser_Begin) && string.IsNullOrEmpty(Ser_End))
                {
                    //右区间
                    DateTime be = DateTime.Parse(Ser_Begin);
                    Wheres.Add("a.OccDate>=?");
                    paraList.Add(be);
                }
                else if (string.IsNullOrEmpty(End_Begin) && !string.IsNullOrEmpty(Ser_End))
                {
                    //左区间
                    DateTime end = DateTime.Parse(Ser_End);
                    Wheres.Add("a.OccDate<=?");
                    paraList.Add(end);
                }


                if (!string.IsNullOrEmpty(End_Begin) && !string.IsNullOrEmpty(End_End))
                {
                    //区间内
                    DateTime be = DateTime.Parse(End_Begin);
                    DateTime end = DateTime.Parse(End_End);
                    Wheres.Add("a.EndDate>=?");
                    Wheres.Add("a.EndDate<=?");
                    paraList.Add(be);
                    paraList.Add(end);
                }
                else if (!string.IsNullOrEmpty(End_Begin) && string.IsNullOrEmpty(End_End))
                {
                    //右区间
                    DateTime be = DateTime.Parse(End_Begin);
                    Wheres.Add("a.EndDate>=?");
                    paraList.Add(be);
                }
                else if (string.IsNullOrEmpty(End_Begin) && !string.IsNullOrEmpty(End_End))
                {
                    //左区间
                    DateTime end = DateTime.Parse(End_End);
                    Wheres.Add("a.EndDate<=?");
                    paraList.Add(end);
                }
                if (Wheres.Count > 0)
                {
                    //连接条件
                    Where = string.Join(" AND ", Wheres.ToArray());
                }
                #endregion

                #region 处理请求
                string hql = "";
                if (Model == "0")
                {
                    if (Wheres.Count > 0)
                    {
                        hql = "SELECT NEW AcceptancesInfo(c.TypeName,SUM(a.LoanAmount),SUM(a.Repayed),SUM(a.Balance),SUM(a.Margin)) FROM AcceptancesInfo a,CompanyInfo b,TypeInfo c WHERE  a.Company=b.Company AND b.TypeID=c.id AND " + Where + " AND b.id in (" + user.RoleCmp + ") GROUP BY c.TypeName";
                    }
                    else
                    {
                        hql = "SELECT NEW AcceptancesInfo(c.TypeName,SUM(a.LoanAmount),SUM(a.Repayed),SUM(a.Balance),SUM(a.Margin)) FROM AcceptancesInfo a,CompanyInfo b,TypeInfo c WHERE  a.Company=b.Company AND b.TypeID=c.id AND b.id in (" + user.RoleCmp + ") GROUP BY c.TypeName";
                    }
                }
                else if (Model == "1")
                {
                    if (Wheres.Count > 0)
                    {
                        hql = "SELECT NEW AcceptancesInfo(a.Company,SUM(a.LoanAmount),SUM(a.Repayed),SUM(a.Balance),SUM(a.Margin)) FROM AcceptancesInfo a,CompanyInfo b,TypeInfo c WHERE  a.Company=b.Company AND b.TypeID=c.id AND a.Flag='未清' AND " + Where + " AND b.id in (" + user.RoleCmp + ") GROUP BY a.Company";
                    }
                    else
                    {
                        hql = "SELECT NEW AcceptancesInfo(a.Company,SUM(a.LoanAmount),SUM(a.Repayed),SUM(a.Balance),SUM(a.Margin)) FROM AcceptancesInfo a,CompanyInfo b,TypeInfo c WHERE a.Company=b.Company AND b.TypeID=c.id AND a.Flag='未清' AND b.id in (" + user.RoleCmp + ") GROUP BY a.Company";
                    }
                }
                else if (Model == "2")
                {
                    if (Wheres.Count > 0)
                    {
                        hql = "SELECT NEW AcceptancesInfo(a.id,a.Type,a.OccDate,a.Abstract,a.Rates,a.Company,a.Bank,a.LoanAmount,a.EndDate,a.Remarks,a.BuildDate,a.Flag,a.RepayAmount,a.Balance,a.Margin,a.Status,a.Repayed,a.Repayrecord,a.Userid,d.NickName,b.id,c.id,c.TypeName)FROM AcceptancesInfo a,CompanyInfo b,AdminInfo d,TypeInfo c WHERE a.Userid = d.id AND a.Company=b.Company AND b.TypeID=c.id AND "+ Where + " AND b.id in (" + user.RoleCmp + ") ORDER BY a.Company,a.Bank,a.OccDate";

                    }
                    else
                    {
                        hql = "SELECT NEW AcceptancesInfo(a.id,a.Type,a.OccDate,a.Abstract,a.Rates,a.Company,a.Bank,a.LoanAmount,a.EndDate,a.Remarks,a.BuildDate,a.Flag,a.RepayAmount,a.Balance,a.Margin,a.Status,a.Repayed,a.Repayrecord,a.Userid,d.NickName,b.id,c.id,c.TypeName)FROM AcceptancesInfo a,CompanyInfo b,AdminInfo d,TypeInfo c WHERE a.Userid = d.id AND a.Company=b.Company AND b.TypeID=c.id AND b.id in (" + user.RoleCmp + ") ORDER BY a.Company,a.Bank,a.OccDate";
                    }

                }


                SimpleQuery<AcceptancesInfo> Query = new SimpleQuery<AcceptancesInfo>(hql, paraList.ToArray());
                AcceptancesInfo[] Beans = Query.Execute();

                #endregion
                #region 返回数据

                //JsonConvert.SerializeObject(bean).ToString().Replace("\r\n", "")
                msg.Add("msg", "OK");
                msg.Add("data", JsonConvert.SerializeObject(Beans).ToString().Replace("\r\n", ""));
                return msg.ToString();
                #endregion
            }
            catch (Exception _e)
            {
                Log.Error("Home/Search_ServerRec4", _e);
                msg.Add("msg", "error");
                throw;
            }

        }

        #endregion

    }

}