using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Tags
{
    public class Property : Attribute
    {
        public Property() { 
            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="PType">PK主键，FK外键</param>
        public Property(string PType) {
            this.PType = PType;
        }
        /// <summary>
        /// 如果是string类型
        /// 0:精确匹配 1:模糊匹配
        /// 如果是数字、日期类型
        /// 0:精确匹配 1:小于等于 2:大于等于
        /// </summary>
        /// <param name="isDateString">1.字符串类型时间</param>
        public Property(int StringModel) {
            this.StringModel = StringModel;
        }


        /// <summary>
        /// 主键PK,外键FK
        /// </summary>
        public string PType { get; set; }

        /// <summary>
        /// 
        /// 如果是string类型
        /// 0:精确匹配 1:模糊匹配 2:in 匹配
        /// 如果是数字、日期类型
        /// 0:精确匹配 1:小于等于 2:大于等于 3:不匹配 
        /// 数字的值为-1不查询
        /// </summary>
        public int Model { get; set; }

        /// <summary>
        /// 当string
        /// Model:2时,选这个做字段
        /// </summary>
        public string Colums { get; set; }

        /// <summary>
        /// 0.普通字符串
        /// 1.时间类字符串
        /// </summary>
        public int StringModel { get; set; }

        /// <summary>
        /// 不加入到select字段内
        /// </summary>
        public bool NotSelect { get; set; }

    }
}
