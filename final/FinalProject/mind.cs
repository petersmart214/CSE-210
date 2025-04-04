

class Mind {
    private string owner;
    public Mind(string owner) {
        this.owner = owner;
    }
    public View GetView() {
        return ConstructView();
    }
    protected View ConstructView() {
        return new View();
    }

}