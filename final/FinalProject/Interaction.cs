class Interaction {
    public delegate void InteractionAction(Atom atom);
    string _name;
    InteractionAction _interaction;
    public Interaction(string name, InteractionAction action) {
        _name = name;
        _interaction = action;
    }
    public string GetName() {
        return _name;
    }
    public void RunAction(Atom atom) {
        _interaction(atom);
    }
}