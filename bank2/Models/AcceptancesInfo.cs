using Castle.ActiveRecord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace bank2.Models
{
    /// <summary>
    /// 承兑记录表
    /// </summary>
    [ActiveRecord("AcceptancesInfo", DynamicInsert = true, DynamicUpdate = true)]
    public class AcceptancesInfo : ActiveRecordBase
    {
        #region 构造方法

        public AcceptancesInfo()
        {

        }

        public AcceptancesInfo(int id)
        {
            this.id = id;
        }

        public AcceptancesInfo(string TypeName, decimal Sum_LoanAmount, decimal Sum_Repayed, decimal Sum_Balance,decimal Sum_Margin)
        {
            this.cmp = new CompanyInfo()
            {
                SType = new TypeInfo()
                {
                    TypeName = TypeName
                }
            };
            this.Sum_LoanAmount = Sum_LoanAmount;
            this.Sum_Repayed = Sum_Repayed;
            this.Sum_Margin = Sum_Margin;
            this.Sum_Balance = Sum_Balance;
            
        }

        //SELECT NEW AcceptancesInfo(a.id,a.Type,a.OccDate,a.Abstract,a.Rates,a.Company,a.Bank,a.LoanAmount,a.EndDate,a.Remarks,a.BuildDate,a.Flag,a.RepayAmount,a.Balance,a.Margin,a.Status,a.Repayed,a.Repayrecord,a.Userid,c.NickName)FROM AcceptancesInfo a,CompanyInfo b,AdminInfo c ;
        //1                1                  1              1               1              1             1            1                 1                 1             1                 1              1             1                  1           1              1               1               1             1
        //System.Int64, System.String, System.DateTime, System.String, System.String, System.String, System.String, System.Decimal, System.DateTime, System.String, System.DateTime, System.String, System.Decimal, System.Decimal, System.Decimal, System.Int32, System.Decimal, System.String, System.Int32, System.String) found in class: bank2.Models.AcceptancesInfo"}	System.Exception {NHibernate.InstantiationException}
        //                          1       1              1             1               1              1            1              1                1               1                 1                  1               1                 1             1            1             1                 1             1                 1
        public AcceptancesInfo(long id,string Type,DateTime OccDate,string Abstract,string Rates,string Company,string Bank,decimal LoanAmount,DateTime EndDate,string Remarks, DateTime BuildDate, string Flag,decimal RepayAmount,decimal Balance,decimal Margin,int Status, decimal Repayed,string Repayrecord,int Userid,string User_Name, long Cmp_id, long Type_id, string Type_Name)
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
            this.Margin = Margin;
            this.Status = Status;
            this.Repayed = Repayed;
            this.Repayrecord = Repayrecord;
            if (!string.IsNullOrWhiteSpace(Repayrecord)) {
                var flg = false;
                if (Repayrecord.IndexOf('/')>=0&& Repayrecord.IndexOf(',')>=0) {
                    flg = true;
                }
                string[] strs = Repayrecord.Split('/');
                for (int i= 0;i<strs.Length&&flg; i++) {
                    if (!string.IsNullOrWhiteSpace(strs[i])) {
                        string[] strs2 = strs[i].Split(',');
                        this.ReplyRecord += decimal.Parse(strs2[1]).ToString("f4")+";";
                    }
                }
            }


            this.Userid = Userid;
            this.User = new AdminInfo()
            {
                id = Userid,
                NickName = User_Name
            };
            this.CompanyAndBnak = "企业：" + Company + "，银行：" + Bank;
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

        public AcceptancesInfo(string Type_Name,string Company,decimal LM_Balance) {
            this.cmp = new CompanyInfo()
            {
                Company = Company,
                SType = new TypeInfo()
                {
                    TypeName = Type_Name
                }
            };
            this.LM_Balance = LM_Balance;
        }

        #endregion


        #region 属性

        /// <summary>
        ///主键
        /// </summary>
        [PrimaryKey(PrimaryKeyType.Native)]
        public long id { get; set; }

        /// <summary>
        /// 资金操作类别
        /// </summary>
        [Property()]
        public string Type { get; set; }

        /// <summary>
        /// 业务发生日期
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
        /// 创建日期
        /// </summary>
        [Property()]
        public DateTime? BuildDate { get; set; }

        /// <summary>
        /// 贷款业务状态
        /// </summary>
        [Property()]
        public string Flag { get; set; }

        /// <summary>
        /// 本次还款金额
        /// </summary>
        [Property()]
        public decimal RepayAmount { get; set; }

        /// <summary>
        /// 未清余额
        /// </summary>
        [Property()]
        public decimal Balance { get; set; }

        /// <summary>
        /// 保证金
        /// </summary>
        [Property()]
        public decimal Margin { get; set; }

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
        /// </summary>
        [Property()]
        public string Repayrecord { get; set; }

        /// <summary>
        /// 操作用户Id
        /// </summary>
        [Property()]
        public int Userid { get; set; }

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
        /// 根据Repayrecord 生成 xx;xx 数据 显示还款金额
        /// </summary>
        public string ReplyRecord { get; set; }

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
        /// 总保证金
        /// </summary>
        public decimal Sum_Margin { get; set; }

        /// <summary>
        /// 总余额
        /// </summary>
        public decimal Sum_Balance { get; set; }

        #endregion

        #region 实现方法

        public static void DeleteAll()
        {
            ActiveRecordBase.DeleteAll(typeof(AcceptancesInfo));
        }

        public static AcceptancesInfo[] FindAll()
        {
            return ((AcceptancesInfo[])(ActiveRecordBase.FindAll(typeof(AcceptancesInfo))));
        }

        public static AcceptancesInfo Find(long Id)
        {
            return ((AcceptancesInfo)(ActiveRecordBase.FindByPrimaryKey(typeof(AcceptancesInfo), Id)));
        }

        #endregion
    }
}