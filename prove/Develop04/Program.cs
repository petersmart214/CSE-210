

using System.ComponentModel;
using System.Runtime.InteropServices;

class Program
{
    static string[] REFLECTION_QUESTION_POOL = ["Why was this experience meaningful to you?", "Have you ever done anything like this before?", "How did you get started?", "How did you feel when it was complete?", "What made this time different than other times when you were not as successful?", "What is your favorite thing about this experience?", "What could you learn from this experience that applies to other situations?", "What did you learn about yourself through this experience?", "How can you keep this experience in mind in the future?"];
    static string[] REFLECTION_PROMPT_POOL = ["Think of a time when you stood up for someone else.", "Think of a time when you did something really difficult.", "Think of a time when you helped someone in need.", "Think of a time when you did something truly selfless."];
    static string[] LISTING_PROMPT_POOL = ["Who are people that you appreciate?", "What are personal strengths of yours?", "Who are people that you have helped this week?", "When have you felt the Holy Ghost this month?", "Who are some of your personal heroes?"];
    static void Main(string[] args)
    {
        List<Activity> activities = new List<Activity>();
        activities.Add(new BreathingActivity("Breathing Activity", "In this Activity you will breath in and out according to the on-screen prompt."));
        activities.Add(new ReflectionActivity("Reflection Activity", "In this Activity you will recieve a prompt. Then, you will recieve a number of questions to anwser about the prompt.", REFLECTION_PROMPT_POOL, REFLECTION_QUESTION_POOL));
        activities.Add(new ListingActivity("Listing Activity", "In this activity you will recieve a prompt, and proceed to write about it.", LISTING_PROMPT_POOL));
        Menu menu = new Menu();
        activities.ForEach(menu.AddOption);
        menu.DisplayMenu();
    }
}

class Activity : IMenuItem
{
    const int PAUSE_TIME = 5;
    /// <summary>
    /// how long in milliseconds an animation should wait before the next animation step
    /// </summary>
    const int ANIMATE_LENGTH = 500;
    protected string _name;
    protected string _desc;
    protected long _timeToEnd;
    protected int _duration;

    public Activity(string name, string desc)
    {
        _name = name;
        _desc = desc;
        _duration = 0;
        _timeToEnd = -1;
    }

    public virtual void RunActivity()
    {
        Console.WriteLine(GetIntro());
        _duration = QueryIntLooping();
        RunAnimation(PAUSE_TIME, "Prepare to begin...");
    }
    public void FinishActivity()
    {
        Console.WriteLine("Thank you for completing this activity.");
    }

    public string GetIntro()
    {
        return $"{_name}\n  {_desc}";
    }
    public void SetStart()
    {
        _timeToEnd = DateTime.Now.AddSeconds(_duration).Ticks;
    }

    public Boolean GetFinished()
    {
        return DateTime.Now.Ticks >= _timeToEnd;
    }
    /// <summary>
    /// Queries the user to provide an int, will loop forever if it cant parse any provided input.
    /// </summary>
    /// <returns></returns>
    private static int QueryIntLooping()
    {
        while (true)
        {
            Console.WriteLine("\nPlease input the duration (in seconds) you want to do this activity!");
            try
            {
                int rtrn_int = int.Parse(Console.ReadLine());
                return rtrn_int;
            }
            catch
            {
                Console.WriteLine("Bad Input!");
            }
        }
    }

    public void RunAnimation(int animate_time, string message = null)
    {
        DateTime tmpdt = DateTime.Now;
        for (int iter = 0; iter < ((animate_time * 1000) / ANIMATE_LENGTH); iter++)
        {
            string tmp_bar = "---------";
            Console.Clear();
            if (message != null) Console.WriteLine(message);
            Console.WriteLine(tmp_bar.Insert(iter % tmp_bar.Length, "|"));
            Thread.Sleep(ANIMATE_LENGTH);
        }
        Console.WriteLine(DateTime.Now.Subtract(tmpdt).ToString());
    }

    public string GetName()
    {
        return _name;
    }

    public void RunOption()
    {
        RunActivity();
    }
}

class PromptActivity : Activity
{
    protected string[] _prompts;
    public PromptActivity(string name, string desc, string[] prompts) : base(name, desc)
    {
        _prompts = prompts;
    }
    public static string GetPromptFromStr(string[] list)
    {
        Random tmp_rand = new Random();
        return list[tmp_rand.Next(0, list.Length - 1)];
    }
}

class BreathingActivity : Activity
{
    public BreathingActivity(string name, string desc) : base(name, desc) { }

    public override void RunActivity()
    {
        base.RunActivity();
        SetStart();
        while (!GetFinished())
        {
            RunAnimation(5, "Breathe in...");
            RunAnimation(5, "Breathe out...");
        }
        FinishActivity();
    }
}

class ReflectionActivity : PromptActivity
{
    private string[] _questions;
    public ReflectionActivity(string name, string desc, string[] prompts, string[] questions) : base(name, desc, prompts)
    {
        _questions = questions;
    }

    public override void RunActivity()
    {
        base.RunActivity();
        RunAnimation(10, GetPromptFromStr(_prompts));
        SetStart();
        while (!GetFinished())
        {
            RunAnimation(5, GetPromptFromStr(_questions));
        }
        FinishActivity();
    }
}

class ListingActivity : PromptActivity
{
    private List<string> _listed;
    public ListingActivity(string name, string desc, string[] prompts) : base(name, desc, prompts)
    {
        _listed = new List<string>();
    }
    public override void RunActivity()
    {
        base.RunActivity();
        _listed = new List<string>();
        string tmp_prompt = GetPromptFromStr(_prompts);
        SetStart();
        while (!GetFinished())
        {
            RunAnimation(5, tmp_prompt);
            Console.Clear();
            Console.WriteLine(tmp_prompt);
            _listed.Add(Console.ReadLine());
            Console.Clear();
        }
        Console.WriteLine($"You listed {_listed.Count} things in response to this prompt!");
        FinishActivity();
    }
}


class Menu
{
    List<MenuOption> _menuOptions;

    public Menu()
    {
        _menuOptions = new List<MenuOption>();
    }
    public void AddOption(IMenuItem to_add)
    {
        _menuOptions.Add(new MenuOption(to_add.GetName(), to_add));
    }

    public void DisplayMenu()
    {
        Console.Clear();
        int iter = 1;
        foreach (MenuOption option in _menuOptions)
        {
            Console.WriteLine($"{iter}: {option._name}");
            iter++;
        }
        Console.WriteLine("Input the number specified above to choose an option.");

        int choice;
        try
        {
            choice = int.Parse(Console.ReadLine());
        }
        catch
        {
            Console.WriteLine("Command not recognized");
            return;
        }
        try
        {
            _menuOptions.ElementAt(choice - 1)._action.RunOption();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Bad argument. Threw exception: {e}");
            return;
        }
    }


    public delegate void menuAction();
    class MenuOption
    {
        public string _name;
        public IMenuItem _action;

        public MenuOption(string name, IMenuItem action)
        {
            this._name = name;
            this._action = action;
        }
    }

}
