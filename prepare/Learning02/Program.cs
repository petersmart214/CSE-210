using System;
using System.Data;

class Program
{
    static void Main(string[] args)
    {
        Employee jerry = new Employee("Jerry Johnson", new List<Job>([new Job("CEO", "JERRYRIG HARDWARE", "1999 - 2024"), new Job("Janitor", "JERRYRIG HARDWARE", "2024 - 2024")]));
        jerry.displayDetails();
    }
}


class Job {

    string _title;
    string _company;
    //could do timeframe with an int tuple but eh
    string _timeframe;

    public Job(string title, string company, string timeframe) {
        this._title = title;
        this._company = company;
        this._timeframe = timeframe;
    }

    public string getFormatedString() {
        return $"{this._title} ({this._company}) {this._timeframe}";
    }
}

class Employee {
    string _name;
    List<Job> _jobs;

    public Employee(string name) {
        this._name = name;
        _jobs = new List<Job>();
    }

    public Employee(string name, List<Job> jobs) {
        this._name = name;
        this._jobs = jobs;
    }

    public void addJob(Job job) {
        this._jobs.Add(job);
    }

    public void displayDetails() {
        Console.WriteLine($"Name: {_name}");
        Console.WriteLine("Jobs: ");
        foreach(Job iter in this._jobs) {
            Console.WriteLine(iter.getFormatedString());
        }
    }
}



//from retake
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