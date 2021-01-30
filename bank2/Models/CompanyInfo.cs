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
    [ActiveRecord("CompanyInfo", DynamicInsert = true, DynamicUpdate = true)]
    public class CompanyInfo : ActiveRecordBase
    {
        #region 构造方法

        public CompanyInfo()
        {

        }

        public CompanyInfo(int id)
        {
            this.id = id;
        }
        //SELECT NEW CompanyInfo(a.id,a.Company) FROM CompanyInfo a
        public CompanyInfo(long id, string Company) {
            this.id = id;
            this.Company = Company;
        }
        public CompanyInfo(long id, string Company,long TypeID,string Type_Name)
        {
            this.id = id;
            this.Company = Company;
            this.SType = new TypeInfo()
            {
                id = TypeID,
                TypeName = Type_Name
            };
        }

        //hql = "SELECT NEW CompanyInfo(a.id,a.Company,a.TypeID) FROM CompanyInfo a";
        public CompanyInfo(long id,string Company,int TypeID) {
            this.id = id;
            this.Company = Company;
            this.TypeID = TypeID;
        }
        //
        //hql = "SELECT NEW CompanyInfo(a.id,a.Company,a.TypeID,a.Builddate,a.Remarks,a.SortID,a.Userid,b.id,b.TypeName) FROM CompanyInfo a LEFT JOIN TypeInfo b ON a.TypeID=b.id 
        //      1           1           1               1               1           1               1
        //System.Int64, System.String, System.Int32, System.DateTime, System.String, System.String, System.Int32) found in class: bank2.Models.CompanyInfo"}	System.Exception {NHibernate.InstantiationException}
        //                      1       1           1           1                   1           1               1
        public CompanyInfo(long id,string Company,int TypeID,DateTime Builddate,string Remarks,string SortID,int Userid,long Type_Id,string Type_Name)
        {
            this.id = id;
            this.Company = Company;
            this.TypeID = TypeID;
            this.Builddate = Builddate;
            this.Remarks = Remarks;
            this.SortID = SortID;
            this.Userid = Userid;
            this.SType = new TypeInfo()
            {
                id = Type_Id,
                TypeName = Type_Name
            };
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
        /// 公司大类
        /// </summary>
        [Property()]
        public int TypeID { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        [Property()]
        public DateTime? Builddate { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Property()]
        public string Remarks { get; set; }

        /// <summary>
        /// 排序id
        /// </summary>
        [Property()]
        public string SortID { get; set; }

        /// <summary>
        /// 操作员Id
        /// </summary>
        [Property()]
        public int Userid { get; set; }
        /// <summary>
        /// 类别
        /// </summary>
        public TypeInfo SType { get; set; }

        #endregion

        #region 额外属性

        public List<string> BankList { get; set; }

        #endregion

        #region 实现方法

        public static void DeleteAll()
        {
            ActiveRecordBase.DeleteAll(typeof(CompanyInfo));
        }

        public static CompanyInfo[] FindAll()
        {
            return ((CompanyInfo[])(ActiveRecordBase.FindAll(typeof(CompanyInfo))));
        }

        public static CompanyInfo Find(long Id)
        {
            return ((CompanyInfo)(ActiveRecordBase.FindByPrimaryKey(typeof(CompanyInfo), Id)));
        }

        #endregion

    }
}