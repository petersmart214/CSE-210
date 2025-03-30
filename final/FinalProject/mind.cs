

class Mind {
    private string owner;
    private Atom parentAtom;

    public Mind(string owner, Atom parentAtom) {
        this.owner = owner;
        this.parentAtom = parentAtom;
    }

    public Atom getParent() {
        return parentAtom;
    }
    public View GetView() {
        return ConstructView();
    }
    protected View ConstructView() {
        return new View();
    }

}