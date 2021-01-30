using Castle.ActiveRecord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace bank2.Models
{
    /// <summary>
    /// 授信记录表
    /// </summary>
    [ActiveRecord("CreditInfo", DynamicInsert = true, DynamicUpdate = true)]
    public class CreditInfo : ActiveRecordBase
    {
        #region 构造方法

        public CreditInfo()
        {

        }

        public CreditInfo(long id)
        {
            this.id = id;
        }
        //SELECT NEW CreditInfo(a.id,a.Company,a.Bank,a.Credit,a.Acceptance,a.Loans,a.Arrears,a.Remarks,a.Builddate,a.Rates,a.Acceptanced,a.Loansed,a.Total,a.Userid,b.TypeID,c.TypeName) FROM CreditInfo a,CompanyInfo b,TypeInfo c WHERE a.Company=b.Company and b.TypeID = c.id
        public CreditInfo(long id,string Company,string Bank,decimal Credit,decimal Acceptance,decimal Loans,decimal Arrears,string Remarks,DateTime Builddate,string Rates,decimal Acceptanced,decimal Loansed,decimal Total,long Userid,int cmp_TypeId,string Type_Name)
        {
            this.id = id;
            this.Company = Company;
            this.Bank = Bank;
            this.Credit = Credit;
            this.Acceptance = Acceptance;
            this.Loans = Loans;
            this.Arrears = Arrears;
            this.Remarks = Remarks;
            this.Builddate = Builddate;
            this.Rates = Rates;
            this.Acceptanced = Acceptanced;
            this.Loansed = Loansed;
            this.Total = Total;
            this.Userid = Userid;
            this.cmp = new CompanyInfo
            {
                Company = Company,
                TypeID = (int)cmp_TypeId,
                SType = new TypeInfo() { 
                    id = (int)cmp_TypeId,
                    TypeName = Type_Name
                }
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
        /// 银行名称
        /// </summary>
        [Property()]
        public string Bank { get; set; }

        /// <summary>
        /// 授信金额
        /// </summary>
        [Property()]
        public decimal Credit { get; set; }

        /// <summary>
        /// 承兑额度
        /// </summary>
        [Property()]
        public decimal Acceptance { get; set; }

        /// <summary>
        /// 贷款额度
        /// </summary>
        [Property()]
        public decimal Loans { get; set; }

        /// <summary>
        /// 欠款总额
        /// </summary>
        [Property()]
        public decimal Arrears { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Property()]
        public string Remarks { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        [Property()]
        public DateTime? Builddate { get; set; }

        /// <summary>
        /// 贷款利率
        /// </summary>
        [Property()]
        public string Rates { get; set; }

        /// <summary>
        /// 剩余承兑额度
        /// </summary>
        [Property()]
        public decimal Acceptanced { get; set; }

        /// <summary>
        /// 剩余贷款额度
        /// </summary>
        [Property()]
        public decimal Loansed { get; set; }

        /// <summary>
        /// 停用
        /// </summary>
        [Property()]
        public decimal Total { get; set; }

        /// <summary>
        /// 操作员ID
        /// </summary>
        [Property()]
        public long Userid { get; set; }

        #endregion

        #region 额外属性
        /// <summary>
        /// 对应公司
        /// </summary>
        public CompanyInfo cmp { get; set; }

        #endregion

        #region 实现方法

        public static void DeleteAll()
        {
            ActiveRecordBase.DeleteAll(typeof(CreditInfo));
        }

        public static CreditInfo[] FindAll()
        {
            return ((CreditInfo[])(ActiveRecordBase.FindAll(typeof(CreditInfo))));
        }

        public static CreditInfo Find(long Id)
        {
            return ((CreditInfo)(ActiveRecordBase.FindByPrimaryKey(typeof(CreditInfo), Id)));
        }

        #endregion
    }
}