class Direction
{
    public static readonly Direction north = new Direction(0, 1);
    public static readonly Direction south = new Direction(0, -1);
    public static readonly Direction east = new Direction(1, 0);
    public static readonly Direction west = new Direction(-1, 0);
    int _dir_x;
    int _dir_y;
    public Direction(int x, int y)
    {
        _dir_x = x;
        _dir_y = y;
    }
    public Loc ApplyDirectionCopy(Loc loc)
    {
        return ApplyDirection(loc.Copy());
    }
    public Loc ApplyDirection(Loc loc)
    {
        loc.AddX(_dir_x);
        loc.AddY(_dir_y);
        return loc;
    }
    public void ApplyDirectionColliding(Loc loc)
    {
        Playfield tmp_field = loc.GetField();
        Loc proj_loc = loc.Copy();
        ApplyDirection(proj_loc);
        Boolean obstructed = false;
        tmp_field.EnumerateByLoc(loc, a => { if (a.GetLoc().Equals(proj_loc)) obstructed = true; });
        if (!obstructed)
        {
            ApplyDirection(loc);
        }
    }
}