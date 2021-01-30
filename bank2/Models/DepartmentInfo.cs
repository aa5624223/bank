using Castle.ActiveRecord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace bank2.Models
{
    /// <summary>
    /// 部门表
    /// </summary>
    [ActiveRecord("DepartmentInfo", DynamicInsert = true, DynamicUpdate = true)]
    public class DepartmentInfo : ActiveRecordBase
    {
        #region 构造方法

        public DepartmentInfo()
        {

        }

        public DepartmentInfo(long id)
        {
            this.id = id;
        }
        //System.Int64, System.Int32, System.String, System.String, System.Int32
        public DepartmentInfo(long id,int CmpID,string DptName,string SortID,int Userid)
        {
            this.id = id;
            this.CmpID = (int)CmpID;
            this.DptName = DptName;
            this.SortID = SortID;
            this.Userid = (int)Userid;
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
        /// 公司ID
        /// </summary>
        [Property()]
        public int CmpID { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        [Property()]
        public string DptName { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [Property()]
        public string SortID { get; set; }

        /// <summary>
        /// 操作人ID
        /// </summary>
        [Property()]
        public int Userid { get; set; }


        #endregion

        #region 额外属性


        #endregion

        #region 实现方法

        public static void DeleteAll()
        {
            ActiveRecordBase.DeleteAll(typeof(DepartmentInfo));
        }

        public static DepartmentInfo[] FindAll()
        {
            return ((DepartmentInfo[])(ActiveRecordBase.FindAll(typeof(DepartmentInfo))));
        }

        public static DepartmentInfo Find(long Id)
        {
            return ((DepartmentInfo)(ActiveRecordBase.FindByPrimaryKey(typeof(DepartmentInfo), Id)));
        }

        #endregion
    }
}