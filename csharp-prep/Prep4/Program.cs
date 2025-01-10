using System;

class Program
{

    public static int getLargest(List<int> to_sort) {
        int currentLargest = 0;
        foreach(int i in to_sort) {
            if (i > currentLargest) currentLargest = i;
        }
        return currentLargest; 
    }

    public static float getAvgFloat(List<int> to_avg) {
        float average = 0;
        foreach (int i in to_avg) {
            average = (float) i + average;
        }
        return average / to_avg.Count;
    }
    static void Main(string[] args)
    {
        List<int> userList = new List<int>();
        Console.WriteLine("Input Numbers to add to a list. Use 0 to finish.");
        int picked = -1;
        while (picked != 0) {
            picked = int.Parse(Console.ReadLine());
            if (picked > 0) userList.Add(picked);
        }
        Console.WriteLine($"Average: {getAvgFloat(userList)}");
        Console.WriteLine($"Largest: {getLargest(userList)}");
    }
}