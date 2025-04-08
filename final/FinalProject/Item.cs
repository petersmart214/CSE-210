class ConnectorTool : Atom, IInteractable
{
    public ConnectorTool(string name, char appearance, Loc loc) : base(name, appearance, loc)
    {
    }

    public void OnInteract(Atom interactor)
    {
        throw new NotImplementedException();
    }
}