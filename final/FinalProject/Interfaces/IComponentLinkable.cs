interface IComponentLinkable
{
    public Boolean SendLink(IComponentLinkable to_link);
    public Boolean RecieveLink(IComponentLinkable to_link);
    public Boolean SendUnlink(IComponentLinkable to_delink);
    public Boolean RecieveUnlink(IComponentLinkable to_delink);
    public void SendComponent(IComponentLinkable data);
}