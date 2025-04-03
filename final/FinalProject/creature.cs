class Creature : Atom {
    private Component[] parts;
    new protected byte[] _traits = [Trait.CAN_MOVE];
    public Creature(string name) : base(name) {
        
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
}