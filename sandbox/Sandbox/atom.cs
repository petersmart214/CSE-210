

class Atom {
    protected Loc loc = null;
    private Mind mind = null;
    protected byte[] traits = [];
    string name;
    private string desc;
    protected Playfield activeField; //This MAY make a death-loop, but who knows!

    public Atom(string name) {
        this.name = name;
        this.desc = null;
        this.loc = new Loc();
    }
    public Atom(string name, string desc) {
        this.name = name;
        this.desc = desc;
        this.loc = new Loc();
    }
    public virtual Boolean hasMind() {
        return this.mind != null;
    }
    public virtual Boolean hasTrait(byte trait) {
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
}