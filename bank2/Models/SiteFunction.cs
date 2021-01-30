using Castle.ActiveRecord;
using Castle.ActiveRecord.Queries;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace bank2.Models
{
    [ActiveRecord("SiteFunction", DynamicInsert = true, DynamicUpdate = true)]
    public class SiteFunction : ActiveRecordBase
    {
        #region 构造方法

        public SiteFunction() { 
            
        }

        public SiteFunction(long id) {
            this.id = id;
        }

        public SiteFunction(long id,string SName,string Url,long ParentId,int SLevel,bool isShow) {
            this.id = id;
            this.SName = SName;
            this.Url = Url;
            this.ParentId = ParentId;
            this.SLevel = SLevel;
            this.isShow = isShow;
        }

        /// <summary>
        /// 返回一个功能列表JObject 树
        /// </summary>
        /// 显示ParentId - ChildId 的功能菜单
        /// <param name="ParentId">需要显示的第一次层级</param>
        /// <param name="ChildId">需要显示到哪一层级</param>
        /// <returns></returns>
        public static List<SiteFunction> getFunctionList(int ParentId,int ChildId) {
            string hql = "SELECT NEW SiteFunction(a.id,a.SName,a.Url,a.ParentId,a.SLevel,a.isShow)FROM SiteFunction a";
            SimpleQuery<SiteFunction> Query = new SimpleQuery<SiteFunction>(hql);
            SiteFunction[] beans = Query.Execute();
            List<SiteFunction> beans2 = null;
            List<SiteFunction> beans3 = beans.Select(t=> {
                if (t.SLevel<ParentId) {
                    return null;
                }
                else
                {
                    return t;
                }
            }).ToList();
            beans3.RemoveAll(t=>t==null);
            if (ParentId==0 && ChildId==0) {
                beans2 = getFunctionTree(ParentId, beans3, null, 999, ParentId);
            }
            else
            {
                beans2 = getFunctionTree(ParentId, beans3, null, ChildId, ParentId);
            }
            
            return beans2;
        }
        //获取用户所拥有的权限
        public static List<SiteFunction> getUserRole(string User_Role) {
            string hql = "SELECT NEW SiteFunction(a.id,a.SName,a.Url,a.ParentId,a.SLevel,a.isShow)FROM SiteFunction a";
            SimpleQuery<SiteFunction> Query = new SimpleQuery<SiteFunction>(hql);
            SiteFunction[] beans = Query.Execute();
            string[] Role = User_Role.Split(',');
            List<SiteFunction> beans2 = beans.Select(item =>
            {
                if (Role.Contains(item.id + "") || item.SLevel == 0)
                {
                    return item;
                }
                else
                {
                    return null;
                }
            }).ToList();
            beans2.RemoveAll(item => item == null);
            return beans2;
        }
        /// <summary>
        /// 创建侧边栏树
        /// </summary>
        /// <param name="beans"></param>
        /// <returns></returns>
        public static List<SiteFunction> getSideBar(List<SiteFunction> beans) {
            List<SiteFunction> beans2  = getFunctionTree(0,beans,null,2,0);
            return beans2;
        }
        /// <summary>
        /// 创建功能树
        /// level2 最后一层
        /// </summary>
        /// <param name="beans"></param>
        /// <param name="level">要找的最后一层</param>
        /// <param name="levelNow">当前层数</param>
        /// <returns></returns>
        public static List<SiteFunction> getFunctionTree(int FirstLevel,List<SiteFunction> beans,SiteFunction bean,int level,int levelNow) {
            if (levelNow>level) {
                return null;
            }
            else
            {
                var Finds = beans.Select(t=> {
                    if ((t.SLevel== FirstLevel && bean==null)||t.SLevel==levelNow && t.ParentId == bean.id ) {
                        return t;
                    }
                    else
                    {
                        return null;
                    }
                }).ToList();
                Finds.RemoveAll(t => t == null);
                foreach (var item in Finds) {
                    var FindItem = getFunctionTree(FirstLevel, beans,item,level,levelNow+1);
                    item.Child = FindItem;
                }
                return Finds;
            }
        }


        #endregion

        #region 属性
        /// <summary>
        ///主键
        /// </summary>
        [PrimaryKey(PrimaryKeyType.Native)]
        public long id { get; set; }

        /// <summary>
        /// 模块名称
        /// </summary>
        [Property()]
        public string SName { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        [Property()]
        public string Url { get; set; }

        /// <summary>
        /// 父功能
        /// </summary>
        [Property()]
        public long ParentId { get; set; }

        /// <summary>
        /// 所在层级
        /// </summary>
        [Property]
        public int SLevel { get; set; }

        /// <summary>
        /// 是否显示在页面上
        /// </summary>
        [Property]
        public bool isShow { get; set; }

        #endregion

        #region 额外属性

        /// <summary>
        /// 其下的所有子功能
        /// </summary>
        public List<SiteFunction> Child { get; set; }

        #endregion

        #region 实现方法

        public static void DeleteAll()
        {
            ActiveRecordBase.DeleteAll(typeof(SiteFunction));
        }

        public static SiteFunction[] FindAll()
        {
            return ((SiteFunction[])(ActiveRecordBase.FindAll(typeof(SiteFunction))));
        }

        public static SiteFunction Find(long Id)
        {
            return ((SiteFunction)(ActiveRecordBase.FindByPrimaryKey(typeof(SiteFunction), Id)));
        }

        #endregion


    }
}