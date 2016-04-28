using BusinessEntities;
using BusinessService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApi.Controllers
{
    public class ProductController : ApiController
    {
        /// <summary>
        /// 定义IproductService变量
        /// </summary>
        private readonly IProductServices _productServices;

        /// <summary>
        /// 显式定义构造函数
        /// 使用接口变量实例化实现了接口的类的对象
        /// </summary>
        public ProductController()
        {
            _productServices = new ProductService();
        }


        public HttpResponseMessage Get()
        {
            //获取所有的产品
            var products = _productServices.GetAllProducts();
            ///??如果此运算符的左操作数不为 null，则此运算符将返回左操作数；否则返回右操作数。
            var productEntities = products as List<ProductEntity> ?? products.ToList();

            if (productEntities.Any())
            {
                return Request.CreateResponse(HttpStatusCode.OK, productEntities);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Products Not Found");
            }

        }

        public HttpResponseMessage Get(int id)
        {
            var product = _productServices.GetProductById(id);
            if (product != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, product);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "NO Product Found for this id");
            }
        }

        public int Post([FromBody] ProductEntity productEntity)
        {
            //创建产品
            return _productServices.CreateProduct(productEntity);
        }

        public bool Put(int id, [FromBody] ProductEntity productEntity)
        {
            if (id > 0)
            {
                //更新产品
                return _productServices.UpdateProduct(id, productEntity);
            }
            else
            {
                return false;
            }
        }

        public bool Delete(int id)
        {
            if (id > 0)
            {
                return _productServices.DeleteProduct(id);
            }
            else
            {
                return false;
            }
        }







    }
}
