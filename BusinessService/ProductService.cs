using BusinessEntities;
using DataModel;
using DataModel.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
namespace BusinessService
{
    public class ProductService : IProductServices
    {

        private readonly UnitOfWork _unitOfWork;

        /// <summary>
        /// 显示的声明构造函数
        /// </summary>
        public ProductService()
        {
            //实例化工作单元对象
            _unitOfWork = new UnitOfWork();
        }

        /// <summary>
        /// 根据产品Id，获取产品
        /// To get product by id ( GetproductById ) : 
        /// We call repository to get the product by id. 
        /// Id comes as a parameter from the calling method to that service method. 
        /// It returns the product entity from the database.
        /// Note that it will not return the exact db entity, 
        /// instead we’ll map it with our business entity using AutoMapper and return it to calling method.
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public BusinessEntities.ProductEntity GetProductById(int productId)
        {
            var product = _unitOfWork.ProductRepository.GetByID(productId);
            if (product != null)
            {
                AutoMapper.Mapper.CreateMap<Product, ProductEntity>();
                var productModel = AutoMapper.Mapper.Map<Product, ProductEntity>(product);
                return productModel;

            }
            else
            {
                return null;
            }
            //throw new NotImplementedException();
        }

        /// <summary>
        /// 获取所有的产品
        /// Get all products from database (GetAllProducts) : 
        /// This method returns all the products residing in database,
        /// again we make use of AutoMapper to map the list and return back.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BusinessEntities.ProductEntity> GetAllProducts()
        {
            var products = _unitOfWork.ProductRepository.GetAll().ToList();
            if (products != null)
            {
                AutoMapper.Mapper.CreateMap<Product, ProductEntity>();
                var productsModel = AutoMapper.Mapper.Map<List<Product>, List<ProductEntity>>(products);
                return productsModel;
            }
            else
            {
                return null;
            }
            //throw new NotImplementedException();
        }

        /// <summary>
        /// 创建产品
        /// Create a new product (CreateProduct) :
        /// This method takes product BusinessEntity as an argument
        /// and creates a new object of actual database entity and insert it using unit of work.
        /// </summary>
        /// <param name="productEntity"></param>
        /// <returns></returns>
        public int CreateProduct(BusinessEntities.ProductEntity productEntity)
        {
            using (var scope = new TransactionScope())  //TransactionScope在System.TransactionScope命名空间下
            {
                var product = new Product()
                {
                    ProductName = productEntity.ProductName
                };
                _unitOfWork.ProductRepository.Insert(product);
                _unitOfWork.Save();
                scope.Complete();  //事务完成
                return product.ProductId;
            }
            //throw new NotImplementedException();
        }

        /// <summary>
        /// 更新产品
        /// 先查找到，在修改
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="productEntity"></param>
        /// <returns></returns>
        public bool UpdateProduct(int productId, BusinessEntities.ProductEntity productEntity)
        {
            var success = false;
            if (productEntity != null)
            {
                using (var scope = new TransactionScope())
                {
                    var product = _unitOfWork.ProductRepository.GetByID(productId);
                    if (product != null)
                    {
                        product.ProductName = productEntity.ProductName;
                        _unitOfWork.ProductRepository.Update(product);
                        _unitOfWork.Save();
                        scope.Complete();
                        success = true;
                    }

                }
            }
            return success;
            // throw new NotImplementedException();
        }

        /// <summary>
        /// 删除产品
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public bool DeleteProduct(int productId)
        //思考：一个方法怎么写，首先看他的返回值类型，
        //比如返回值是bool，那么方法体中首先定义个变量，来初始化，然后返回这个变量

            //删除和修改，默认都是先根据ID找到这个实体，在删除
        {
            bool success = false;
            if (productId > 0)   //int类型的变量如果没有赋值，初始值默认是0，这里判断大于0就是相当于传值过来了。
            {
                using (var scope = new TransactionScope())
                {
                    Product product = _unitOfWork.ProductRepository.GetByID(productId);
                    if (product != null)
                    {
                        _unitOfWork.ProductRepository.Delete(productId);
                        _unitOfWork.Save();
                        scope.Complete();
                        success = true;
                    }
                }
            }
            return success;
            //throw new NotImplementedException();
        }
    }
}
