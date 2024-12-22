using shop.DB;
namespace shop.commands;

public static class CommandFactory
{
    public static Dictionary<string, ICommand> CreateCommands(Account account, IAccountService accountService, IOrderService orderService, IProductService productService)
    {
        if (account == null)
        {
            return new Dictionary<string, ICommand>
            {
                { "1", new CommandRegister(accountService) },
                { "2", new LoginCommand(accountService) },
                { "3", new ExitCommand() }
            };
        }

        return account switch
        {
            UserAccount => new Dictionary<string, ICommand>
            {
                { "1", new CheckBalanceCommand() },
                { "2", new ViewProductsCommand(productService)},
                { "3", new ViewCartCommand(productService, accountService, orderService) },
                { "4", new ViewOrderHistoryCommand(orderService) },
                { "5", new LogoutCommand() },
                { "6", new ExitCommand() }
            },
            AdminAccount => new Dictionary<string, ICommand>
            {
                {"1", new ProductCommand(productService) },
                { "2", new ViewAllOrderHistoryCommand(orderService) },
                { "3", new ViewAllAccountsCommand(accountService) },
                { "4", new LogoutCommand() },
                { "5", new ExitCommand() }
            },
            _ => throw new ArgumentException("Invalid account type.")
        };
    }
}