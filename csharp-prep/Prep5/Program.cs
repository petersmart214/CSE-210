using System;

class Program
{

    public static void DisplayWelcome() {
        Console.WriteLine("Welcome to the Program");
    }

    public static string PromptUserName() {
        Console.WriteLine("Please input your name...");
        return Console.ReadLine();
    }

    public static int PromptUserNumber() {
        Console.WriteLine("Please input your number...");
        return int.Parse(Console.ReadLine());
    }

    public static int SquareNumber(int numToSquare) {
        return (int) Math.Pow(numToSquare, 2);
    }

    public static void DisplayResult(string userName, int userNum) {
        Console.WriteLine($"For {userName} your number is {userNum}.");
    }
    
    static void Main(string[] args)
    {
        DisplayWelcome();
        DisplayResult(PromptUserName(), SquareNumber(PromptUserNumber()));
    }
}