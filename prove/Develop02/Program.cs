

using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

class Program
{
    //static variables are iffy, but in this case I like the modularity of it (avoid needing passing journals into menuAction delegates, useful for, say, loading a new Journal)
    static readonly string[] DEFAULT_PROMPTS = ["What was something interesting that happened today?", "How are you feeling now?", "What from today would you like to happen again?", "What things did you complete today? (even just things like work or school count!)", "What was your favorite thing from today?"];
    static Journal _openedJournal;
    
    static void Main(string[] args) {
        Menu tmpMenu = new Menu().addOption("Load Journal", loadJournal).addOption("Save Journal", saveJournal).addOption("Add Journal Entry", addEntryJournal).addOption("Remove Journal Entry", removeEntryJournal).addOption("View Journal Entries", viewJournalEntries);
        while(true) {
            Console.Clear();
            tmpMenu.displayMenu();
        }
    }
    
    static void addEntryJournal() {
        if(!checkOpenedJournal()) return;
        //print instructions, then ask for name, then data
        Console.WriteLine("Please name this entry, and then a prompt will be given");
        string tmpname = Console.ReadLine();
        string tmpdata = "";
        string tmpprompt = "";
        while(true) {
            tmpprompt = Program._openedJournal.generatePrompt();
            Console.WriteLine(tmpprompt);
            Console.WriteLine("Write something in response to this prompt, or enter / to regenerate the given prompt.");
            tmpdata = Console.ReadLine();
            if (!tmpdata.Equals("/")) break;
        }
        Program._openedJournal.addEntry(new JournalEntry(tmpname, tmpprompt, tmpdata));

        Console.WriteLine("Input anything to return");
        Console.ReadLine();
    }

    static void viewJournalEntries() {
        if(!checkOpenedJournal()) return;
        foreach (JournalEntry entry in Program._openedJournal._entries) {
            Console.WriteLine($"{entry._name} / {entry._usedPrompt} : {entry._data} @ {entry._time}");
        }

        Console.WriteLine("Input anything to return");
        Console.ReadLine();
    }

    static void removeEntryJournal() {
        if(!checkOpenedJournal()) return;
        Console.WriteLine("Please input the name of the entry you want to remove");
        string toRemove = Console.ReadLine();
        Boolean successful = Program._openedJournal.removeEntry(toRemove);
        Console.WriteLine(successful ? "Removal successful..." : "Removal failed...");
        Console.WriteLine("Input anything to return");
        Console.ReadLine();
    }

    static void saveJournal() {
        Console.WriteLine("Please input the name you want this Journal to be saved as");
        string fileName = string.Concat(Console.ReadLine(), ".json");
        Boolean successful = false;
        try {
            System.IO.StreamWriter file = new System.IO.StreamWriter(fileName);
            file.Write(JsonSerializer.Serialize(_openedJournal));
            file.Close();
            successful = true;
        } catch (Exception e) {
                //TODO: dont just print out errors, maybe log or some such
                Console.WriteLine($"Error while saving: {e}");
        }
        Console.WriteLine(successful ? "Save successful..." : "Save failed...");

        Console.WriteLine("Input anything to return");
        Console.ReadLine();
    }

    static void loadJournal() {
        Console.WriteLine("Please input the file name of the Journal to load, or input NEW to make a new Journal");
        string fileToLoad = Console.ReadLine();
        if(fileToLoad.Equals("NEW")) {
            //using just a generic default prompts const, would be dynamic in other situations but thats beyond this projects scope
            Program._openedJournal = new Journal(Program.DEFAULT_PROMPTS);
            return;
        }
        try {
            Program._openedJournal = JsonSerializer.Deserialize<Journal>(File.ReadAllText(string.Concat(fileToLoad, ".json")));
        } catch (Exception e) {
            //Maybe hacky? might be better to just let it die without printing this
            Console.WriteLine($"Could not load any entries from local files due to exception: {e}");
            Console.WriteLine("Input anything to return");
            Console.ReadLine();
            //Process.GetCurrentProcess().Kill();
            //Process.GetCurrentProcess().WaitForExit();
        }
    }

