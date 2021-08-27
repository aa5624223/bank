using Models.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace bank2.Models
{
    /// <summary>
    /// 系统日志
    /// </summary>
    public class LogInfo
    {

        #region 属性

        /// <summary>
        /// 主键
        /// </summary>
        [Property("PK")]
        public long ID { get; set; }

        /// <summary>
        /// 操作类型
        /// </summary>
        [Property()]
        public string LogType { get; set; }

        /// <summary>
        /// 操作内容
        /// </summary>
        [Property()]
        public string Contents { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        [Property()]
        public DateTime Datetime1 { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        [Property()]
        public string UserCode { get; set; }

        /// <summary>
        /// 操作的对象
        /// ,间隔
        /// </summary>
        [Property()]
        public string IDS { get; set; }

        #endregion

    }
}