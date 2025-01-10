using System;
using System.Data;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine(new JournalEntry("null", "null").getDateAndTime()[0]);
        
    }
}

class JournalEntry {
    private string entryName;
    private string entryData;
    private string dateMade;
    private string timeMade;
    public JournalEntry(string entryName, string entryData) {
        this.entryName = entryName;
        this.entryData = entryData;
        this.dateMade = DateTime.Now.ToLongDateString();
        this.timeMade = DateTime.Now.ToLongTimeString();
    }

    public string[] getEntry() {
        return (string[]) [entryName, entryData];
    }
    public string[] getDateAndTime() {
        return (string[]) [dateMade, timeMade];
    } 
}