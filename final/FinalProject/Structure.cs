
class Structure : Atom
{
    public const int BASE_INTEG = 0;
    protected int _integrity;
    public Structure(string name, char appearance, Loc loc) : base(name, appearance, loc)
    {
        base._anchored = true;
        _integrity = BASE_INTEG;
    }
}

class Wall : Atom
{
    public const int BASE_INTEG = 50;
    public Wall(string name, Loc loc) : base(name, '█', loc)
    {
    }
}

class Button : Structure, IInteractable
{
    public Button(string name, Loc loc) : base(name, 'B', loc)
    {
        
    }
    public void OnInteract(Atom interactor)
    {
        OpenQueryMenu(interactor);
    }
    protected void OpenQueryMenu(Atom interactor) {
        List<Interaction> interactions = new List<Interaction>();
        foreach(Component c in _components) {
            interactions.AddRange(c.GetInteractions());
        }
        Console.Clear();
        for(int i = 0; i < interactions.Count; i ++) {
            Console.WriteLine($"{i+1}: {interactions.ElementAt(i).GetName()}");
        }
        int choice;
        try {
            choice = int.Parse(Console.ReadLine()) - 1;
        } catch {
            Console.WriteLine("Unable to parse choice!");
            Console.ReadLine();
            return;
        }
        interactions.ElementAt(choice).RunAction(interactor);
    }
}
class Machine : Structure
{
    public Machine(string name, char appearance, Loc loc) : base(name, appearance, loc)
    {
    }
}

class LinkedMachine : Machine, IComponentLinkable
{
    public LinkedMachine(string name, char appearance, Loc loc) : base(name, appearance, loc)
    {
    }

    public bool RecieveLink(IComponentLinkable to_link)
    {
        throw new NotImplementedException();
    }

    public bool RecieveUnlink(IComponentLinkable to_delink)
    {
        throw new NotImplementedException();
    }

    public void SendComponent(IComponentLinkable data)
    {
        throw new NotImplementedException();
    }

    public bool SendLink(IComponentLinkable to_link)
    {
        throw new NotImplementedException();
    }

    public bool SendUnlink(IComponentLinkable to_delink)
    {
        throw new NotImplementedException();
    }
}

