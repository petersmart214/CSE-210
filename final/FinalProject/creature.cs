class Creature : Atom {
    private Component[] parts;
    new protected byte[] traits = [Trait.CAN_MOVE];
    public Creature(string name) : base(name) {
        
    }
    public virtual string tryDamage(byte damageType, byte forceType, double strength) {
        return "";
    }
    public virtual string tryHeal(byte damageType, double strength, byte forceType = Trait.DAMAGE) {
        return "";
    }
}