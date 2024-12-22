using shop.DB;

namespace shop.commands;
public class ProductCommand(IProductService productService)
    : ICommand
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
            { "1", new AddProductCommand(productService)},
            {"2", new DeleteProductCommand(productService)},
            {"3", new UpdateProductCommand(productService)}
        };


        Console.WriteLine("Choose an option:");
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

public class AddProductCommand(IProductService productService) : ICommand
{
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
        productService.Create(newProduct);
        Console.WriteLine("Product added successfully.");
    }

    public string ShowInfo()
    {
        return "Add Product";
    }
}
public class DeleteProductCommand(IProductService productService) : ICommand
{
    public void Execute()
    {
        Console.WriteLine("Enter product ID to delete: ");
        if (int.TryParse(Console.ReadLine(), out int productId))
        {
            var product = productService.ReadById(productId);
            if (product != null)
            {
                productService.Delete(productId);
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
public class UpdateProductCommand(IProductService productService) : ICommand
{
    public void Execute()
    {
        Console.WriteLine("Enter product ID to update: ");
        if (int.TryParse(Console.ReadLine(), out int productId))
        {
            var product = productService.ReadById(productId);
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

                productService.Update(product);
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
public class ViewAllOrderHistoryCommand(IOrderService orderService) : ICommand
{
    public void Execute()
    {
        var orders = orderService.ReadAll();
        if (orders.Count == 0)
        {
            Console.WriteLine("No orders found.");
            return;
        }

        Console.WriteLine("All orders:");
        foreach (var order in orders)
        {
            Console.WriteLine($"Order ID: {order.OrderId} | Account ID: {order.CustomerId} | Date: {order.OrderDate}");
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
public class DeleteUserAccountCommand(IAccountService accountService) : ICommand
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

public class ViewAllAccountsCommand(IAccountService accountService) : ICommand
{
    private readonly IAccountService _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));

    public void Execute()
    {
        var accounts = _accountService.ReadAll();

        if (accounts.Count == 0)
        {
            Console.WriteLine("No accounts found.");
            return;
        }

        Console.WriteLine("List of all accounts:");
        foreach (var account in accounts)
        {
            Console.WriteLine($"ID: {account.Id}, Name: {account.Name}, Email: {account.Email}, Balance: {account.Balance}");
        }

        Console.WriteLine("Do you want to delete a user account? (y/n)");
        string input = Console.ReadLine();
        if (input?.ToLower() == "y")
        {
            var deleteCommand = new DeleteUserAccountCommand(_accountService);
            deleteCommand.Execute();
        }
        else
        {
            Console.WriteLine("Returning to the previous menu.");
        }
    }

    public string ShowInfo()
    {
        return "View all accounts.";
    }
}

