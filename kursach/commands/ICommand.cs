﻿
using kursach;

namespace shop.commands;

public interface ICommand
{
    void Execute();
    string ShowInfo();
}
public class LogoutCommand: ICommand
{
    public void Execute()
    {
        Account currentAccount = null;
        UserManager.Login(currentAccount);
    }

    public string ShowInfo()
    {
        return "Log out.";
    }
}

public class ExitCommand : ICommand
{
    public void Execute()
    {
        Console.WriteLine("Exiting the application. Goodbye!");
        Environment.Exit(0); 
    }

    public string ShowInfo()
    {
        return "Exit the application.";
    }
}

