using Castle.ActiveRecord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace bank2.Models
{
    [ActiveRecord("AdminInfo", DynamicInsert = true, DynamicUpdate = true)]
    public class AdminInfo : ActiveRecordBase
    {
        #region 构造方法

        public AdminInfo() {

        }

        public AdminInfo(long id)
        {
            this.id = id;
        }

        //hql = "SELECT NEW AdminInfo(a.id,a.DptID,a.CmpID,a.Username,a.NickName,a.RoleCmp,a.RoleConfig,a.SiteConfig) FROM AdminInfo a";
        public AdminInfo(long id,int DptId,int CmpID,string Username, string NickName,string RoleCmp,string RoleConfig,string SiteConfig) {
            this.id = id;
            this.DptID = DptId;
            this.CmpID = CmpID;
            this.Username = Username;
            this.NickName = NickName;
            this.RoleCmp = RoleCmp;
            this.RoleConfig = RoleConfig;
            this.SiteConfig = SiteConfig;
        }

        public AdminInfo(long id, string Username,string Password,string NickName) {
            this.id = id;
            this.Username = Username;
            this.Password = Password;
            this.NickName = NickName;
        }
        //hql = "SELECT NEW AdminInfo(a.id,a.Username,a.DptID,a.CmpID,a.WorkID,a.Password,a.RoleCmp,a.Role,a.RoleConfig,a.NickName,a.Userid,a.SiteConfig) FROM AdminInfo a";
        public AdminInfo(long id,string Username,int DptID,int CmpID,string WorkID,string Password,string RoleCmp,int Role,string RoleConfig,string NickName,int Userid,string SiteConfig) {
            this.id = id;
            this.Username = Username;
            this.DptID = DptID;
            this.CmpID = CmpID;
            this.WorkID = WorkID;
            this.Password = Password;
            this.RoleCmp = RoleCmp;
            this.Role = Role;
            this.RoleConfig = RoleConfig;
            this.NickName = NickName;
            this.Userid = Userid;
            this.SiteConfig = SiteConfig;
        }

        #endregion

        #region 属性

        /// <summary>
        ///主键
        /// </summary>
        [PrimaryKey(PrimaryKeyType.Native)]
        public long id { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        [Property()]
        public string Username { get; set; }

        /// <summary>
        /// 部门id
        /// </summary>
        [Property()]
        public int DptID { get; set; }

        /// <summary>
        /// 公司id
        /// </summary>
        [Property()]
        public int CmpID { get; set; }

        /// <summary>
        /// 工号
        /// </summary>
        [Property()]
        public string WorkID { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Property()]
        public string Password { get; set; }

        /// <summary>
        /// 公司权限
        /// </summary>
        [Property()]
        public string RoleCmp { get; set; }

        /// <summary>
        /// 是否管理员
        /// 1 为管理员
        /// </summary>
        [Property()]
        public int Role { get; set; }

        /// <summary>
        /// 操作权限
        /// </summary>
        [Property()]
        public string RoleConfig { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        [Property()]
        public string NickName { get; set; }

        /// <summary>
        /// 停用
        /// </summary>
        [Property()]
        public int Userid { get; set; }

        [Property()]
        public string SiteConfig { get; set; }

        ///// <summary>
        ///// 物料计划表
        ///// </summary>
        //[BelongsTo("PId")]
        //public lw_MatrilPlan Plan { get; set; }

        #endregion

        #region 额外属性


        #endregion

        #region 实现方法

        public static void DeleteAll()
        {
            ActiveRecordBase.DeleteAll(typeof(AdminInfo));
        }

        public static AdminInfo[] FindAll()
        {
            return ((AdminInfo[])(ActiveRecordBase.FindAll(typeof(AdminInfo))));
        }

        public static AdminInfo Find(long Id)
        {
            return ((AdminInfo)(ActiveRecordBase.FindByPrimaryKey(typeof(AdminInfo), Id)));
        }

        #endregion
    }
}