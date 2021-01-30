using Castle.ActiveRecord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace bank2.Models
{
    /// <summary>
    /// 公司表
    /// </summary>
    [ActiveRecord("CmpInfo", DynamicInsert = true, DynamicUpdate = true)]
    public class CmpInfo : ActiveRecordBase
    {
        #region 构造方法

        public CmpInfo()
        {

        }

        public CmpInfo(long id)
        {
            this.id = id;
        }

        public CmpInfo(long id,string Company,string SortID, int Userid)
        {
            this.id = id;
            this.Company = Company;
            this.SortID = SortID;
            this.Userid = Userid;
        }

        #endregion


        #region 属性

        /// <summary>
        ///主键
        /// </summary>
        [PrimaryKey(PrimaryKeyType.Native)]
        public long id { get; set; }

        /// <summary>
        /// 公司名称
        /// </summary>
        [Property()]
        public string Company { get; set; }

        /// <summary>
        /// 排序Id
        /// </summary>
        [Property()]
        public string SortID { get; set; }

        /// <summary>
        /// 操作员Id
        /// </summary>
        [Property()]
        public int Userid { get; set; }

        #endregion

        #region 额外属性


        #endregion

        #region 实现方法

        public static void DeleteAll()
        {
            ActiveRecordBase.DeleteAll(typeof(CmpInfo));
        }

        public static CmpInfo[] FindAll()
        {
            return ((CmpInfo[])(ActiveRecordBase.FindAll(typeof(CmpInfo))));
        }

        public static CmpInfo Find(long Id)
        {
            return ((CmpInfo)(ActiveRecordBase.FindByPrimaryKey(typeof(CmpInfo), Id)));
        }

        #endregion
    }
}