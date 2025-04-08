class Component : Atom {

    Atom _parent;
    protected List<Interaction> _interactions = new List<Interaction>();
    public Component(string name, Atom parent) : base(name) {
        this._parent = parent;
    }
    public List<Interaction> GetInteractions() {
        return _interactions;
    }
}

class LinkedComponent : Component, IComponentLinkable {
    List<IComponentLinkable> _linked_input = new List<IComponentLinkable>();
    List<IComponentLinkable> _linked_output = new List<IComponentLinkable>();

    public LinkedComponent(string name, Atom parent) : base(name, parent)
    {
    }
    public virtual void Trigger(IComponentLinkable data) {
    }

    public bool RecieveLink(IComponentLinkable to_link)
    {
        _linked_input.Add(to_link);
        return true;
    }

    public bool RecieveUnlink(IComponentLinkable to_delink)
    {
        return _linked_input.Remove(to_delink);
    }

    public void SendComponent(IComponentLinkable data)
    {
        Trigger(data);
    }

    public bool SendLink(IComponentLinkable to_link)
    {
        if(to_link.RecieveLink(this)) {
            _linked_output.Add(to_link);
            return true;
        }
        return false;
    }

    public bool SendUnlink(IComponentLinkable to_delink)
    {
        if(to_delink.RecieveUnlink(this)) {
            if(_linked_output.Remove(to_delink)) return true;
        }
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
        _interactions.Add(new Interaction("Grab Connector", GrabConnector));
        _interactions.Add(new Interaction("Plug in a Connector", interactor => {this.OnInteract(interactor);}));
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
    public void GrabConnector(Atom interactor) {
        if(interactor is GrippyCreature) {
            GrippyCreature tmp = (GrippyCreature)interactor;
            tmp.GrabAtom(this);
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
        if(interactor is PowerSocket) {
            PowerSocket tmp = (PowerSocket)interactor;
            tmp.SendLink(this);
        }
    }
}