class Component : Atom {

    Atom parent;
    public Component(string name, Atom parent) : base(name) {
        this.parent = parent;
    }
}