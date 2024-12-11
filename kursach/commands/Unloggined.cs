using System.Text.RegularExpressions;
using shop.DB;

namespace shop.commands;

public class CommandRegister : ICommand
{
    private AccountService _accountService;
    
    public CommandRegister(AccountService accountService)
    {
        _accountService = accountService;
    }

    public void Execute()
    {
        string username, email, password;
        
        do
        {
            Console.WriteLine("Please enter your account name: ");
            username = Console.ReadLine();
        } while (string.IsNullOrEmpty(username));
        
        
        do
        {
            Console.WriteLine("Please enter your email: ");
            email = Console.ReadLine();
        } while (!IsValidEmail(email)); 
        
        
        do
        {
            Console.WriteLine("Please enter your password: ");
            password = ReadPassword();
        } while (string.IsNullOrEmpty(password));
        
        int accountType = email.EndsWith("@admin.com") ? 2 : 1;
        
        int balance = 0;
        
        Account newAccount = AccountFactory.CreateAccount(accountType, username, balance, email, password);
        _accountService.Create(newAccount);
        Program.currentAccount = newAccount;
        Console.WriteLine($"Account for {username} created.");
    }
    
    private bool IsValidEmail(string email)
    {
        var emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        return Regex.IsMatch(email, emailPattern);
    }


    private string ReadPassword()
    {
        string password = string.Empty;
        ConsoleKeyInfo key;
        
        while ((key = Console.ReadKey(true)).Key != ConsoleKey.Enter)
        {
            if (key.Key == ConsoleKey.Backspace && password.Length > 0)
            {
                password = password.Substring(0, password.Length - 1); 
                Console.Write("\b \b"); 
            }
            else if (!char.IsControl(key.KeyChar)) 
            {
                password += key.KeyChar;
                Console.Write("*");
            }
        }

        Console.WriteLine();
        return password;
    }

    public string ShowInfo()
    {
        return "Register";
    }
}

public class LoginCommand : ICommand
{
    private AccountService _accountService;

    public LoginCommand(AccountService accountService)
    {
        _accountService = accountService;
    }

    public void Execute()
    {
        Console.WriteLine("Please enter your account name: ");
        string username = Console.ReadLine();

        Account account = _accountService.ReadByUserName(username);
        if (account == null)
        {
            Console.WriteLine("Account not found!");
            return;
        }

        Console.WriteLine("Please enter your password: ");
        string password = ReadPassword(); // Використовуємо метод ReadPassword без окремого класу

        if (account.Password == password)
        {
            Console.WriteLine($"Account for {username} logged in.");
            Program.currentAccount = account;
        }
        else
        {
            Console.WriteLine("Invalid password!");
            Program.currentAccount = null;
        }
    }

    private string ReadPassword()
    {
        string password = string.Empty;
        ConsoleKeyInfo key;

        while ((key = Console.ReadKey(true)).Key != ConsoleKey.Enter)
        {
            if (key.Key == ConsoleKey.Backspace && password.Length > 0)
            {
                password = password.Substring(0, password.Length - 1);
                Console.Write("\b \b");
            }
            else if (!char.IsControl(key.KeyChar))
            {
                password += key.KeyChar;
                Console.Write("*");
            }
        }

        Console.WriteLine();
        return password;
    }

    public string ShowInfo()
    {
        return "Login";
    }
}
