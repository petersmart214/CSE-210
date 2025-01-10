

class Observer : Atom {
    new byte[] traits = [Trait.INCORPOREAL, Trait.CAN_SEE, Trait.CAN_MOVE];
    const string observerName = "Observer";
    public Observer() : base(observerName) {

    }
}