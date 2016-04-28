using BusinessEntities;
using System.Collections.Generic;

namespace BusinessService
{
    /// <summary>
    /// 接口默认是private修饰符，里面的方法，不能有方法体，方法默认是public abstract的
    /// </summary>
   public interface IProductServices
    {
       ProductEntity GetProductById(int productId);

       IEnumerable<ProductEntity> GetAllProducts();

       int CreateProduct(ProductEntity productEntity);

       bool UpdateProduct(int productId, ProductEntity productEntity);

       bool DeleteProduct(int productId);
      
    }
}
