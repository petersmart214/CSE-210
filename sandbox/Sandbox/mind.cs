

class Mind {

    private int view;
    private string owner;
    private Atom parentAtom;

    public Mind(int view, string owner, Atom parentAtom) {
        this.view = view;
        this.owner = owner;
        this.parentAtom = parentAtom;
    }

    public Atom getParent() {
        return parentAtom;
    }

}