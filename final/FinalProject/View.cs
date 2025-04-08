class View
{
    const int RANGE = 10;

    public static string GetDisplay(Playfield field)
    {
        List<string> output = new List<string>();
        for (int i = 0; i < RANGE; i++)
        {
            output.Add(new string('.', RANGE) + "\n");
        }
        foreach (Atom alist in field.GetInPlay())
        {
            char tmp_app = alist.GetAppearance();
            Loc tmp_loc = alist.GetLoc();
            if (tmp_loc.x < 0 || tmp_loc.y < 0)
            {
                continue;
            }
            if (tmp_loc.x > RANGE - 1 || tmp_loc.y > RANGE - 1)
            {
                continue;
            }
            char[] t = output.ElementAt(tmp_loc.y).ToCharArray();
            t[tmp_loc.x] = tmp_app;
            output[tmp_loc.y] = new string(t);
        }
        output.Reverse();
        return string.Join('\n', output);
    }
}