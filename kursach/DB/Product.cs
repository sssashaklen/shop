namespace shop.DB;

public class ProductRepository : IProductRepository
{
    private readonly DbContext _dbContext;

    public ProductRepository(DbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
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
        return _dbContext.products;
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
    }
    
    public void Delete(Product product)
    {
        _dbContext.products.Remove(product);
    }
    
    public void DecreaseQuantity(Product product, int quantityToDecrease)
    {
        product.quantity -= quantityToDecrease;
    }
}

public class ProductService(IProductRepository productRepository) : IProductService
{
    private readonly IProductRepository _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));

    public void Create(Product product)
    {
        if (product == null)
        {
            throw new ArgumentNullException(nameof(product), "Product cannot be null.");
        }

        _productRepository.Create(product);
    }

    public Product ReadById(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(id), "Product ID must be a positive integer.");
        }

        var product = _productRepository.ReadById(id);
        if (product == null)
        {
            throw new KeyNotFoundException("Product not found.");
        }

        return product;
    }

    public List<Product> ReadAll()
    {
        var products = _productRepository.ReadAll();
        if (products == null || !products.Any())
        {
            throw new InvalidOperationException("No products found.");
        }
        return products;
    }

    public void Update(Product product)
    {
        if (product == null)
        {
            throw new ArgumentNullException(nameof(product), "Product cannot be null.");
        }

        if (product.id <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(product.id), "Product ID must be a positive integer.");
        }

        var existingProduct = _productRepository.ReadById(product.id);
        if (existingProduct == null)
        {
            throw new KeyNotFoundException("Product not found.");
        }

        _productRepository.Update(product);
    }

    public void Delete(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(id), "Product ID must be a positive integer.");
        }

        var product = _productRepository.ReadById(id);
        if (product == null)
        {
            throw new KeyNotFoundException("Product not found.");
        }

        _productRepository.Delete(product);
    }

    public void DecreaseQuantity(int productId, int quantityToDecrease)
    {
        if (productId <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(productId), "Product ID must be a positive integer.");
        }

        if (quantityToDecrease <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(quantityToDecrease), "Quantity to decrease must be a positive integer.");
        }

        var product = _productRepository.ReadById(productId);
        if (product == null)
        {
            throw new KeyNotFoundException("Product not found.");
        }

        if (product.quantity < quantityToDecrease)
        {
            throw new InvalidOperationException("Not enough stock to decrease the quantity.");
        }

        _productRepository.DecreaseQuantity(product, quantityToDecrease);
    }
}