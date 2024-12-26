using System.Text;
using kursach;
using shop.DB;

namespace shop.commands;


public class AddBalanceCommand : ICommand
{
    public void Execute()
    {
        var account = UserManager.GetCurrentAccount();
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
        var account = UserManager.GetCurrentAccount();
        Console.WriteLine($"Your current balance is: {account.Balance}");

        // Додаткові дії після перевірки балансу
        var commands = new Dictionary<string, ICommand>
        {
            { "1", new AddBalanceCommand() }
        };

        Console.WriteLine("Would you like to add balance?");
        foreach (var entry in commands)
        {
            Console.WriteLine($"{entry.Key}. {entry.Value.ShowInfo()}");
        }

        string input = Console.ReadLine();
        if (commands.TryGetValue(input, out ICommand command))
        {
            try
            {
                command.Execute();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("Returning to main menu.");
        }
    }

    public string ShowInfo()
    {
        return "Check Balance";
    }
}

public class ViewProductsCommand(IProductService productService) : ICommand
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
        
        var commands = new Dictionary<string, ICommand>
        {
            { "1", new AddProductToCartCommand(productService) },
        };


        Console.WriteLine("Would you like to add products?:");
        foreach (var entry in commands)
        {
            Console.WriteLine($"{entry.Key}. {entry.Value.ShowInfo()}");
        }
        
        string input = Console.ReadLine();
        if (commands.TryGetValue(input, out ICommand command))
        {
            try
            {
                command.Execute();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("Invalid option, please try again.");
        }
    }

    public string ShowInfo()
    {
        return "View Products";
    }
}

public class AddProductToCartCommand(IProductService productService) : ICommand
{
    public void Execute()
    {
        Console.WriteLine("Enter the ID of the product to add to your cart: ");
        if (int.TryParse(Console.ReadLine(), out int productId))
        {
            var account = UserManager.GetCurrentAccount();
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

public class DeleteProductFromCartCommand(IAccountService accountService)
    : ICommand
{
    public void Execute()
    {
        Console.WriteLine("Enter the ID of the product to remove from your cart: ");
    
        if (int.TryParse(Console.ReadLine(), out int productId))
        {
            var account = UserManager.GetCurrentAccount();
            var cart = account.Cart;
            
            Console.WriteLine($"Current quantity of this product in your cart: {accountService.GetCartItemQuantity(account, productId)}");
            Console.WriteLine("Enter the quantity to remove: ");
        
            if (int.TryParse(Console.ReadLine(), out int quantity) && quantity > 0)
            {
                try
                {
                    accountService.ReduceCartItemQuantity(account, productId, quantity);
                    Console.WriteLine($"Successfully removed {quantity} units of product ID {productId} from your cart.");
                    
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                Console.WriteLine("Invalid quantity entered.");
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


public class ViewCartCommand(IProductService productService, IAccountService accountService, IOrderService orderService): ICommand
{
    public void Execute()
    {
        var account = UserManager.GetCurrentAccount();
        var currentCart = account.Cart;

        if (currentCart == null || currentCart.Products == null || currentCart.Products.Count == 0)
        {
            Console.WriteLine("Your cart is empty.");
            return;
        }

        Console.WriteLine("Products in your cart:");
        foreach (var cartItem in currentCart.Products)
        {
            Console.WriteLine($"Product: {cartItem.Product.name} | Price: {cartItem.Product.price} | Quantity: {cartItem.Quantity}");
        }
        Console.WriteLine($"Total price: {currentCart.totalPrice}");

        // Додаткові дії після перегляду кошика
        var commands = new Dictionary<string, ICommand>
        {
            { "1", new DeleteProductFromCartCommand(accountService) },
            { "2", new CreateOrderCommand(orderService, productService) }
        };

        Console.WriteLine("Choose an action:");
        foreach (var entry in commands)
        {
            Console.WriteLine($"{entry.Key}. {entry.Value.ShowInfo()}");
        }

        string input = Console.ReadLine();
        if (commands.TryGetValue(input, out ICommand command))
        {
            try
            {
                command.Execute();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("Invalid option, returning to main menu.");
        }
    }

    public string ShowInfo()
    {
        return "View Cart";
    }
}


public class ViewOrderHistoryCommand(IOrderService orderService) : ICommand
{
    public void Execute()
    {
        var _account = UserManager.GetCurrentAccount();
        Console.Clear();
        var orders = orderService.GetOrdersByAccountId(_account.Id);
        if (orders.Count == 0)
        {
            Console.WriteLine("You have no order history.");
            return;
        }
        
        Console.WriteLine("Order history:");
        foreach (var order in orders)
        {
            Console.WriteLine($"Order ID: {order.OrderId} | Date: {order.OrderDate} | Time : {order.OrderTime}");
            foreach (var item in order.Products)
            {
                Console.WriteLine($"  - Product: {item.Product.name} | Price: {item.Product.price}");
                
            }
            Console.WriteLine($"  - Total price: {order.OrderPrice}");
        }
    }

    public string ShowInfo()
    {
        return "View Order History";
    }
}

public class CreateOrderCommand(IOrderService orderService, IProductService productService) : ICommand
{
    public void Execute()
    {
        var account = UserManager.GetCurrentAccount();
        var cart = account.Cart;
        var totalPrice = cart.totalPrice;
        var balance = account.Balance;
        if (totalPrice <= balance)
        {
            var order = orderService.Create(account);
            Console.WriteLine("Order created successfully.");
            Console.WriteLine($"Price: {order.OrderPrice}");
            foreach( var productItem in cart.Products)
            {
                var productId = productItem.Product.id;
                var productItemQuantity = productItem.Quantity;
                productService.DecreaseQuantity(productId, productItemQuantity);
            }
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