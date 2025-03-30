

class Observer : Atom {
    new byte[] traits = [Trait.INCORPOREAL, Trait.CAN_SEE, Trait.CAN_MOVE, Trait.NO_INTERACTION_ABSOLUTE];
    const string observerName = "Observer";
    public Observer() : base(observerName) {

    }
}