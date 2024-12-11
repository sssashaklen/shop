namespace shop
{
    public class Cart
    {
        public int CartId { get; set; }
        public List<CartItem> Products { get; set; } = new();
        
        public Cart(List<CartItem> items)
        {
            Products = items;
        }

        public void AddToCart(Product product, int quantity)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product), "Product cannot be null");

            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));

            var cartItem = new CartItem(product, quantity);
            
            var existingCartItem = Products.FirstOrDefault(item => item.Product.id == product.id);

            if (existingCartItem != null)
            {
                existingCartItem.IncreaseQuantity(cartItem.Quantity);
            }
            else
            {
                Products.Add(cartItem);
            }
        }
    }

    public class CartItem(Product product, int quantity)
    {
        public Product Product { get; set; } = product;
        public int Quantity { get; set; } = quantity;
        
        public void IncreaseQuantity(int quantity)
        {
            Quantity += quantity;
        }
        public void DecreaseQuantity(int quantity)
        {
            Quantity -= quantity;
        }
    }
}