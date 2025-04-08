using System;

class Program
{
    static char[][] tmp_load = [
    ['w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w'], 
    ['w', 's', '.', '.', '.', '.', '.', '.', '.', 'w'], 
    ['w', '.', '.', '.', '.', '.', '.', '.', '.', 'w'],
    ['w', '.', 'b', '.', '.', '.', '.', '.', '.', 'w'], 
    ['w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w']];
    //static List<Atom> atom_list = new List<Atom>();
    static Playfield field = new Playfield();
    static Button b;
    static Button s;
    static void Main(string[] args)
    {
        GrippyCreature ob_ref = new GrippyCreature("The Fool", '@', new Loc(field, 2, 2));
        Mind player = new Mind("Me");
        field.RegisterAtom(ob_ref);
        ob_ref.PlaceMind(player);
        ReadLoad(tmp_load);
        IComponentLinkable btmp = (IComponentLinkable)b._components.ElementAt(0);
        IComponentLinkable stmp = (IComponentLinkable)s._components.ElementAt(0);
        Console.Clear();
        while (true) {
        ob_ref.DisplayAtom(field);
        if(Console.KeyAvailable) {
            ob_ref.RunAbilityByKey(Console.ReadKey().Key.ToString());
        }
        Thread.Sleep(200);
        }
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
                        Button tmp = new Button("button", new Loc(field, ii, i));
                        tmp.AddComponent(new PowerSocket("notsource", tmp, false));
                        field.RegisterAtom(tmp);
                        b = tmp;
                        break;
                    case 's':
                        Button ttmp = new Button("socketbutton", new Loc(field, ii, i));
                        ttmp.AddComponent(new PowerSocket("source", ttmp, true));
                        field.RegisterAtom(ttmp);
                        s = ttmp;
                        break;
                    default:
                    break;
                }
            }
        }
    }
}