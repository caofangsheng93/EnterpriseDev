using DataModel.GenericRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Text;

namespace DataModel.UnitOfWork
{
       /// <summary>
      /// 工作单元，主要是负责数据库的事务处理
      /// To give a heads up, again from my existing article, the important responsibilities of Unit of Work are,
      /// To manage transactions.
      ///To order the database inserts, deletes, and updates.
      ///To prevent duplicate updates. Inside a single usage of a Unit of Work object, 
     ///different parts of the code may mark the same Invoice object as changed, 
     ///but the Unit of Work class will only issue a single UPDATE command to the database.
    /// </summary>
    public class UnitOfWork : IDisposable
    {
        #region 定义变量
        private WebApiDbEntities _context = null;   //数据上下文
        private GenericRepository<User> _userReposipory;   //仓储User
        private GenericRepository<Product> _productReposipory; //仓储Product
        private GenericRepository<Token> _tokenRepository; //仓储Token
        #endregion

        /// <summary>
        /// 显式的定义无参构造函数
        /// </summary>
        public UnitOfWork()
        {
            //实例化数据上下文对象
            _context = new WebApiDbEntities();
        }

        #region 创建仓储
        /// <summary>
        /// UserRepository只读属性
        /// 不要忘了属性特征：get，set
        /// </summary>
        public GenericRepository<User> UserRepository
        {
            get
            {
                if (this._userReposipory == null)
                {
                    this._userReposipory = new GenericRepository<User>(_context);
                }
                return _userReposipory;

            }
        }

        /// <summary>
        /// ProductRepository只读属性
        /// </summary>
        public GenericRepository<Product> ProductRepository
        {
            get
            {
                if (_productReposipory == null)
                {
                    this._productReposipory = new GenericRepository<Product>(_context);
                }
                return _productReposipory;
            }

        }

        /// <summary>
        /// TokenRepository
        /// </summary>
        public GenericRepository<Token> TokenRepository
        {
            get
            {
                if (this._tokenRepository == null)
                {
                    this._tokenRepository = new GenericRepository<Token>(_context);
                }
                return _tokenRepository;
            }
        } 
        #endregion


        #region 保存
        /// <summary>
        /// 保存
        /// </summary>
        public void Save()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (DbEntityValidationException ex)  //DbEntityValidationException在System.Data.Entity命名空间下
            {
                var outputLines = new List<string>();
                foreach (var itemListError in ex.EntityValidationErrors)
                {
                    outputLines.Add(string.Format("{0}:Entity of type\"{1}\"in state\"{2}\"has the following validation errors:",
                        DateTime.Now, itemListError.Entry.Entity.GetType().Name, itemListError.Entry.State));
                    foreach (var itemError in itemListError.ValidationErrors)
                    {
                        outputLines.Add(string.Format("-Property:\"{0}\",Error:\"{1}\"", itemError.PropertyName, itemError.ErrorMessage));
                    }
                }
                System.IO.File.AppendAllLines(@"C:\errors.txt", outputLines, Encoding.UTF8);

                throw ex;
            }
        } 
        #endregion


        /// <summary>
        /// 定义私有的Disposed变量
        /// </summary>
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {

            if (!this.disposed)
            {
                if (disposing)
                {
                    //Debug在命名空间System.Diagnostics下
                    Debug.WriteLine("UnitOfWork is being Disposing");
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

       
        /// <summary>
        /// 实现接口IDisposable的Dispose方法
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
            //throw new NotImplementedException();
        }
    }
}
