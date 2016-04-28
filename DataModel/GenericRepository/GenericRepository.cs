using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.GenericRepository
{
    /// <summary>
    /// Generic Repository class for Entity Operations
    /// 用来操作实体的泛型仓储类
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
   public  class GenericRepository<TEntity> where TEntity:class
    {
        #region 私有成员变量
        internal WebApiDbEntities Context;  //数据上下文对象变量
        internal DbSet<TEntity> DbSet;     //返回值类型和泛型类的类型参数一样
        #endregion

       /// <summary>
       /// 构造函数初始化本地私有变量
       /// </summary>
       /// <param name="context"></param>
        public GenericRepository(WebApiDbEntities context)
        {
            this.Context = context;
            this.DbSet = context.Set<TEntity>();

        }
       /// <summary>
       /// 泛型方法，Get()
       /// </summary>
       /// <returns></returns>
        public virtual IEnumerable<TEntity> Get()
        {
            IQueryable<TEntity> query = DbSet;
            return query.ToList();
        }

       /// <summary>
       /// 根据ID查询
       /// </summary>
       /// <param name="id"></param>
       /// <returns></returns>
        public virtual TEntity GetByID(object id)
        {
            return DbSet.Find(id);
        }

       /// <summary>
       /// 增加+Insert()
       /// </summary>
       /// <param name="model"></param>
 
       public virtual void Insert(TEntity model)
        {
             DbSet.Add(model);
        }

       /// <summary>
       /// 删除+Delete
       /// 先根据id查找到实体，在删除
       /// </summary>
       /// <param name="model"></param>
       public virtual void Delete(object id)
       {
           TEntity entity=  DbSet.Find(id);
           Delete(entity);
       }

       /// <summary>
       /// 设置实体的状态为Deleted
       /// </summary>
       /// <param name="deleteEntity"></param>
       public virtual void Delete(TEntity deleteEntity)
       {
           //实体的状态默认是Detached
           if (Context.Entry(deleteEntity).State == EntityState.Detached)
           {
               //加载到数据上下文中
               DbSet.Attach(deleteEntity);
           }
           //将实体的状态设置为Deleteed
           DbSet.Remove(deleteEntity);
       }








    }
}
