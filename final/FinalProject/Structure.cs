
using System.ComponentModel;

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
    protected void OpenQueryMenu(Atom interactor)
    {
        List<Interaction> interactions = new List<Interaction>();
        foreach (Component c in _components)
        {
            interactions.AddRange(c.GetInteractions());
        }
        Console.Clear();
        for (int i = 0; i < interactions.Count; i++)
        {
            Console.WriteLine($"{i + 1}: {interactions.ElementAt(i).GetName()}");
        }
        int choice;
        try
        {
            choice = int.Parse(Console.ReadLine()) - 1;
        }
        catch
        {
            Console.WriteLine("Unable to parse choice!");
            Console.ReadLine();
            return;
        }
        interactions.ElementAt(choice).RunAction(interactor);
    }
}

class Machine : Structure, IComponentLinkable
{
    public Machine(string name, char appearance, Loc loc) : base(name, appearance, loc)
    {
    }

    protected List<IComponentLinkable> _linked_input = new List<IComponentLinkable>();
    protected List<IComponentLinkable> _linked_output = new List<IComponentLinkable>();

    public virtual void Trigger(IComponentLinkable data)
    {
    }

    public bool RecieveLink(IComponentLinkable to_link)
    {
        if (to_link == this) return false;
        _linked_input.Add(to_link);
        return true;
    }

    public bool RecieveUnlink(IComponentLinkable to_delink)
    {
        return _linked_input.Remove(to_delink);
    }

    public void SendComponent(IComponentLinkable data)
    {
        Trigger(data);
    }

    public bool SendLink(IComponentLinkable to_link)
    {
        if (to_link.RecieveLink(this))
        {
            _linked_output.Add(to_link);
            return true;
        }
        return false;
    }

    public bool SendUnlink(IComponentLinkable to_delink)
    {
        if (to_delink.RecieveUnlink(this))
        {
            if (_linked_output.Remove(to_delink)) return true;
        }
        return false;
    }
}

class Lever : Machine, IInteractable, IProvider
{
    Boolean _pulled = false;
    public Lever(string name, Loc loc) : base(name, 'L', loc)
    {
        _interactions.Add(new Interaction("Pull the Lever", interactor =>
        {
            if (!_pulled) { _pulled = true; }
            else { _pulled = false; }
            Console.WriteLine($"You pulled the lever into an {(_pulled ? "On" : "Off")} position");
            Console.ReadLine();
        }));
        _interactions.Add(new Interaction("Grab Connector", GrabConnector));
        _interactions.Add(new Interaction("Connect Held Connector", ConnectConnector));
    }

    public void OnInteract(Atom interactor)
    {
        foreach (IComponentLinkable c in _linked_output)
        {
            c.SendComponent(this);
        }
        Interaction.OpenQueryMenu(interactor, _interactions);
    }


    //reused, should be so inherited functionality eventually
    public void GrabConnector(Atom interactor)
    {
        if (interactor is GrippyCreature)
        {
            GrippyCreature tmp = (GrippyCreature)interactor;
            tmp.GrabAtom(this);
        }
    }
    public void ConnectConnector(Atom interactor)
    {
        if (interactor is IComponentLinkable)
        {
            IComponentLinkable tmp = (IComponentLinkable)interactor;
            tmp.SendLink(this);
            tmp.SendComponent(this);
        }
    }

    public bool GetPower()
    {
        Boolean tmp_power = false;
        foreach (IComponentLinkable c in _linked_input)
        {
            if (c is IProvider)
            {
                IProvider tmp_prov = (IProvider)c;
                tmp_power = tmp_power || tmp_prov.GetPower();
            }
        }
        return tmp_power && _pulled;
    }
}

class Lamp : Machine, IInteractable, IProcessable
{
    Boolean _powered = false;
    public Lamp(string name, Loc loc) : base(name, 'O', loc)
    {
        _interactions.Add(new Interaction("Check Lamp Status", interactor =>
        {
            Console.WriteLine($"The lamp is {(_powered ? "On" : "Off")}");
            Console.ReadLine();
        }));
        _interactions.Add(new Interaction("Connect Held Connector", ConnectConnector));
    }
    public override void Trigger(IComponentLinkable data)
    {
        Boolean tmp_power = false;
        foreach (IComponentLinkable c in _linked_input)
        {
            if (c is IProvider)
            {
                IProvider tmp_prov = (IProvider)c;
                _powered = tmp_power || tmp_prov.GetPower();
            }
        }
    }
    public void ConnectConnector(Atom interactor)
    {
        if (interactor is IComponentLinkable)
        {
            IComponentLinkable tmp = (IComponentLinkable)interactor;
            tmp.SendLink(this);
            tmp.SendComponent(this);
        }
    }

    public void OnInteract(Atom interactor)
    {
        Interaction.OpenQueryMenu(interactor, _interactions);
    }

    public void Process()
    {
        foreach (IComponentLinkable c in _linked_input)
        {
            Trigger(c);
        }
    }
}

class GenericMachine : Machine, IInteractable
{
    public GenericMachine(string name, Loc loc) : base(name, 'M', loc)
    {
    }

    public void OnInteract(Atom interactor)
    {
        List<Interaction> interactions = new List<Interaction>();
        foreach (Component c in _components)
        {
            interactions.AddRange(c.GetInteractions());
        }
        Interaction.OpenQueryMenu(interactor, interactions);
    }
}

