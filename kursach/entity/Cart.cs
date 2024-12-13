namespace shop;

    public class Cart(List<CartItem>? products = null)
    {
        public List<CartItem> Products { get; } = products ?? new List<CartItem>();

        public void AddToCart(Product product, int quantity)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product), "Product cannot be null");

            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));
            
            CartItem item = new CartItem(product, quantity);
            
            var existingCartItem = Products.FirstOrDefault(item => item.Product.id == product.id);

            if (existingCartItem != null)
            {
                existingCartItem.IncreaseQuantity(item.Quantity);
            }
            else
            {
                Products.Add(item);
            }
        }
    }

    public class CartItem
    {
        public Product Product { get; private set; }
        public int Quantity { get; private set; }

        public CartItem(Product product, int quantity)
        {
            if (product == null) 
                throw new ArgumentNullException(nameof(product), "Product cannot be null.");
            if (quantity <= 0) 
                throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));
        
            Product = product; 
            Quantity = quantity; 
        }

        public void IncreaseQuantity(int amount)
        {
            Quantity += amount;
        }

        public void DecreaseQuantity(int amount)
        {
            Quantity -= amount;
        }
    }

