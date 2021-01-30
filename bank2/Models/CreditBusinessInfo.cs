using Castle.ActiveRecord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace bank2.Models
{
    /// <summary>
    /// 借款业务记录表
    /// </summary>
    [ActiveRecord("CreditBusinessInfo", DynamicInsert = true, DynamicUpdate = true)]
    public class CreditBusinessInfo : ActiveRecordBase
    {
        #region 构造方法

        public CreditBusinessInfo()
        {

        }

        public CreditBusinessInfo(int id)
        {
            this.id = id;
        }
        /// <summary>
        /// 
        /// SELECT NEW CreditBusinessInfo(c.TypeName,SUM(a.LoanAmount),SUM(a.RepayAmount),SUM(a.Balance)) FROM CreditBusinessInfo a,CompanyInfo b,TypeInfo c WHERE a.Company=b.Company AND b.TypeID=c.id " + Where +" Group by c.TypeName";
        /// 
        /// </summary>
        /// <param name="TypeName"></param>
        /// <param name="Sum_LoanAmount"></param>
        /// <param name="Sum_Repayed"></param>
        /// <param name="Sum_Balance"></param>
        public CreditBusinessInfo(string TypeName, decimal Sum_LoanAmount,decimal Sum_Repayed, decimal Sum_Balance) {
            this.cmp = new CompanyInfo()
            {
                SType = new TypeInfo()
                {
                    TypeName = TypeName
                }
            };
            this.Sum_LoanAmount = Sum_LoanAmount;
            this.Sum_Repayed = Sum_Repayed;
            this.Sum_Balance = Sum_Balance;
        }

        //SELECT NEW CreditBusinessInfo(a.id,a.Type,a.OccDate,a.Abstract,a.Rates,a.Company,a.Bank,a.LoanAmount,a.EndDate,a.Remarks,a.BuildDate,a.Flag,a.RepayAmount,a.Balance,a.Status,a.Repayed,a.Repayrecord,a.Userid,c.NickName)FROM CreditBusinessInfo a
        public CreditBusinessInfo(long id,string Type,DateTime OccDate,string Abstract,string Rates,string Company,string Bank,decimal LoanAmount,DateTime EndDate,string Remarks,DateTime BuildDate,string Flag,decimal RepayAmount,decimal Balance,int Status,decimal Repayed,string Repayrecord,long Userid,string User_Name,long Cmp_id,long Type_id,string Type_Name)
        {
            this.id = id;
            this.Type = Type;
            this.OccDate = OccDate;
            this.Abstract = Abstract;
            this.Rates = Rates;
            this.Company = Company;
            this.Bank = Bank;
            this.LoanAmount = LoanAmount;
            this.EndDate = EndDate;
            this.Remarks = Remarks;
            this.BuildDate = BuildDate;
            this.Flag = Flag;
            this.RepayAmount = RepayAmount;
            this.Balance = Balance;
            this.Status = Status;
            this.Repayed = Repayed;
            this.Repayrecord = Repayrecord;
            this.Userid = Userid;
            this.User = new AdminInfo()
            {
                id = Userid,
                NickName = User_Name
            };
            this.CompanyAndBnak = "企业："+Company+"，银行："+Bank;
            this.cmp = new CompanyInfo()
            {
                id = Cmp_id,
                Company = Company,
                SType = new TypeInfo()
                {
                    id = Type_id,
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
        /// 类别 还/贷
        /// </summary>
        [Property()]
        public string Type { get; set; }

        /// <summary>
        /// 业务创建日期
        /// </summary>
        [Property()]
        public DateTime? OccDate { get; set; }

        /// <summary>
        /// 摘要
        /// </summary>
        [Property()]
        public string Abstract { get; set; }

        /// <summary>
        /// 利率
        /// </summary>
        [Property()]
        public string Rates { get; set; }

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
        /// 贷款金额
        /// </summary>
        [Property()]
        public decimal LoanAmount { get; set; }

        /// <summary>
        /// 到期日期
        /// </summary>
        [Property()]
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 备注信息
        /// </summary>
        [Property()]
        public string Remarks { get; set; }

        /// <summary>
        /// 创建日期 YYYY-MM-DD
        /// </summary>
        [Property()]
        public DateTime? BuildDate { get; set; }

        /// <summary>
        /// 贷款业务状态 未清/已清
        /// </summary>
        [Property()]
        public string Flag { get; set; }

        /// <summary>
        /// 本次还款金额 还款业务
        /// </summary>
        [Property()]
        public decimal RepayAmount { get; set; }

        /// <summary>
        /// 未清余额
        /// </summary>
        [Property()]
        public decimal Balance { get; set; }

        /// <summary>
        /// 是否有还款记录
        /// </summary>
        [Property()]
        public int Status { get; set; }

        /// <summary>
        /// 已还金额
        /// </summary>
        [Property()]
        public decimal Repayed { get; set; }

        /// <summary>
        /// 还款对照记录
        /// 借款记录ID、本次还款金额/借款记录ID，本次还款金额
        /// </summary>
        [Property()]
        public string Repayrecord { get; set; }

        /// <summary>
        /// 操作用户ID
        /// </summary>
        [Property()]
        public long Userid { get; set; }

        #endregion

        #region 额外属性

        public AdminInfo User { get; set; }

        /// <summary>
        /// 企业：xxx,银行：xxx
        /// </summary>
        public string CompanyAndBnak { get; set; }

        /// <summary>
        /// 对应公司
        /// </summary>
        public CompanyInfo cmp { get; set; }

        /// <summary>
        /// 下个月到期金额
        /// </summary>
        public decimal LM_Balance { get; set; }

        /// <summary>
        /// 借款总额
        /// </summary>
        public decimal Sum_LoanAmount { get; set; }

        /// <summary>
        /// 已还总额
        /// </summary>
        public decimal Sum_Repayed { get; set; }

        /// <summary>
        /// 总余额
        /// </summary>
        public decimal Sum_Balance { get; set; }

        #endregion

        #region 实现方法

        public static void DeleteAll()
        {
            ActiveRecordBase.DeleteAll(typeof(CreditBusinessInfo));
        }

        public static CreditBusinessInfo[] FindAll()
        {
            return ((CreditBusinessInfo[])(ActiveRecordBase.FindAll(typeof(CreditBusinessInfo))));
        }

        public static CreditBusinessInfo Find(long Id)
        {
            return ((CreditBusinessInfo)(ActiveRecordBase.FindByPrimaryKey(typeof(CreditBusinessInfo), Id)));
        }

        #endregion
    }
}