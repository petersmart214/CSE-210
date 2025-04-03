class Component : Atom {

    Atom _parent;
    public Component(string name, Atom parent) : base(name) {
        this._parent = parent;
    }
}