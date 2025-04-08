class Creature : MoveableAtom {
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
    protected virtual void InitAbilities() {
        _abilities.Add(new Ability("Go North", "W", ()=>{this.MoveSelf(Direction.north);}));
        _abilities.Add(new Ability("Go Sorth", "S", ()=>{this.MoveSelf(Direction.south);}));
        _abilities.Add(new Ability("Go East", "D", ()=>{this.MoveSelf(Direction.east);}));
        _abilities.Add(new Ability("Go West", "A", ()=>{this.MoveSelf(Direction.west);}));
        _abilities.Add(new Ability("Interact", "Enter", ()=>{this.Interact(_direction_facing);}));
    }
    public void RunAbilityByKey(string key) {
        foreach(Ability i in _abilities) {
            if(i.GetHotkey() == key) {
                i.GetAction()();
                break;
            }
        }
    }
}

class GrippyCreature : Creature
{
    Atom _grabbed = null;
    public GrippyCreature(string name, char appearance, Loc loc) : base(name, appearance, loc)
    {

    }
    public void GrabAtom(Atom to_grab) {
        if(_grabbed != null) {DropAtom();}
        _grabbed = to_grab;
    }
    public void DropAtom() {
        _grabbed = null;
    }
    public override void Interact(Direction to_interact, Atom atom = null) {
        base.Interact(to_interact, _grabbed);
    }
    public override void DisplayAtom(Playfield field)
    {
        base.DisplayAtom(field);
        if(_grabbed != null) Console.WriteLine(_grabbed.GetName());
    }
    protected override void InitAbilities() {
        base.InitAbilities();
        _abilities.Add(new Ability("Drop Item", "Q", ()=>{DropAtom();}));
    }
}