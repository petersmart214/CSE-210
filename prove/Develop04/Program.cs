
class Program
{

    static Action[] actions = 
    [new BreathingAction("Breathing Activity", "In this Activity you will breath in and out according to the on-screen prompt."),
    new ReflectionAction("Reflection Activity", "In this Activity you will recieve a prompt. Then, you will recieve a number of questions to anwser about the prompt.",["Think of a time when you stood up for someone else.","Think of a time when you did something really difficult.","Think of a time when you helped someone in need.","Think of a time when you did something truly selfless."],["Why was this experience meaningful to you?","Have you ever done anything like this before?","How did you get started?","How did you feel when it was complete?","What made this time different than other times when you were not as successful?","What is your favorite thing about this experience?","What could you learn from this experience that applies to other situations?","What did you learn about yourself through this experience?","How can you keep this experience in mind in the future?"]),
    new ListingAction("Thought Listing Activity", "In this activity you will recieve a prompt, and proceed to write about it.",["Who are people that you appreciate?","What are personal strengths of yours?","Who are people that you have helped this week?","When have you felt the Holy Ghost this month?","Who are some of your personal heroes?"])];
    static void Main(string[] args)
    {
        List<string> tmpnames = new List<string>();
        foreach(Action i in actions) {
            tmpnames.Add(i._name);
        }
        Menu menu = new Menu(tmpnames.ToArray(), Enumerable.Repeat<Menu.menuOption>((object obj)=>{if(obj is Action) {Action act = (Action)obj; act.run();}}, actions.Length).ToArray(), actions);
        menu.displayAndQuery();
    }
    public static void giveTime(int seconds, string auxMessage = null, Boolean countdown = false) {
        for (int i = 0; i < seconds * 10; i ++) {
            Console.Clear();
            Console.WriteLine(auxMessage);
            if(countdown) Console.WriteLine(seconds - ((int)i / 10)); else
                Console.WriteLine("-----------------------------".Insert(i % 30, "/"));
            Thread.Sleep(100);
        }
        Console.Clear();
        if(auxMessage != null) Console.WriteLine(auxMessage);
    }
}
class Action {
    public string _name {get; init;}
    public string _desc {get; init;}

    protected int _timePassed {get; set;}
    protected int duration = 0;

    public Action(string name, string desc) {
        this._name = name;
        this._desc = desc;
        this._timePassed = 0;        
    }

    public Boolean isDone() {
        return _timePassed >= duration;
    }

    public void run() {
        this.runAction();
        Console.WriteLine("Thank you for participating! Your activity is now completed.");
    }
    public virtual void runAction() {
        Console.WriteLine($"This activity is called a {_name}. Its is described as {_desc}.");
        Console.WriteLine("Hit enter to proceed with this activity.");
        Console.ReadLine();
        Console.WriteLine("Please input the desired activity duration, in seconds.");
        while(true) {
            try {
                duration = int.Parse(Console.ReadLine());
                break;
            } catch {
                Console.WriteLine("Input time is invalid, please try again.");
            }
        }
        Console.Clear();
    }
}

class BreathingAction : Action {
    public BreathingAction(string name, string desc) : base(name, desc) {
    }

    public override void runAction()
    {
        base.runAction();
        Boolean inBreathe = true;
        while(!base.isDone()) {
            if (inBreathe) {
                Program.giveTime(5, "Breathe in....", true);
                inBreathe = false;
            } else {
                Program.giveTime(5, "Breathe out....", true);
                inBreathe = true;
            }
            _timePassed +=5;
        }
    }

}

class PromptAction : Action {

    private string[] promptPool;
    protected string pickedPrompt = "";

    public PromptAction(string name, string desc, string[] promptPool) : base(name, desc) {
        this.promptPool = promptPool;
    }

    protected string pickRandomPrompt() {
        pickedPrompt = promptPool[new Random().Next(0, promptPool.Length)];
        return pickedPrompt;
    }
    
    public override void runAction() {
        base.runAction();
    }
}

class ListingAction : PromptAction {

    private List<string> listedThoughts;
    public ListingAction(string name, string desc, string[] promptPool) : base(name, desc, promptPool) {
        this.listedThoughts = new List<string>();
    }

    public override void runAction()
    {
        base.runAction();
        while(!base.isDone()) {
            Console.Clear();
            pickRandomPrompt();
            Console.WriteLine(pickedPrompt);
            if(listedThoughts.Count > 0) {
                Console.WriteLine("All your current thoughts:");
                Console.WriteLine();
                foreach (string i in listedThoughts) {
                    Console.WriteLine(i);
                }
                Console.WriteLine("Please hit enter to move on");
                Console.ReadLine();
            }
            Program.giveTime(5, pickedPrompt);
            base._timePassed += 5;
            Console.WriteLine("Please input your thoughts.");
            listedThoughts.Add(Console.ReadLine());
        }
        Console.Clear();
        Console.WriteLine("All your current thoughts:");
        foreach (string i in listedThoughts) {
            Console.WriteLine(i);
        }
    }

}

class ReflectionAction : PromptAction {
    private string[] questionPool;
    public ReflectionAction(string name, string desc, string[] promptPool, string[] questionPool) : base(name, desc, promptPool) {
        this.questionPool = questionPool;
    }

    private string pickRandomQuestion() {
        return questionPool[new Random().Next(0, questionPool.Length)];
       
    }
    public override void runAction()
    {
        base.runAction();
        Console.WriteLine(pickRandomPrompt());
        while(!base.isDone()) {
            Console.Clear();
            Console.WriteLine(pickedPrompt);
            Console.WriteLine("Please hit enter to move onto a new question.");
            Console.ReadLine();
            Program.giveTime(5, pickRandomQuestion());
            base._timePassed += 5;
        }
    }

}