using System;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;



class Program
{
    const string filename = "temp.json";
    static Boolean cont = true;
    static Player currentPlayer = null;
    static List<Goal> goals = new List<Goal>();
    static Menu mainMenu = new Menu(["Create New Goal","Progress Goal","Save Goals","Quit"], [createNewGoal, progressGoal, save, quit]);
    static Menu progressMenu;
    static void Main(string[] args) {
        //Loading
        load(null);
        //Setup
        if (currentPlayer == null) {
            Console.WriteLine("Please input your name!");
            currentPlayer = new Player(Console.ReadLine());
        }
        while (cont) {
            mainMenu.displayAndQuery();
        }
    }

    static void createNewGoal(object obj) {
        Console.WriteLine("To make a new goal, please choose a name for it.");
        string tmpname = Console.ReadLine();
        Console.WriteLine("Now, add a description to it. Alternatively, you may leave it blank.");
        string tmpdesc = Console.ReadLine();
        Boolean goalOut = false;
        while (true) {
            Console.WriteLine("Please choose the type of goal this is.");
            Console.WriteLine("1: Simple Goal");
            Console.WriteLine("2: Eternal Goal");
            Console.WriteLine("3: Repeated Goal");
            string tmpgoaltype = Console.ReadLine();
            try {
                switch (tmpgoaltype) {
                    case "1":
                        Console.WriteLine("Please input a point value for when this item is completed.");
                        goals.Add(new SimpleGoal(tmpname, tmpdesc, currentPlayer, int.Parse(Console.ReadLine())));
                        goalOut = true;
                    break;
                    case "2":
                        Console.WriteLine("Please input a point value for when this item is completed.");
                        goalOut = true;
                    break;
                    case "3":
                        Console.WriteLine("Please input the amount of desired repetitions.");
                        int tmpNumRepeat = int.Parse(Console.ReadLine());
                        Console.WriteLine("Please input the value per completetion.");
                        int tmpminorval = int.Parse(Console.ReadLine());
                        Console.WriteLine("Please input the value when it is finished.");
                        int tmpmajorval = int.Parse(Console.ReadLine());
                        goals.Add(new ChecklistGoal(tmpname, tmpdesc, currentPlayer, tmpminorval, tmpmajorval, tmpNumRepeat));
                        goalOut = true;
                    break;
                    default: Console.WriteLine("Type not recognized, please Try again.");
                    break;
                    }
                } catch {
                    Console.WriteLine("Something went wrong! Please try again.");
                }
                goals.ForEach((Goal i) => {Console.WriteLine(i);});
                if(goalOut) break;
            }
        

        }
    static void viewLogs(object obj) {

    }
    static void save(object obj) {
        Console.WriteLine("Saving goals....");
         try {
            System.IO.StreamWriter file = new System.IO.StreamWriter(filename);
            file.Write(JsonSerializer.Serialize(goals));
            file.Close();
        } catch (Exception e) {
                //TODO: dont just print out errors, maybe log or some such
                Console.WriteLine($"Error while saving: {e}");
            }
    }
    static void load(object obj) {
        if(File.Exists(filename)) {
            try {
                    goals.AddRange(JsonSerializer.Deserialize<List<Goal>>(File.ReadAllText(string.Concat(filename, ".json"))));
            } catch (Exception e) {
                //Maybe hacky? might be better to just let it die without printing this
                Console.WriteLine($"Could not load any entries from local files due to exception: {e}");
            }
        }
    }
    static void progressGoal(object obj) {
        List<string> tmpnames = new List<string>();
            foreach(Goal i in goals) {
                tmpnames.Add(i.getName());
            }
        progressMenu = new Menu(tmpnames.ToArray(), Enumerable.Repeat<Menu.menuOption>((object obj)=>{if(obj is Goal) {Goal goal = (Goal)obj; goal.progress(null);}}, goals.Count).ToArray(), goals.ToArray());
        List<Goal> toRemove = new List<Goal>();
        foreach(Goal i in goals) {
            if(i.shouldBeDeleted()) toRemove.Add(i);
        }
        toRemove.ForEach((i)=>{goals.Remove(i);});
    }
    static void quit(object obj) {
        cont = false;
    }

}

class Player {
    protected string name;
    protected int collectedPoints;
    protected string earnedTitle;

    public Player(string name) {
        this.name = "";
        this.collectedPoints = 0;
        this.earnedTitle = "";
    }

    public void addToPoints(int points) {
        Console.WriteLine($"You have recieved {this.collectedPoints}.");
        this.collectedPoints += points;
    }

}


class Goal {
    
    [JsonInclude] protected Player playerReference;
    [JsonInclude] protected string name;
    [JsonInclude] protected string desc;
    [JsonInclude] protected Boolean completed = false;
    
    public Goal(string name, string desc, Player player) {
        this.name = name;
        this.desc = desc;
        this.playerReference = player;
    }
    static string getDateAndTime() {
        return $"{DateTime.Now.ToLongDateString()} {DateTime.Now.ToLongTimeString()}";
    }
    public string getName() {
        return name;
    }
    public string getDesc() {
        return desc;
    }

    public virtual void progress(object obj) {
        this.displayGoal();
        this.playerReference.addToPoints(this.updateGoal());
    }

    public virtual string displayGoal() {
        return $"{Goal.getDateAndTime()}: Goal Type: {name}; ";
    }
    public virtual int updateGoal() {
        return 0;
    }
    public virtual Boolean shouldBeDeleted() {
        return completed;
    }

}
class SimpleGoal : Goal {
    [JsonInclude] protected int completedPoints;
    public SimpleGoal(string name, string desc, Player player, int completedPoints) : base(name, desc, player) {
        this.completedPoints = completedPoints;
    }
    public override int updateGoal() {
        completed = true;
        return completedPoints;
    }
    public override string displayGoal() {
        return $"{base.displayGoal()}Points Recieved: {completedPoints};";
    }
}
class ChecklistGoal : Goal {

    [JsonInclude] protected int minorPoints;
    [JsonInclude] protected int majorPoints;
    [JsonInclude] protected int numToGetMajor;
    [JsonInclude] private int numCompleted = 0;

    public ChecklistGoal(string name, string desc, Player player, int minorPoints, int majorPoints, int numToGetMajor) : base(name, desc,player) {
        this.minorPoints = minorPoints;
        this.majorPoints = majorPoints;
        this.numToGetMajor = numToGetMajor;
    }
    public override int updateGoal() {
        if (numCompleted >= numToGetMajor) {
            numCompleted = 0;
            completed = true;
            return majorPoints;
        }
        return minorPoints;
    }
    public override string displayGoal() {
        return $"{base.displayGoal()}Points Recieved: {(numCompleted >= numToGetMajor ? majorPoints : minorPoints)}; Number of Times Completed: {numCompleted}/{numToGetMajor};";
    }
}