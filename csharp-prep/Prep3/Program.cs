using System;
using System.Linq.Expressions;

class Program
{

    static void Main(string[] args)
    {
        Fraction frac1 = new Fraction();
        Fraction frac2 = new Fraction(5);
        Fraction frac3 = new Fraction(3, 4);
        Fraction frac4 = new Fraction(1, 3);

        Console.WriteLine(frac1.getFractionString());
        Console.WriteLine(frac1.getDecimalValue());
        Console.WriteLine(frac2.getFractionString());
        Console.WriteLine(frac2.getDecimalValue());
        Console.WriteLine(frac3.getFractionString());
        Console.WriteLine(frac3.getDecimalValue());
        Console.WriteLine(frac4.getFractionString());
        Console.WriteLine(frac4.getDecimalValue());
    }
}