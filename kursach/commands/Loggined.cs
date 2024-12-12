using shop.DB;

namespace shop.commands;


public class AddBalanceCommand : ICommand
{
    public void Execute()
    {
        Account account = Program.currentAccount;
        Console.WriteLine("Enter the amount to add to your balance: ");
        if (int.TryParse(Console.ReadLine(), out int amount) && amount > 0)
        {
            account.Balance += amount;
            Console.WriteLine($"Balance successfully updated. New balance: {account.Balance}");
        }
        else
        {
            Console.WriteLine("Invalid amount entered.");
        }
    }

    public string ShowInfo()
    {
        return "Add Balance";
    }
}

public class CheckBalanceCommand : ICommand
{
    public void Execute()
    {
        Account account = Program.currentAccount;
        Console.WriteLine($"Your current balance is: {account.Balance} ");
    }

    public string ShowInfo()
    {
        return "Check Balance";
    }
}
public class ViewProductsCommand(ProductService productService) : ICommand
{
    public void Execute()
    {
        var products = productService.ReadAll();
        if (products.Count == 0)
        {
            Console.WriteLine("No products available.");
            return;
        }
        Console.WriteLine("Available products:");
        foreach (var product in products)
        {
            Console.WriteLine($"ID: {product.id} | Name: {product.name} | Price: {product.price} | Quantity: {product.quantity} | Description: {product.description}");
        }
    }

    public string ShowInfo()
    {
        return "View Products";
    }
}

public class AddProductToCartCommand(ProductService productService) : ICommand
{
    public void Execute()
    {
        Console.WriteLine("Enter the ID of the product to add to your cart: ");
        if (int.TryParse(Console.ReadLine(), out int productId))
        {
            Account account = Program.currentAccount;
            var product = productService.ReadById(productId);
                if (productId != null)
                {
                    Console.WriteLine("Enter the quantity to add: ");
                    if (int.TryParse(Console.ReadLine(), out int quantity) && quantity > 0 &&
                        quantity <= product.quantity)
                    { 
                        account.Cart.AddToCart(product, quantity);
                        Console.WriteLine($"{product.name} successfully added to your cart.");
                    }
                    else if (quantity > product.quantity)
                    {
                        Console.WriteLine($"Max quantity for this product is {product.quantity}.");
                    }
                    else
                    {
                        Console.WriteLine("Invalid quantity.");
                    }
                }

            else
            {
                Console.WriteLine("Product not found.");
            }
        }
        else
        {
            Console.WriteLine("Invalid product ID entered.");
        }
    }
    public string ShowInfo()
    {
        return "Add Product to Cart";
    }
}

public class DeleteProductFromCartCommand(AccountService accountService)
    : ICommand
{
    public void Execute()
    {
        Console.WriteLine("Enter the ID of the product to remove from your cart: ");
        
        if (int.TryParse(Console.ReadLine(), out int productId))
        {
            Account account = Program.currentAccount;
            var cartItem = accountService.GetCartItem(account, productId);
            
            if (cartItem != null)
            {
                Console.WriteLine($"Current quantity of this product in your cart: {cartItem.Quantity}");
                Console.WriteLine("Enter the quantity to remove: ");
                
                if (int.TryParse(Console.ReadLine(), out int quantity) && quantity > 0)
                {
                    if (quantity < cartItem.Quantity)
                    {
                        accountService.ReduceCartItemQuantity(account, productId, quantity);
                        Console.WriteLine($"Removed {quantity} units of product ID {productId} from your cart.");
                    }
                    else if (quantity == cartItem.Quantity)
                    {
                        accountService.RemoveFromCart(account, productId);
                        Console.WriteLine($"Product ID {productId} has been fully removed from your cart.");
                    }
                    else
                    {
                        Console.WriteLine("You can't remove more than the available quantity in your cart.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid quantity entered.");
                }
            }
            else
            {
                Console.WriteLine("Product not found in your cart.");
            }
        }
        else
        {
            Console.WriteLine("Invalid product ID entered.");
        }
    }

    public string ShowInfo()
    {
        return "Delete or reduce product quantity from cart.";
    }
}
public class ViewCartCommand() : ICommand
{
    public void Execute()
    {
        var account = Program.currentAccount;
        var currentCart = account.Cart;
        
        if (currentCart == null || currentCart.Products == null || currentCart.Products.Count == 0)
        {
            Console.WriteLine("Your cart is empty.");
            return;
        }
        
        Console.WriteLine("Products in your cart:");
        float totalPrice = 0;
        foreach (var cartItem in currentCart.Products)
        {
            var price = cartItem.Product.price * cartItem.Quantity;
            Console.WriteLine($"Product: {cartItem.Product.name} | Price: {price} | Quantity: {cartItem.Quantity}");
            totalPrice += price;
        }
        Console.WriteLine($"Total price: {totalPrice}");
    }

    public string ShowInfo()
    {
        return "View Cart";
    }
}

public class ViewOrderHistoryCommand(OrderService orderService) : ICommand
{
    private readonly Account _account = Program.currentAccount;
    public void Execute()
    {
        Console.Clear();
        var orders = orderService.GetOrdersByAccountId(_account.Id);
        if (orders.Count == 0)
        {
            Console.WriteLine("You have no order history.");
            return;
        }

        float totalPrice = 0;
        Console.WriteLine("Order history:");
        foreach (var order in orders)
        {
            Console.WriteLine($"Order ID: {order.OrderId} | Date: {order.OrderDate} | Time : {order.OrderTime}");
            foreach (var item in order.Products)
            {
                var price = item.Product.price * item.Quantity;
                Console.WriteLine($"  - Product: {item.Product.name} | Price: {item.Product.price}");
                
                totalPrice += price;
            }
            Console.WriteLine($"  - Total price: {totalPrice}");
        }
    }

    public string ShowInfo()
    {
        return "View Order History";
    }
}

public class CreateOrderCommand(OrderService orderService, ProductService productService) : ICommand
{
    public void Execute()
    {
        var account = Program.currentAccount;
        var cart = account.Cart;
        Order order = new Order(account.Id, cart);
        var balance = account.Balance;
        if (order.OrderPrice <= balance)
        {
            orderService.Create(order);
            Console.WriteLine("Order created successfully.");
            Console.WriteLine($"Price: {order.OrderPrice}");
            foreach( var productItem in cart.Products)
            {
                var productId = productItem.Product.id;
                var productItemQuantity = productItem.Quantity;
                productService.DecreaseQuantity(productId, productItemQuantity);
            }
            int totalPrice = order.CalculateOrderPrice();
            account.Balance -= totalPrice;
        }
        else
        {
            Console.WriteLine("Order could not be created. Please add money to balance.");
        }
    }
    public string ShowInfo()
    {
        return "Create Order";
    }
}