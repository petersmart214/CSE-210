using System;

class Program
{
    static char[][] tmp_load = [
    ['w', 'w', 'w', 'w', 'w'], 
    ['w', '.', '.', '.', 'w'], 
    ['w', '.', '.', '.', 'w'],
    ['w', '.', '.', '.', 'w'], 
    ['w', 'w', 'w', 'w', 'w']];
    //static List<Atom> atom_list = new List<Atom>();
    static Playfield field = new Playfield();
    static void Main(string[] args)
    {
        Observer ob_ref = new Observer(new Loc(2, 2));
        field.registerAtom(ob_ref);
        ReadLoad(tmp_load);
        ob_ref.DisplayAtom(field);
    }

    public static void ReadLoad(char[][] list) {
        for(int i = 0; i < list.Length; i ++) {
            char[] c = list[i];
            for (int ii = 0; ii < c.Length; ii ++) {
                char cc = c[ii];
                switch (cc) {
                    case 'w':
                        field.registerAtom(new Wall("barrier", new Loc(ii, i)));
                    break;
                    default:
                    break;
                }
            }
        }
    }
}