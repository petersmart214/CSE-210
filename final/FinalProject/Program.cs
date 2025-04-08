using System;

class Program
{
    static char[][] tmp_load = [
    ['w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w'],
    ['w', 'O', '.', 'M', '.', '.', '.', '.', '.', 'w'],
    ['w', '.', '.', '.', '.', '.', '.', '.', '.', 'w'],
    ['w', '.', 'L', '.', '.', '.', '.', '.', '.', 'w'],
    ['w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w']];
    //static List<Atom> atom_list = new List<Atom>();
    static Playfield field = new Playfield();
    static List<IProcessable> proc_list = new List<IProcessable>();
    static void Main(string[] args)
    {
        GrippyCreature ob_ref = new GrippyCreature("The Fool", '@', new Loc(field, 2, 2));
        Mind player = new Mind("Me");
        field.RegisterAtom(ob_ref);
        ob_ref.PlaceMind(player);
        ReadLoad(tmp_load);
        Console.Clear();
        while (true)
        {
            foreach (IProcessable proc in proc_list)
            {
                proc.Process();
            }
            ob_ref.DisplayAtom(field);
            if (Console.KeyAvailable)
            {
                ob_ref.RunAbilityByKey(Console.ReadKey().Key.ToString());
            }
            Thread.Sleep(200);
        }
    }

    public static void ReadLoad(char[][] list)
    {
        for (int i = 0; i < list.Length; i++)
        {
            char[] c = list[i];
            for (int ii = 0; ii < c.Length; ii++)
            {
                char cc = c[ii];
                switch (cc)
                {
                    case 'w':
                        field.RegisterAtom(new Wall("barrier", new Loc(field, ii, i)));
                        break;
                    case 'O':
                        Lamp lamp = new Lamp("Bright Lamp", new Loc(field, ii, i));
                        proc_list.Add(lamp);
                        field.RegisterAtom(lamp);
                        break;
                    case 'L':
                        Lever lever = new Lever("Old Lever", new Loc(field, ii, i));
                        field.RegisterAtom(lever);
                        break;
                    case 'M':
                        GenericMachine gmach = new GenericMachine("Ancient Generator", new Loc(field, ii, i));
                        gmach.AddComponent(new Generator("The Core", gmach));
                        field.RegisterAtom(gmach);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}