using Castle.ActiveRecord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace bank2.Models
{
    [ActiveRecord("ModuleInfo", DynamicInsert = true, DynamicUpdate = true)]
    public class ModuleInfo : ActiveRecordBase
    {
        #region 构造方法

        public ModuleInfo()
        {

        }

        public ModuleInfo(int id)
        {
            this.id = id;
        }

        public ModuleInfo(int id,string Modulename,string Url)
        {
            this.id = id;
            this.Modulename = Modulename;
            this.Url = Url;
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
        public string Modulename { get; set; }

        /// <summary>
        /// 连接
        /// </summary>
        [Property()]
        public string Url { get; set; }

        #endregion

        #region 额外属性


        #endregion

        #region 实现方法

        public static void DeleteAll()
        {
            ActiveRecordBase.DeleteAll(typeof(ModuleInfo));
        }

        public static ModuleInfo[] FindAll()
        {
            return ((ModuleInfo[])(ActiveRecordBase.FindAll(typeof(ModuleInfo))));
        }

        public static ModuleInfo Find(long Id)
        {
            return ((ModuleInfo)(ActiveRecordBase.FindByPrimaryKey(typeof(ModuleInfo), Id)));
        }

        #endregion
    }
}