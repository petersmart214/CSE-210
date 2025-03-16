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
/*class Menu
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
        //if(!hasObjs) throw new ArgumentException("Static menu items are not supported yet....");
            //Probably a terrible way to do this but it is what it is....
            foreach (string i in displayMenu()) {
                Console.WriteLine(i);
            }
            Console.WriteLine("Input the number specified above to choose an option.");
            while(true) {
                try {
                    int listIndex = queryAction(Console.ReadLine());
                    optionActions[listIndex](hasObjs ? optionObjs[listIndex] : null);
                    break;
                } catch {
                    Console.WriteLine("An error occured while picking the specified item. Please try again");
                }
            }
        }
    
    public void removeItemByString(string toRemove) {
        int indexToRemove = 0;
        foreach (string i in options) {
            if(i.Equals(toRemove)) break;
            indexToRemove ++;
        }
        List<string> tmpoptions = new List<string>(options);
        tmpoptions.RemoveAt(indexToRemove);
        options = tmpoptions.ToArray();

        List<menuOption> tmpoptionsActions = new List<menuOption>(optionActions);
        tmpoptions.RemoveAt(indexToRemove);
        optionActions = tmpoptionsActions.ToArray();
        
        if(hasObjs) {
            List<object> tmpoptionObjs = new List<object>(optionObjs);
            tmpoptions.RemoveAt(indexToRemove);
            optionObjs = tmpoptionObjs.ToArray();
        }
    }

}*/


class Menu
{
    List<MenuOption> _menuOptions;

    public Menu()
    {
        _menuOptions = new List<MenuOption>();
    }
    public void AddOption(IMenuItem to_add)
    {
        _menuOptions.Add(new MenuOptionAction(to_add.GetName(), to_add));
    }
    public Menu AddOption(string name, MenuAction to_add) {
        _menuOptions.Add(new MenuOptionStatic(name, to_add));
        return this;
    }

    public void DisplayMenu()
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
            _menuOptions.ElementAt(choice - 1).RunOption();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Bad argument. Threw exception: {e}");
            return;
        }
    }

    public void DisplayMenuLooping() {
        while(true) {
            Console.Clear();
            int iter = 1;
            foreach (MenuOption option in _menuOptions)
            {
                Console.WriteLine($"{iter}: {option._name}");
                iter++;
            }
            Console.WriteLine("Input the number specified above to choose an option, or back to leave this menu.");

            int choice;
            string choice_string;
            choice_string = Console.ReadLine();
            if(choice_string == "back") break;
            try
            {  
                choice = int.Parse(choice_string);
            }
            catch
            {
                Console.WriteLine("Command not recognized");
                continue;
            }
            try
            {
                _menuOptions.ElementAt(choice - 1).RunOption();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Bad argument. Threw exception: {e}");
                continue;
            }
        }
    }


    public delegate void MenuAction();
    class MenuOption
    {
        public string _name;

        public MenuOption(string name)
        {
            this._name = name;
        }
        public virtual void RunOption() {

        }
    }

    class MenuOptionStatic : MenuOption {
        MenuAction _action;
        public MenuOptionStatic(string name, MenuAction action) : base(name) {
            _action = action;
        }
        public override void RunOption()
        {
            _action();
        }
    }
    class MenuOptionAction : MenuOption {
        public IMenuItem _item;
        public MenuOptionAction(string name, IMenuItem item) : base(name) {
            _item = item;
        }
        public override void RunOption()
        {
            _item.RunOption();
        }
    }

}