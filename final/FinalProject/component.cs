class Component : Atom {

    Atom _parent;
    public Component(string name, Atom parent) : base(name) {
        this._parent = parent;
    }
}

class LinkedComponent : Component, IComponentLinkable {
    List<IComponentLinkable> _linked = new List<IComponentLinkable>();

    public LinkedComponent(string name, Atom parent) : base(name, parent)
    {
    }
    public virtual void Trigger(IComponentLinkable data) {
    }

    public bool RecieveLink(IComponentLinkable to_link)
    {
        _linked.Add(to_link);
        return true;
    }

    public bool RecieveUnlink(IComponentLinkable to_delink)
    {
        return _linked.Remove(to_delink);
    }

    public void SendComponent(IComponentLinkable data)
    {
        Trigger(data);
    }

    public bool SendLink(IComponentLinkable to_link)
    {
        if(to_link.RecieveLink(this)) return true;
        return false;
    }

    public bool SendUnlink(IComponentLinkable to_delink)
    {
        if(to_delink.RecieveUnlink(this)) return true;
        return false;
    }
}

class PowerSocket : LinkedComponent, IInteractable
{
    Boolean _powered = false;
    Boolean _source = false;
    public PowerSocket(string name, Atom parent, Boolean state) : base(name, parent)
    {
        _source = state;
        _powered = state;
    }
    public override void Trigger(IComponentLinkable data)
    {
        if(_source) _powered = true;
        if(data is PowerSocket) {
            PowerSocket tdata = (PowerSocket)data;
            if((!tdata.GetPowered() && _powered) || _source ) {
                tdata.SetPowered(true);
                return;
            }
            tdata.SetPowered(false);
        }
    }
    public void SetPowered(Boolean state) {
        _powered = state;
    } 
    public Boolean GetPowered() {
        return _powered;
    }

    public void OnInteract(Atom interactor)
    {
        Console.WriteLine($"This socket is: {_powered}");
    }
}