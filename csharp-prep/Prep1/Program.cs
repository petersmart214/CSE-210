using System;

class Program
{
    static void Main(string[] args)
    {
        Loop();   
    }
    static void Loop() {
        Console.WriteLine("ID, please...");
        string[] name = Console.ReadLine().Split(' ');
        if (name.Length != 2) Loop(); else {
            Console.WriteLine($"Writing for you...\nHello, {name[0]}, {name[0]} {name[1]}");
        }
        
    }

}