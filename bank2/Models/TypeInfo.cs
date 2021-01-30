using Castle.ActiveRecord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace bank2.Models
{
    /// <summary>
    /// 信息分类表
    /// </summary>
    [ActiveRecord("TypeInfo", DynamicInsert = true, DynamicUpdate = true)]
    public class TypeInfo : ActiveRecordBase
    {
        #region 构造方法

        public TypeInfo()
        {

        }

        public TypeInfo(int id)
        {
            this.id = id;
        }
        //hql = "SELECT NEW TypeInfo(a.id,a.TableName,a.TypeName,a.ParentID,a.SortID,a.Userid) FROM TypeInfo a
        public TypeInfo(long id,string TableName,string TypeName,long ParentID,string SortID,int Userid)
        {
            this.id = id;
            this.TableName = TableName;
            this.TypeName = TypeName;
            this.ParentID = ParentID;
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
        /// 用量
        /// </summary>
        [Property()]
        public string TableName { get; set; }

        /// <summary>
        /// 用量
        /// </summary>
        [Property()]
        public string TypeName { get; set; }

        /// <summary>
        /// 用量
        /// </summary>
        [Property()]
        public long ParentID { get; set; }

        /// <summary>
        /// 用量
        /// </summary>
        [Property()]
        public string SortID { get; set; }

        /// <summary>
        /// 用量
        /// </summary>
        [Property()]
        public int Userid { get; set; }


        #endregion

        #region 额外属性

        public List<CompanyInfo> CompanyList { get; set; }

        #endregion

        #region 实现方法

        public static void DeleteAll()
        {
            ActiveRecordBase.DeleteAll(typeof(TypeInfo));
        }

        public static TypeInfo[] FindAll()
        {
            return ((TypeInfo[])(ActiveRecordBase.FindAll(typeof(TypeInfo))));
        }

        public static TypeInfo Find(long Id)
        {
            return ((TypeInfo)(ActiveRecordBase.FindByPrimaryKey(typeof(TypeInfo), Id)));
        }

        #endregion
    }
}