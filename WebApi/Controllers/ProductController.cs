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

            var products = _productServices.GetAllProducts();
            var productEntities = products as List<ProductEntity> ?? products.ToList();
        }





    }
}
