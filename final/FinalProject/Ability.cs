class Ability {

    string _name;
    string _hotkey = null;
    Action _action;

    public Ability(string name, Action action) {
        _name = name;
        _action = action;
    }
    public Ability(string name, string hotkey, Action action) {
        _name = name;
        _hotkey = hotkey;
        _action = action;
    }
}