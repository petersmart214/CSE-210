/*class MenuObj {
    string optionName;
    object optionObj;
    Menu.menuOption optionAction;

    public MenuObj(string name, object obj, Menu.menuOption action) {
        this.optionName = name;
        this.optionObj = obj;
        this.optionAction = action;
    }

} */
class Menu
{
    public delegate void menuOption(object obj);
    string[] options;
    object[] optionObjs;
    menuOption[] optionActions;
    //If this is a list of static methods, or needs objects to relate to it
    Boolean hasObjs;

    public Menu(string[] options, menuOption[] optionActions)
    {
        if (options.Length != optionActions.Length) throw new ArgumentException("Bad menu args, a perfect match between options, optionObjs, and optionActions is required.");
        this.options = options;
        this.optionActions = optionActions;
        hasObjs = false;
    }

    public Menu(string[] options, menuOption[] optionActions, object[] optionObjs) {
        if (options.Length != optionActions.Length && options.Length != optionObjs.Length) throw new ArgumentException("Bad menu args, a perfect match between options, optionObjs, and optionActions is required.");
        this.options = options;
        this.optionActions = optionActions;
        this.optionObjs = optionObjs;
        hasObjs = true;
    }

    public string[] displayMenu() {
        List<string> tmpStrList = new List<string>();
        int iter = 1;
        foreach (string i in options)
        {
            tmpStrList.Add($"{iter}: {i}");
            iter++;
        }
        return tmpStrList.ToArray();
    }

    public int queryAction(string numOfAction)
    //Returns the menuOption delegate if input is valid, null if otherwise. Deal with repeat handling if it is null externally.
    {
        return int.Parse(numOfAction) - 1;
    }

    public void displayAndQuery() {
        //TODO: support static menu items
        if(!hasObjs) throw new ArgumentException("Static menu items are not supported yet....");
            //Probably a terrible way to do this but it is what it is....
            foreach (string i in displayMenu()) {
                Console.WriteLine(i);
            }
            Console.WriteLine("Input the number specified above to choose an option.");
            while(true) {
                try {
                    int listIndex = queryAction(Console.ReadLine());
                    optionActions[listIndex](optionObjs[listIndex]);
                    break;
                } catch {
                    Console.WriteLine("An error occured while picking the specified item. Please try again");
                }
            }
        }
}