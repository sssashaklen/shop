namespace shop.DB;

public class ProductRepository : IProductRepository
{
    private readonly DbContext _dbContext;

    public ProductRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Create(Product product)
    {
        _dbContext.products.Add(product);
    }

    public Product ReadById(int id)
    {
        return _dbContext.products.FirstOrDefault(g => g.id == id);
    }

    public List<Product> ReadAll()
    {
        return _dbContext.products.Count == 0 ? null : _dbContext.products.ToList();
    }

    public void Update(Product product)
    {
        var existingProduct = ReadById(product.id);
        if (existingProduct != null)
        {
            existingProduct.name = product.name;
            existingProduct.price = product.price;
            existingProduct.quantity = product.quantity;
        }
        else
        {
            throw new ArgumentException("Product not found.");
        }
    }

    public void Delete(int id)
    {
        var product = ReadById(id);
        if (product != null)
        {
            _dbContext.products.Remove(product);
        }
        else
        {
            throw new ArgumentException("Product not found.");
        }
    }
    
    public void DecreaseQuantity(int productId, int quantityToDecrease)
    {
        var product = ReadById(productId);

        if (product == null)
        {
            throw new ArgumentException("Product not found.");
        }

        if (product.quantity < quantityToDecrease)
        {
            throw new ArgumentException("Not enough stock to decrease.");
        }

        product.quantity -= quantityToDecrease;
        Update(product);  
    }
}


public class ProductService(ProductRepository productRepository) : IProductService
{
    private ProductRepository productRepository = productRepository;

    public void Create(Product product)
    {
       productRepository.Create(product);
    }

    public Product ReadById(int id)
    {
        return productRepository.ReadById(id);
    }

    public List<Product> ReadAll()
    {
        return productRepository.ReadAll();
    }

    public void Update(Product product)
    {
        productRepository.Update(product);
    }

    public void Delete(int id)
    {
        productRepository.Delete(id);
    }

    public void DecreaseQuantity(int productId, int quantityToDecrease)
    {
        productRepository.DecreaseQuantity(productId, quantityToDecrease);
    }
}