using System;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Please input grade...");
        int grade = int.Parse(Console.ReadLine());
        string letter_grade = "";
        Boolean passing = true;
        if (grade >= 90) {
            letter_grade = "A";
        }
        else if (grade >= 80) {
            letter_grade = "B";
        }
        else if (grade >= 70) {
            letter_grade = "C";
        }
        else if (grade >= 60) {
            letter_grade = "D";
        }
        else if (grade < 60) {
            letter_grade = "F";
        }

        if (grade < 70) passing = false;

        Console.WriteLine($"You have received a(n) {letter_grade}, which is {(passing ? "passing" : "failing")}.");
    }
}