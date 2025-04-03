

class Atom
{
    protected Loc _loc = null;
    private Mind _mind = null;
    protected Boolean _anchored = false;
    protected byte[] _traits = [];
    string _name;
    private string _desc;

    //vars that might be better to offload to children classes

    protected char _appearance = '.';

    public Atom(string name)
    {
        this._name = name;
        this._desc = null;
        this._loc = new Loc();
    }
    public Atom(string name, char appearance)
    {
        this._name = name;
        this._desc = null;
        _appearance = appearance;
        this._loc = new Loc();
    }
    public Atom(string name, char appearance, Loc loc)
    {
        this._name = name;
        this._desc = null;
        _appearance = appearance;
        this._loc = loc;
    }
    public virtual Boolean HasMind()
    {
        return this._mind != null;
    }
    public virtual Boolean HasTrait(byte trait)
    {
        return _traits.Contains(trait);
    }
    public virtual Mind GetMind()
    {
        return this._mind;
    }
    public virtual Atom PlaceMind(Mind mind)
    {
        if (this._mind != null)
        {
            this.expelCurrentMind();
        }
        this._mind = mind;
        return this;
    }
    public virtual Atom SetLoc(Loc loc)
    {
        this._loc = loc;
        return this;
    }
    public virtual void expelCurrentMind()
    {
        if (_mind != null)
        {
            //Move the mind elsewhere, we dont want a mind to get deleted on accident
            this._loc.GetField().RegisterAtom(new Observer().PlaceMind(this._mind).SetLoc(this._loc));
        }
    }
    public virtual char GetAppearance()
    {
        return _appearance;
    }
    public virtual Loc GetLoc()
    {
        return _loc;
    }
    public virtual void DisplayAtom(Playfield field)
    {
        Console.WriteLine(View.GetDisplay(field));
    }
    public void Interact(Direction dir) {
        Atom obj_int = GetLoc().GetField().AtomAtLoc(dir.ApplyDirection(GetLoc()));
        if((obj_int != null) && (obj_int is IInteractable)) {
            IInteractable tmp_int = (IInteractable)obj_int;
            tmp_int.OnInteract(this);
        }
    }
}

class MoveableAtom : Atom, IMoveable
{
    public MoveableAtom(string name, char appearance, Loc loc) : base(name, appearance, loc)
    {
    }

    public void MoveSelf(Direction to_move)
    {
        to_move.ApplyDirectionColliding(GetLoc());
    }
}