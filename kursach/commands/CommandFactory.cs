using shop.DB;
namespace shop.commands;

public static class CommandFactory
{
    public static Dictionary<string, ICommand> CreateCommands(Account account, AccountService accountService, OrderService orderService, ProductService productService)
    {
        return account switch
        {
            UserAccount => new Dictionary<string, ICommand>
            {
                { "1", new AddBalanceCommand() },
                { "2", new CheckBalanceCommand() },
                { "3", new ViewProductsCommand(productService) },
                { "4", new AddProductToCartCommand(productService) },
                { "5", new DeleteProductFromCartCommand(accountService) },
                { "6", new ViewCartCommand() },
                { "7", new CreateOrderCommand(orderService, productService) },
                { "8", new ViewOrderHistoryCommand(orderService) },
                { "9", new LogoutCommand() },
                { "10", new ExitCommand() }
            },
            AdminAccount => new Dictionary<string, ICommand>
            {
                { "1", new AddProductCommand(productService) },
                { "2", new DeleteProductCommand(productService) },
                { "3", new UpdateProductCommand(productService) },
                { "4", new ViewProductsCommand(productService) },
                { "5", new ViewAllOrderHistoryCommand(orderService) },
                { "6", new ViewAllAccountsCommand(accountService) },
                { "7", new DeleteUserAccountCommand(accountService) },
                { "8", new LogoutCommand() },
                { "9", new ExitCommand() }
            },
            _ => throw new ArgumentException("Invalid account type.")
        };
    }
}