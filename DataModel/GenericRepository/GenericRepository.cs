using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

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

       /// <summary>
       /// 更新--设置实体的状态为已修改
       /// </summary>
       /// <param name="model"></param>
       public virtual void Update(TEntity model)
       { 
           //把实体加载到数据上下文中
           DbSet.Attach(model);

           //设置实体的状态为Modified
           Context.Entry(model).State=EntityState.Modified;
       }

       /// <summary>
       /// 根据条件查询更多,转化为List
       /// </summary>
       /// <param name="where"></param>
       /// <returns></returns>
       public virtual IEnumerable<TEntity> GetMany(Func<TEntity, bool> where)
       {
           return DbSet.Where(where).ToList();
       }

       /// <summary>
       /// 根据条件查询更多，转化为IQueryable
       /// </summary>
       /// <param name="where"></param>
       /// <returns></returns>
       public virtual IQueryable<TEntity> GetManyAsQueryable(Func<TEntity, bool> where)
       {
           return DbSet.Where(where).AsQueryable();
       }

       /// <summary>
       /// 根据条件查询单个的实体TEntity
       /// </summary>
       /// <param name="where"></param>
       /// <returns></returns>
       public TEntity Get(Func<TEntity,bool> where)
       {
           return DbSet.Where(where).FirstOrDefault<TEntity>();
       }

      /// <summary>
      /// 根据条件删除
      /// </summary>
      /// <param name="deleteWhere">要删除的</param>
       public void Delete(Func<TEntity, bool> deleteWhere)
       {
           IQueryable<TEntity> entityList = DbSet.Where(deleteWhere).AsQueryable();
           foreach (TEntity item in entityList)
           {
               DbSet.Remove(item);
           }
       }

       /// <summary>
       /// 查询所有
       /// </summary>
       /// <returns></returns>
       public virtual IEnumerable<TEntity> GetAll()
       {
           return DbSet.ToList();
       }

       /// <summary>
       /// 多表查询使用Include
       /// </summary>
       /// <param name="predicate"></param>
       /// <param name="include"></param>
       /// <returns></returns>
       public IQueryable<TEntity> GetWithInclude(Func<TEntity, bool> predicate, params string[] include)
       {
           IQueryable<TEntity> query = this.DbSet;
           query = include.Aggregate(query, (current, inc) => current.Include(inc));
           return query.Where(predicate).AsQueryable();
       }

       /// <summary>
       /// 判断实体是否存在
       /// true--表示存在
       /// </summary>
       /// <param name="primaryKey"></param>
       /// <returns></returns>
       public bool Exist(object primaryKey)
       {
           return DbSet.Find(primaryKey) != null;
       }

       /// <summary>
       /// 获取单个实体
       /// </summary>
       /// <param name="where"></param>
       /// <returns></returns>
       public TEntity GetSingel(Func<TEntity, bool> where)
       {
           return DbSet.Single<TEntity>(where);
       }

       /// <summary>
       /// 查询第一个实体
       /// </summary>
       /// <param name="where"></param>
       /// <returns></returns>
       public TEntity GetFirst(Func<TEntity, bool> where)
       {
           return DbSet.First<TEntity>(where);
       }

    }
}
