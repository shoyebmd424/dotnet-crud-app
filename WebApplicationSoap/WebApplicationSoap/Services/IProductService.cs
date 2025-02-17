using System.ServiceModel;
using WebApplicationSoap.Models;

namespace WebApplicationSoap.Services
{
    [ServiceContract]
    public interface IProductService
    {
        [OperationContract]
        List<Product> GetAllProducts();

        [OperationContract]
        Product GetProductById(int id);

        [OperationContract]
        void AddProduct(Product product);

        [OperationContract]
        void UpdateProduct(Product product);

        [OperationContract]
        void DeleteProduct(int id);
    }
}