    static Boolean checkOpenedJournal() {
        if (Program._openedJournal == null) {
            Console.WriteLine("You have not yet loaded a Journal! Load a Journal to get started!");
            Console.WriteLine("Input anything to return");
            Console.ReadLine();
            return false;
        }
        return true;
    }
}

class Journal
{
    [JsonInclude] string[] _prompts;
    [JsonInclude] public List<JournalEntry> _entries;
    /// <summary>
    /// Makes a Journal with a list of prompts and instances _entries as an empty list
    /// </summary>
    /// <param name="prompts"></param>
    public Journal(string[] prompts)
    {
        this._prompts = prompts;
        this._entries = new List<JournalEntry>();
    }
    /// <summary>
    /// Makes an empty Journal for loading via json
    /// </summary>
    /// <param name="prompts"></param>
    /// <param name="entries"></param>
    public Journal()
    {
        this._prompts = [];
        this._entries = new List<JournalEntry>();
    }

    public Boolean addEntry(JournalEntry entry)
    {
        _entries.Add(entry);
        //returns for consistancy
        return true;
    }

    public Boolean removeEntry(string nameToRemove)
    {
        if (!_entries.Any()) return false;
        if (nameToRemove.Equals("last"))
        {
            _entries.RemoveAt(_entries.Count - 1);
            return true;
        }
        JournalEntry toRemove = null;
        foreach (JournalEntry i in _entries)
        {
            if (i._name.Equals(nameToRemove))
            {
                toRemove = i;
                break;
            }
        }
        if (toRemove != null)
        {
            return _entries.Remove(toRemove);
        }
        return false;
    }
    public string generatePrompt() {
        Random tmprand = new Random();
        return this._prompts[tmprand.Next(0, this._prompts.Length)];
    }
}

class JournalEntry
{
    [JsonInclude] public string _name;
    [JsonInclude] public string _usedPrompt;
    [JsonInclude] public string _data;
    [JsonInclude] public string _time;
    /// <summary>
    /// Matkes a Journal Entry by grabbing the current time
    /// </summary>
    /// <param name="name"></param>
    /// <param name="usedPrompt"></param>
    /// <param name="data"></param>
    public JournalEntry(string name, string usedPrompt, string data)
    {
        this._name = name;
        this._usedPrompt = usedPrompt;
        this._data = data;
        this._time = $"{DateTime.Now.ToLongDateString()} at {DateTime.Now.ToLongTimeString()}";
    }
    /// <summary>
    /// Makes an empty JournalEntry for loading via json
    /// </summary>
    /// <param name="name"></param>
    /// <param name="usedPrompt"></param>
    /// <param name="data"></param>
    /// <param name="time"></param>
    public JournalEntry()
    {
        this._name = "";
        this._usedPrompt = "";
        this._data = "";
        this._time = "";
    }
}

class Menu
{

    List<MenuOption> _menuOptions;

    public Menu()
    {
        _menuOptions = new List<MenuOption>();
    }

    public Menu addOption(string name, menuAction action)
    {
        _menuOptions.Add(new MenuOption(name, action));
        return this;
    }

    public void displayMenu()
    {
        Console.Clear();
        int iter = 1;
        foreach (MenuOption option in _menuOptions)
        {
            Console.WriteLine($"{iter}: {option._name}");
            iter++;
        }
        Console.WriteLine("Input the number specified above to choose an option.");

        int choice;
        try
        {
            choice = int.Parse(Console.ReadLine());
        }
        catch
        {
            Console.WriteLine("Command not recognized");
            return;
        }
        try
        {
            _menuOptions.ElementAt(choice - 1)._action();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Bad argument. Threw exception: {e}");
            return;
        }
    }


    public delegate void menuAction();
    class MenuOption
    {
        public string _name;
        public menuAction _action;

        public MenuOption(string name, menuAction action)
        {
            this._name = name;
            this._action = action;
        }
    }

}