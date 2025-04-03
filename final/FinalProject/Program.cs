using System;

class Program
{
    static char[][] tmp_load = [
    ['w', 'w', 'w', 'w', 'w'], 
    ['w', '.', '.', '.', 'w'], 
    ['w', '.', '.', '.', 'w'],
    ['w', '.', 'b', '.', 'w'], 
    ['w', 'w', 'w', 'w', 'w']];
    //static List<Atom> atom_list = new List<Atom>();
    static Playfield field = new Playfield();
    static void Main(string[] args)
    {
        Observer ob_ref = new Observer(new Loc(field, 2, 2));
        field.RegisterAtom(ob_ref);
        ReadLoad(tmp_load);
        ob_ref.MoveSelf(Direction.north);
        ob_ref.MoveSelf(Direction.north);
        ob_ref.Interact(Direction.north);
        ob_ref.DisplayAtom(field);
    }

    public static void ReadLoad(char[][] list) {
        for(int i = 0; i < list.Length; i ++) {
            char[] c = list[i];
            for (int ii = 0; ii < c.Length; ii ++) {
                char cc = c[ii];
                switch (cc) {
                    case 'w':
                        field.RegisterAtom(new Wall("barrier", new Loc(field, ii, i)));
                    break;
                    case 'b':
                        field.RegisterAtom(new Button("button", new Loc(field, ii, i)));
                        break;
                    default:
                    break;
                }
            }
        }
    }
}