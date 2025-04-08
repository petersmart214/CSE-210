

class Observer : MoveableAtom
{
    new byte[] _traits = [Trait.INCORPOREAL, Trait.CAN_SEE, Trait.CAN_MOVE, Trait.NO_INTERACTION_ABSOLUTE];
    const string observerName = "Observer";
    public Observer(Loc loc) : base(observerName, 'G', loc)
    {

    }
    public Observer() : base(observerName, 'G', new Loc())
    {

    }
}