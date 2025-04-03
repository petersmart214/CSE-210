
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
        Console.WriteLine($"You interacted with: a button!");
    }
}

