class Loc {

    public int x = 0;
    public int y = 0;
    public int z = 0;
    protected Playfield _active_field;

    public Loc() {
    }
    public Loc(Playfield active_field, int x, int y) {
        this.x = x;
        this.y = y;
        _active_field = active_field;
    }
    public Loc(Playfield active_field, int x, int y, int z) {
        this.x = x;
        this.y = y;
        this.z = z;
        _active_field = active_field;
    }
    public Playfield GetField() {
        return _active_field;
    }
    public void AddX(int x_to_add) {
        x += x_to_add;
    }
    public void AddY(int y_to_add) {
        y += y_to_add;
    }
    public int GetX() {
        return x;
    }
    public int GetY() {
        return y;
    }
    public Loc Copy() {
        return new Loc(_active_field, x, y, z);
    }
    public Boolean Equals(Loc loc) {
        if ((x == loc.GetX()) && (y == loc.GetY())) {
            return true;
        }
        return false;
    }
}