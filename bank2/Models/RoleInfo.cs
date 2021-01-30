using Castle.ActiveRecord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace bank2.Models
{
    /// <summary>
    /// 程序页面表
    /// </summary>
    [ActiveRecord("RoleInfo2", DynamicInsert = true, DynamicUpdate = true)]
    public class RoleInfo : ActiveRecordBase
    {
        #region 构造方法

        public RoleInfo()
        {

        }

        public RoleInfo(long id)
        {
            this.id = id;
        }

        public RoleInfo(long id,string Title,string Control) {
            this.id = id;
            this.Title = Title;
            this.Control = Control;
        }

        public RoleInfo(long id,string Title,string Classname,int Classid,string Control,string Url,int sortid)
        {
            this.id = id;
            this.Title = Title;
            this.Classname = Classname;
            this.Classid = Classid;
            this.Control = Control;
            this.Url = Url;
            this.sortid = sortid;
        }

        #endregion


        #region 属性

        /// <summary>
        ///主键
        /// </summary>
        [PrimaryKey(PrimaryKeyType.Native)]
        public long id { get; set; }

        /// <summary>
        /// 用量
        /// </summary>
        [Property()]
        public string Title { get; set; }

        /// <summary>
        /// 用量
        /// </summary>
        [Property()]
        public string Classname { get; set; }

        /// <summary>
        /// 用量
        /// </summary>
        [Property()]
        public int Classid { get; set; }

        /// <summary>
        /// 用量
        /// </summary>
        [Property()]
        public string Control { get; set; }

        /// <summary>
        /// 用量
        /// </summary>
        [Property()]
        public string Url { get; set; }

        /// <summary>
        /// 用量
        /// </summary>
        [Property()]
        public int sortid { get; set; }

        #endregion

        #region 额外属性


        #endregion

        #region 实现方法

        public static void DeleteAll()
        {
            ActiveRecordBase.DeleteAll(typeof(RoleInfo));
        }

        public static RoleInfo[] FindAll()
        {
            return ((RoleInfo[])(ActiveRecordBase.FindAll(typeof(RoleInfo))));
        }

        public static RoleInfo Find(long Id)
        {
            return ((RoleInfo)(ActiveRecordBase.FindByPrimaryKey(typeof(RoleInfo), Id)));
        }

        #endregion
    }
}