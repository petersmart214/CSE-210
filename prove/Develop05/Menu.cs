class Menu
{
    List<MenuOption> _menuOptions;

    public Menu()
    {
        _menuOptions = new List<MenuOption>();
    }
    public void AddOption(List<MenuOption> options)
    {
        _menuOptions.AddRange(options);
    }
    public void AddOption(IMenuItem to_add)
    {
        _menuOptions.Add(new MenuOptionAction(to_add.GetName(), to_add));
    }
    public void AddOption(IMenuItem to_add, AuxOption aux)
    {
        _menuOptions.Add(new MenuOptionAction(to_add.GetName(), to_add, aux));
    }
    public Menu AddOption(string name, MenuAction to_add)
    {
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
            MenuOption tmp_mo = _menuOptions.ElementAt(choice - 1);
            tmp_mo.RunOption();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Bad argument. Threw exception: {e}");
            return;
        }
    }

    public void DisplayMenuLooping()
    {
        while (true)
        {
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
            if (choice_string == "back") break;
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
                MenuOption tmp_mo = _menuOptions.ElementAt(choice - 1);
                tmp_mo.RunOption();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Bad argument. Threw exception: {e}");
                continue;
            }
            Console.WriteLine("Input anything to continue...");
            Console.ReadLine();
        }
    }


}
public delegate void AuxOption(IMenuItem item);
public delegate void MenuAction();
public class MenuOption
{
    public string _name;

    public MenuOption(string name)
    {
        this._name = name;
    }
    public void UpdateName(string name)
    {
        _name = name;
    }
    public virtual void RunOption()
    {

    }
}

class MenuOptionStatic : MenuOption
{
    MenuAction _action;
    public MenuOptionStatic(string name, MenuAction action) : base(name)
    {
        _action = action;
    }
    public override void RunOption()
    {
        _action();
    }
}

class MenuOptionAction : MenuOption
{
    public IMenuItem _item;
    public AuxOption _aux_option = null;
    public MenuOptionAction(string name, IMenuItem item) : base(name)
    {
        _item = item;
    }
    public MenuOptionAction(string name, IMenuItem item, AuxOption aux) : base(name)
    {
        _item = item;
        _aux_option = aux;
    }
    public override void RunOption()
    {
        if (_aux_option != null) _aux_option(_item);
        _item.RunOption();
        UpdateName(_item.GetName());
    }
}
