/// <summary>
/// the base of (most) everything, basically, if it exists, its an atom or made of them
/// </summary>

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
    protected List<Interaction> _interactions = new List<Interaction>();
    public List<Component> _components = new List<Component>();
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
            this.ExpelCurrentMind();
        }
        this._mind = mind;
        return this;
    }
    public virtual Atom SetLoc(Loc loc)
    {
        this._loc = loc;
        return this;
    }
    public virtual void AddComponent(Component comp) {
        _components.Add(comp);
    }
    public virtual void ExpelCurrentMind()
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
    public virtual string GetName() {
        return _name;
    }
    public virtual Loc GetLoc()
    {
        return _loc;
    }
    public virtual void DisplayAtom(Playfield field)
    {
        Console.Clear();
        Console.WriteLine(View.GetDisplay(field));
    }
    public List<Interaction> GetInteractions() {
        return _interactions;
    }
    public virtual void Interact(Direction dir, Atom atom = null) {
        Atom obj_int = GetLoc().GetField().AtomAtLoc(dir.ApplyDirectionCopy(GetLoc()));
        if((obj_int != null) && (obj_int is IInteractable)) {
            IInteractable tmp_int = (IInteractable)obj_int;
            if(atom == null) {
                tmp_int.OnInteract(this);
                return;
            }
            tmp_int.OnInteract(atom);
        }
    }
}

class MoveableAtom : Atom, IMoveable
{
    protected Direction _direction_facing = Direction.north;
    public MoveableAtom(string name, char appearance, Loc loc) : base(name, appearance, loc)
    {
    }

    public void MoveSelf(Direction to_move)
    {
        to_move.ApplyDirectionColliding(GetLoc());
        _direction_facing = to_move;
    }
}