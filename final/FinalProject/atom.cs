

class Atom {
    protected Loc loc = null;
    private Mind mind = null;
    protected Boolean anchored = false;
    protected byte[] traits = [];
    string name;
    private string desc;
    protected Playfield activeField; //This MAY make a death-loop, but who knows!


    //vars that might be better to offload to children classes

    protected char _appearance = '.';

    public Atom(string name) {
        this.name = name;
        this.desc = null;
        this.loc = new Loc();
    }
    public Atom(string name, char appearance) {
        this.name = name;
        this.desc = null;
        _appearance = appearance;
        this.loc = new Loc();
    }
    public Atom(string name, char appearance, Loc loc) {
        this.name = name;
        this.desc = null;
        _appearance = appearance;
        this.loc = loc;
    }
    public virtual Boolean HasMind() {
        return this.mind != null;
    }
    public virtual Boolean HasTrait(byte trait) {
        return traits.Contains(trait);
    }
    public virtual Mind getMind() {
        return this.mind;
    }
    public virtual Atom placeMind(Mind mind) {
        if(this.mind != null) {
            this.expelCurrentMind();
        }
        this.mind = mind;
        return this;
    }
    public virtual Atom setLoc(Loc loc) {
        this.loc = loc;
        return this;
    }
    public virtual void expelCurrentMind() {
        if(mind != null) {
            //Move the mind elsewhere, we dont want a mind to get deleted on accident
            activeField.registerAtom(new Observer().placeMind(this.mind).setLoc(this.loc));
        }
    }
    public virtual char GetAppearance() {
        return _appearance;
    }
    public virtual Loc GetLoc() {
        return loc;
    }
    public virtual void DisplayAtom(Playfield field) {
        Console.WriteLine(View.GetDisplay(field));
    }
}