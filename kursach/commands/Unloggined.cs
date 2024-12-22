using System.Text.RegularExpressions;
using kursach;
using shop.DB;

namespace shop.commands;

public class CommandRegister(IAccountService accountService) : ICommand
{
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

            var emailErrors = GetEmailErrors(email);
            if (emailErrors.Any())
            {
                Console.WriteLine("Email does not meet the following requirements:");
                foreach (var error in emailErrors)
                {
                    Console.WriteLine($" - {error}");
                }
            }
        } while (!IsValidEmail(email)); 
        
        do
        {
            Console.WriteLine("Please enter your password: ");
            password = PasswordManager.ReadAndHashPassword();
        } while (string.IsNullOrEmpty(password)); 
        
        int accountType = email.EndsWith("@admin.com") ? 2 : 1;
        
        int balance = 0;
        
        Account newAccount = AccountFactory.CreateAccount(accountType, username, balance, email, password);
        accountService.Create(newAccount);
        UserManager.Login(newAccount);
        Console.WriteLine($"Account for {username} created.");
    }
    

    private bool IsValidEmail(string email)
    {
        var emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        return Regex.IsMatch(email, emailPattern);
    }
    
    private List<string> GetEmailErrors(string email)
    {
        var errors = new List<string>();
        
        if (string.IsNullOrEmpty(email))
        {
            errors.Add("Email cannot be empty.");
        }

        if (!email.Contains("@"))
        {
            errors.Add("Email must contain '@' symbol.");
        }

        var emailParts = email.Split('@');
        if (emailParts.Length != 2)
        {
            errors.Add("Email must contain exactly one '@' symbol.");
        }
        else
        {
            string localPart = emailParts[0];
            string domainPart = emailParts[1];
            
            if (string.IsNullOrEmpty(localPart))
            {
                errors.Add("Email must have a local part before '@'.");
            }

            if (string.IsNullOrEmpty(domainPart) || !domainPart.Contains("."))
            {
                errors.Add("Email must have a domain part with a '.' symbol (e.g., 'example.com').");
            }
            
            if (!Regex.IsMatch(domainPart, @"^[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
            {
                errors.Add("Email domain must be valid (e.g., 'example.com').");
            }
        }
        
        if (!Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
        {
            errors.Add("Email does not match the correct format.");
        }

        return errors;
    }

    public string ShowInfo()
    {
        return "Register";
    }
}



public class LoginCommand(IAccountService accountService) : ICommand
{
    public void Execute()
    {
        Console.WriteLine("Please enter your account name: ");
        string username = Console.ReadLine();

        Account account = accountService.ReadByUserName(username);
        if (account == null)
        {
            Console.WriteLine("Account not found!");
            return;
        }

        Console.WriteLine("Please enter your password: ");
        string password = PasswordManager.ReadPassword();
        
        if (PasswordManager.VerifyPassword(password, account.Password))
        {
            Console.WriteLine($"Account for {username} logged in.");
            UserManager.Login(account);
        }
        else
        {
            Console.WriteLine("Invalid password!");
        }
    }



    public string ShowInfo()
    {
        return "Login";
    }
}
