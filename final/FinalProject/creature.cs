class Creature : MoveableAtom {
    private Component[] _parts;
    new protected byte[] _traits = [Trait.CAN_MOVE];
    protected List<Ability> _abilities = new List<Ability>();

    public Creature(string name, char appearance, Loc loc) : base(name, appearance, loc)
    {
        InitAbilities();
    }

    public virtual string TryDamage(byte damageType, byte forceType, double strength) {
        return "";
    }
    public virtual string TryHeal(byte damageType, double strength, byte forceType = Trait.DAMAGE) {
        return "";
    }
    public virtual Boolean CanInteract(Atom interaction_target) {
        //it is probably not optimal to have ALL creatures check for this, and likely better to have it be overriden, but also being able to add no_interaction traits could be useful
        if (HasTrait(Trait.NO_INTERACTION_ABSOLUTE) || HasTrait(Trait.NO_INTERACTION)) return false;
        return false;
    }
    protected void InitAbilities() {
        _abilities.Add(new Ability("Go North", ()=>{this.MoveSelf(Direction.north);}));
        _abilities.Add(new Ability("Go Sorth", ()=>{this.MoveSelf(Direction.south);}));
        _abilities.Add(new Ability("Go East", ()=>{this.MoveSelf(Direction.east);}));
        _abilities.Add(new Ability("Go West", ()=>{this.MoveSelf(Direction.west);}));
    }
    public List<Ability> GetAbilities() {
        return _abilities;
    }
}