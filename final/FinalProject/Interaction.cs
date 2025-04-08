class Interaction
{
    public delegate void InteractionAction(Atom atom);
    string _name;
    InteractionAction _interaction;
    public Interaction(string name, InteractionAction action)
    {
        _name = name;
        _interaction = action;
    }
    public static void OpenQueryMenu(Atom interactor, List<Interaction> interactions)
    {
        Console.Clear();
        for (int i = 0; i < interactions.Count; i++)
        {
            Console.WriteLine($"{i + 1}: {interactions.ElementAt(i).GetName()}");
        }
        int choice;
        try
        {
            choice = int.Parse(Console.ReadLine()) - 1;
        }
        catch
        {
            Console.WriteLine("Unable to parse choice!");
            Console.ReadLine();
            return;
        }
        interactions.ElementAt(choice).RunAction(interactor);
    }
    public string GetName()
    {
        return _name;
    }
    public void RunAction(Atom atom)
    {
        _interaction(atom);
    }
}