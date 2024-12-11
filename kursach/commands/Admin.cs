using shop.DB;

namespace shop.commands;

public class AddProductCommand : ICommand
{
    private ProductService _productService;

    public AddProductCommand(ProductService productService)
    {
        _productService = productService;
    }

    public void Execute()
    {
        Console.WriteLine("Enter product name: ");
        string name = Console.ReadLine();

        Console.WriteLine("Enter product description: ");
        string description = Console.ReadLine();

        Console.WriteLine("Enter product price: ");
        if (!int.TryParse(Console.ReadLine(), out int price) || price <= 0)
        {
            Console.WriteLine("Invalid price.");
            return;
        }

        Console.WriteLine("Enter product quantity: ");
        if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity <= 0)
        {
            Console.WriteLine("Invalid quantity.");
            return;
        }
        
        Product newProduct = new Product(name, description, price, quantity);
        _productService.Create(newProduct);
        Console.WriteLine("Product added successfully.");
    }

    public string ShowInfo()
    {
        return "Add Product";
    }
}
public class DeleteProductCommand : ICommand
{
    private ProductService _productService;

    public DeleteProductCommand(ProductService productService)
    {
        _productService = productService;
    }

    public void Execute()
    {
        Console.WriteLine("Enter product ID to delete: ");
        if (int.TryParse(Console.ReadLine(), out int productId))
        {
            var product = _productService.ReadById(productId);
            if (product != null)
            {
                _productService.Delete(productId);
                Console.WriteLine($"Product {product.name} deleted successfully.");
            }
            else
            {
                Console.WriteLine("Product not found.");
            }
        }
        else
        {
            Console.WriteLine("Invalid product ID.");
        }
    }

    public string ShowInfo()
    {
        return "Delete Product";
    }
}
public class UpdateProductCommand : ICommand
{
    private ProductService _productService;

    public UpdateProductCommand(ProductService productService)
    {
        _productService = productService;
    }

    public void Execute()
    {
        Console.WriteLine("Enter product ID to update: ");
        if (int.TryParse(Console.ReadLine(), out int productId))
        {
            var product = _productService.ReadById(productId);
            if (product != null)
            {
                Console.WriteLine($"Updating product {product.name}");

                Console.WriteLine("Enter new product name (leave blank to keep current): ");
                string newName = Console.ReadLine();
                if (!string.IsNullOrEmpty(newName))
                {
                    product.name = newName;
                }

                Console.WriteLine("Enter new description (leave blank to keep current): ");
                string newDescription = Console.ReadLine();
                if (!string.IsNullOrEmpty(newDescription))
                {
                    product.description = newDescription;
                }

                Console.WriteLine("Enter new price (leave blank to keep current): ");
                string priceInput = Console.ReadLine();
                if (int.TryParse(priceInput, out int newPrice) && newPrice > 0)
                {
                    product.price = newPrice;
                }

                Console.WriteLine("Enter new quantity (leave blank to keep current): ");
                string quantityInput = Console.ReadLine();
                if (int.TryParse(quantityInput, out int newQuantity) && newQuantity >= 0)
                {
                    product.quantity = newQuantity;
                }

                _productService.Update(product);
                Console.WriteLine("Product updated successfully.");
            }
            else
            {
                Console.WriteLine("Product not found.");
            }
        }
        else
        {
            Console.WriteLine("Invalid product ID.");
        }
    }
    public string ShowInfo()
    {
        return "Update Product";
    }
}
public class ViewAllOrderHistoryCommand : ICommand
{
    private readonly OrderService _orderService;

    public ViewAllOrderHistoryCommand(OrderService orderService)
    {
        _orderService = orderService;
    }

    public void Execute()
    {
        var orders = _orderService.ReadAll();
        if (orders.Count == 0)
        {
            Console.WriteLine("No orders found.");
            return;
        }

        Console.WriteLine("All orders:");
        foreach (var order in orders)
        {
            Console.WriteLine($"Order ID: {order.OrderId} | Account ID: {order.CustomerId} | Date: {order.OrderDate} | Status: {order.OrderStatus}");
            foreach (var item in order.Products)
            {
                Console.WriteLine($"  - Product: {item.Product.name} | Price: {item.Product.price}");
            }
        }
    }

    public string ShowInfo()
    {
        return "View All Orders";
    }
}
public class DeleteUserAccountCommand(AccountService accountService) : ICommand
{
    public void Execute()
    {
        Console.WriteLine("Enter the username of the account to delete: ");
        string username = Console.ReadLine();

        var account = accountService.ReadByUserName(username);
        if (account != null && !(account is AdminAccount))
        {
            accountService.Delete(account.Id);
            Console.WriteLine($"Account for {username} deleted successfully.");
        }
        else if (account is AdminAccount)
        {
            Console.WriteLine("Cannot delete admin account.");
        }
        else
        {
            Console.WriteLine("User not found.");
        }
    }

    public string ShowInfo()
    {
        return "Delete User Account";
    }
}

public class ViewAllAccountsCommand(AccountService accountService) : ICommand
{
    // Ініціалізуємо команду з AccountService

    // Метод для виконання команди
    public void Execute()
    {
        var accounts = accountService.ReadAll(); // Отримуємо всі акаунти

        if (accounts.Count == 0)
        {
            Console.WriteLine("No accounts found.");
        }
        else
        {
            Console.WriteLine("List of all accounts:");
            foreach (var account in accounts)
            {
                Console.WriteLine($"ID: {account.Id}, Name: {account.Name}, Email: {account.Email}, Balance: {account.Balance}");
            }
        }
    }

    public string ShowInfo()
    {
        return "View all accounts.";
    }
}
