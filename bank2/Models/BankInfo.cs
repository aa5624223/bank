using Castle.ActiveRecord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace bank2.Models
{
    /// <summary>
    /// 银行表
    /// </summary>
    [ActiveRecord("BankInfo", DynamicInsert = true, DynamicUpdate = true)]
    public class BankInfo : ActiveRecordBase
    {
        #region 构造方法

        public BankInfo()
        {

        }
        
        public BankInfo(int id)
        {
            this.id = id;
        }
        //hql = "SELECT NEW BankInfo(a.id,a.Bank) FROM BankInfo a
        public BankInfo(long id,string Bank)
        {
            this.id = id;
            this.Bank = Bank;
        }

        public BankInfo(long id,string Bank,int TypeID,DateTime BuildDate,string Remarks,string SortID,int Userid,long Type_id,string Type_Name)
        {
            this.id = id;
            this.Bank = Bank;
            this.TypeID = TypeID;
            this.BuildDate = BuildDate;
            this.Remarks = Remarks;
            this.SortID = SortID;
            this.Userid = Userid;
            this.SType = new TypeInfo()
            {
                id = Type_id,
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
        /// 银行分行名称
        /// </summary>
        [Property()]
        public string Bank { get; set; }

        /// <summary>
        /// 大类Id
        /// </summary>
        [Property()]
        public int TypeID { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        [Property()]
        public DateTime? BuildDate { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Property()]
        public string Remarks { get; set; }

        /// <summary>
        /// 排序Id
        /// </summary>
        [Property()]
        public string SortID { get; set; }

        /// <summary>
        /// 操作人Id
        /// </summary>
        [Property()]
        public int Userid { get; set; }

        #endregion

        #region 额外属性
        
        public TypeInfo SType { get; set; }

        #endregion

        #region 实现方法

        public static void DeleteAll()
        {
            ActiveRecordBase.DeleteAll(typeof(BankInfo));
        }

        public static BankInfo[] FindAll()
        {
            return ((BankInfo[])(ActiveRecordBase.FindAll(typeof(BankInfo))));
        }

        public static BankInfo Find(long Id)
        {
            return ((BankInfo)(ActiveRecordBase.FindByPrimaryKey(typeof(BankInfo), Id)));
        }

        #endregion
    }
}