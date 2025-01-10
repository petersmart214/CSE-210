using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

class Program
{
    //Both lists MUST be the same length and all positions must be in the same order.
    //If this wasnt static, I would just make a "baker" type method to make sure they are the same but meh.
    static readonly string[] options = ["View Entries", "Create New Entry", "Remove Entry", "Save Changes", "Load File"];
    static readonly menuOption[] optionActions = [viewEntries, createNewEntry, removeEntry, saveEntries, loadEntries];
        

    //List of prompts given when making a journal
    static readonly string[] prompts = ["What was something interesting that happened today?", "How are you feeling now?", "What from today would you like to happen again?", "What things did you complete today? (even just things like work or school count!)", "What was your favorite thing from today?"];
    static void Main(string[] args)
    {   
        //Conditional to prevent errors from the default file not existing at start. Should try/catch this but man, its just not worth it...
        List<JournalEntry> loadedEntries = Directory.GetFiles(Directory.GetCurrentDirectory(), "journalentries.json").Length != 0 ? Program.loadFromFolder() : new List<JournalEntry>();
        while (true) {
            Console.Clear();
            for (int i = 0; i < options.Length; i ++) {
                Console.WriteLine($"{i}: {options[i]}");
            }
            Console.WriteLine("Input the number specified above to choose an option.");

            int choice;
            try {
            choice = int.Parse(Console.ReadLine());
            } catch {
                Console.WriteLine("Command not recognized");
                continue;
            }
            try {
                optionActions[choice](loadedEntries);
            } catch (Exception e) {
                Console.WriteLine($"Bad argument. Threw exception: {e}");
                continue;
            }
        }


        //print menu
    }
    public delegate List<JournalEntry> menuOption(List<JournalEntry> journalEntries);
    static List<JournalEntry> viewEntries(List<JournalEntry> loadedEntries) {
        foreach (JournalEntry i in loadedEntries) {
            Console.WriteLine($"{i.getEntryName()} / {i.getPromptData()} : {i.getEntryData()} @ {i.getDateAndTime()}");
        }

        Console.WriteLine("Input anything to return");
        Console.ReadLine();
        return loadedEntries;
    }

    static List<JournalEntry> removeEntry(List<JournalEntry> loadedEntries) {
        int countRemoved = 0;
        //terrible thing to do, as I have to iterate over the list more than once to remove items, but I really dont feel like hacking something so I can change a list mid loop
        List<JournalEntry> toRemove = new List<JournalEntry>();

        Console.WriteLine("Please provide the name to remove");
        string nameToRemove = Console.ReadLine();
        foreach (JournalEntry i in loadedEntries) {
            if (i.entryName.Equals(nameToRemove)) {
                toRemove.Add(i);
                countRemoved ++;
            }
        }
        foreach (JournalEntry i in toRemove) {
            loadedEntries.Remove(i);
        }

        Console.WriteLine($"Removed {countRemoved} entries.");

        Console.WriteLine("Input anything to return");
        Console.ReadLine();
        return loadedEntries;
    }
    static List<JournalEntry> createNewEntry(List<JournalEntry> loadedEntries) {
        //print instructions, then ask for name, then data
        Console.WriteLine("Please name this entry, and then a prompt will be given");
        string tmpname = Console.ReadLine();
        string tmpdata = "";
        string tmpprompt = "";
        while(true) {
            tmpprompt = generatePrompt();
            Console.WriteLine(tmpprompt);
            Console.WriteLine("Write something in response to this prompt, or enter / to regenerate the given prompt.");
            tmpdata = Console.ReadLine();
            if (!tmpdata.Equals("/")) break;
        }
        loadedEntries.Add(new JournalEntry(tmpname, tmpprompt, tmpdata));

        Console.WriteLine("Input anything to return");
        Console.ReadLine();
        return loadedEntries;
    }
    static List<JournalEntry> saveEntries(List<JournalEntry> loadedEntries) {
        Console.WriteLine("Please input a desired file name, or input nothing to use the default file name.");
        string fileName = Console.ReadLine();
        Boolean successful = new JournalEntryJSONWrapper(loadedEntries, fileName.Equals("") ? "journalentries" : fileName).save();
        Console.WriteLine(successful ? "Save successful..." : "Save failed...");

        Console.WriteLine("Input anything to return");
        Console.ReadLine();
        return loadedEntries;
    }
    static List<JournalEntry> loadEntries(List<JournalEntry> loadedEntries) {
        Console.WriteLine("Please input the file name to load");
        return loadFromFolder(Console.ReadLine());
    }


    static string generatePrompt() {
        Random tmprand = new Random();
        return prompts[tmprand.Next(0, prompts.Length)];
    }

    static List<JournalEntry> loadFromFolder(string fileToLoad = "journalentries") {
        List<JournalEntry> loadedEntries = new List<JournalEntry>();
        try {
                loadedEntries.AddRange(JsonSerializer.Deserialize<List<JournalEntry>>(File.ReadAllText(string.Concat(fileToLoad, ".json"))));
        return loadedEntries;
        } catch (Exception e) {
            //Maybe hacky? might be better to just let it die without printing this
            Console.WriteLine($"Could not load any entries from local files due to exception: {e}");
            Process.GetCurrentProcess().Kill();
            Process.GetCurrentProcess().WaitForExit();
            return null;
        }
    }
}

class JournalEntry {

    [JsonInclude] public string entryName;
    [JsonInclude] public string promptData;
    [JsonInclude] public string entryData;
    [JsonInclude] public string dateMade;
    [JsonInclude] public string timeMade;
    public JournalEntry(string entryName, string promptData, string entryData) {
        this.entryName = entryName;
        this.entryData = entryData;
        this.promptData = promptData;
        this.dateMade = DateTime.Now.ToLongDateString();
        this.timeMade = DateTime.Now.ToLongTimeString();
    }

    public JournalEntry() {
        /// <summary>
        ///For json deserialization only, using this will create empty strings.
        /// <summary>
        this.entryName = "";
        this.promptData = "";
        this.entryData = "";
        this.dateMade = "";
        this.timeMade = "";
    }
    public JournalEntry(string entryName, string entryData, string promptData, string dateMade, string timeMade) {
        this.entryName = entryName;
        this.entryData = entryData;
        this.promptData = promptData;
        this.dateMade = dateMade;
        this.timeMade = timeMade;
    }
    public string getPromptData() {
        return promptData;
    }
    public string getEntryName() {
        return entryName;
    }
    public string getEntryData() {
        return entryData;
    }
    public string getDateAndTime() {
        return $"{dateMade} {timeMade}";
    }
}


class JournalEntryJSONWrapper {
    [JsonInclude] public List<JournalEntry> journalEntries;
    [JsonIgnore] string fileName;
     public JournalEntryJSONWrapper(List<JournalEntry> journalEntries, string fileName = "journalentries") {
        this.journalEntries = journalEntries;
        this.fileName = string.Concat(fileName, ".json");
    }
    public Boolean save() {
        try {
            System.IO.StreamWriter file = new System.IO.StreamWriter(fileName);
            file.Write(JsonSerializer.Serialize(journalEntries));
            file.Close();
        } catch (Exception e) {
                //TODO: dont just print out errors, maybe log or some such
                Console.WriteLine($"Error while saving: {e}");
                return false;
            }
        return true;
    }
}