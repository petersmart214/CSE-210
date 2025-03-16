class Program
{
    static List<Player> player_list = new List<Player>();
    const string filename = "default";
    const string OBJ_SEPERATOR = "\n|\n";
    public static SimpleGoal FILLER_GOAL = new SimpleGoal("Make a goal", "Use this program to make a goal.", 100);
    public static void Main(string[] args)
    {
        LoadFactory.InitLoaders();
        Menu MainMenu = new Menu();
        MainMenu.AddOption("Choose Player", OpenPlayer).AddOption("Make New Player", MakeNewPlayer).AddOption("Load Players", LoadPlayers).AddOption("Save Players", SavePlayers);
        MainMenu.DisplayMenuLooping();
    }


    public static void OpenPlayer()
    {
        Menu player_menu = new Menu();
        player_list.ForEach(player_menu.AddOption);
        player_menu.DisplayMenuLooping();
    }
    public static void MakeNewPlayer()
    {
        Console.WriteLine("Please input a name for this player!");
        string tmp_name = Console.ReadLine();
        player_list.Add(new Player(tmp_name, "Vaguely Self-Driven"));
    }
    public static void LoadPlayers()
    {
        try
        {
            string[] player_list_strings = File.ReadAllText(string.Concat(filename, ".txt")).Split(OBJ_SEPERATOR);
            foreach (string i in player_list_strings)
            {
                player_list.Add((Player)LoadFactory.LoadObj(Player.TOKEN, i));
            }
        }
        catch (Exception e)
        {
            //Maybe hacky? might be better to just let it die without printing this
            Console.WriteLine($"Could not load any entries from local files due to exception: {e}");
            Console.WriteLine("Input anything to return");
            Console.ReadLine();
            //Process.GetCurrentProcess().Kill();
            //Process.GetCurrentProcess().WaitForExit();
        }
    }
    public static void SavePlayers()
    {
        Boolean successful = false;
        try
        {
            string to_write = "";
            System.IO.StreamWriter file = new System.IO.StreamWriter($"{filename}{".txt"}");
            for (int i = 0; i < player_list.Count; i++)
            {
                if (i < player_list.Count - 1)
                {
                    to_write += $"{player_list[i].Save()}{OBJ_SEPERATOR}";
                    continue;
                }
                to_write += $"{player_list[i].Save()}";

            }
            file.Write(to_write);
            file.Close();
            successful = true;
        }
        catch (Exception e)
        {
            //TODO: dont just print out errors, maybe log or some such
            Console.WriteLine($"Error while saving: {e}");
        }
        Console.WriteLine(successful ? "Save successful..." : "Save failed...");

        Console.WriteLine("Input anything to return");
        Console.ReadLine();
    }
}

class LoadFactory
{
    public delegate IFileable LoadFromString(string data_in);
    public static Dictionary<string, LoadFromString> loaders = new Dictionary<string, LoadFromString>();
    public static void InitLoaders()
    {
        loaders.Add(Player.TOKEN, (string i) => { return new Player(i); });
        loaders.Add(SimpleGoal.TOKEN, (string i) => { return new SimpleGoal(i); });
        loaders.Add(EternalGoal.TOKEN, (string i) => { return new EternalGoal(i); });
        loaders.Add(ChecklistGoal.TOKEN, (string i) => { return new ChecklistGoal(i); });
    }
    public static IFileable LoadObj(string token, string data_in)
    {
        return loaders[token](data_in);
    }
}

class Player : IFileable, IMenuItem
{
    public const string TOKEN = "player";
    const string SEPERATOR = "\n";
    protected string _player_name = "Unloaded Error";
    protected string _player_title = "Unloaded Error";
    protected List<Goal> _goal_list = new List<Goal>();
    protected int _earned_points = -1;

    public Player(string name, string title)
    {
        _player_name = name;
        _player_title = title;
        _earned_points = 0;
    }

    public Player(string load_data)
    {
        this.Load(load_data);
    }

    public void AddGoal(Goal goal)
    {
        _goal_list.Add(goal);
    }

    public string GetFormatedName()
    {
        return null;
    }
    public void AddPoints(int points_to_add)
    {
        _earned_points += points_to_add;
        //check for cool titles
        decimal points_bracket = Math.Floor((decimal)(_earned_points / 500));
        string previous_title = _player_title;
        switch (points_bracket)
        {
            case 0:
                _player_title = "Vaguely Self-Driven";
                break;
            case 1:
                _player_title = "Somewhat Self-Driven";
                break;
            case 2:
                _player_title = "Decently Self-Driven";
                break;
            case 3:
                _player_title = "Self-Driven";
                break;
            case 4:
                _player_title = "Highly Self-Driven";
                break;
            case 5:
                _player_title = "Excellently Self-Driven";
                break;
            default:
                _player_title = "Excellently Self-Driven";
                break;
        }
        if (_player_title != previous_title) Console.WriteLine($"Your title is now {_player_title}! Congratulations!");
    }
    public void Load(string to_load)
    {
        string[] tmp_data = to_load.Split(SEPERATOR);
        tmp_data = tmp_data.ToList().GetRange(1, tmp_data.Length - 1).ToArray();
        _player_name = tmp_data[0];
        _player_title = tmp_data[1];
        _earned_points = int.Parse(tmp_data[2]);
        if (tmp_data.Length <= 3) return;
        for (int i = 3; i < tmp_data.Length; i++)
        {
            string[] tmp_goal = tmp_data[i].Split(Goal.GOAL_SEPORATOR);
            _goal_list.Add((Goal)LoadFactory.LoadObj(tmp_goal[0], String.Join(Goal.GOAL_SEPORATOR, tmp_goal.ToList().GetRange(1, tmp_goal.Length - 1))));
        }
    }

    public string Save()
    {
        if (_goal_list.Count <= 0)
        {
            _goal_list.Add(Program.FILLER_GOAL);
        }
        string saved_goals = "";
        for (int i = 0; i < _goal_list.Count; i++)
        {
            if (i < _goal_list.Count - 1)
            {
                saved_goals = $"{saved_goals}{_goal_list[i].Save()}{SEPERATOR}";
                continue;
            }
            saved_goals = $"{saved_goals}{_goal_list[i].Save()}";
        }
        return $"{GetToken()}{SEPERATOR}{_player_name}{SEPERATOR}{_player_title}{SEPERATOR}{_earned_points}{SEPERATOR}{saved_goals}";
    }

    public string GetToken()
    {
        return TOKEN;
    }

    public string GetName()
    {
        return _player_name;
    }

    public void RunOption()
    {
        Menu player_menu = new Menu();
        player_menu.AddOption("Add new Goal", () =>
        {
            this.AddGoal();
        });
        player_menu.AddOption("Make Progress on Goals", () =>
        {
            Menu sub_goal_menu = new Menu();
            foreach (Goal goal in _goal_list)
            {
                sub_goal_menu.AddOption(goal, (IMenuItem i) => { Goal tmpgoal = (Goal)i; this.AddPoints(tmpgoal.GetPointsToAdd()); });
            }
            sub_goal_menu.DisplayMenuLooping();
        });
        player_menu.AddOption("View Points", () =>
        {
            Console.WriteLine($"Your current title is: {_player_title}");
            Console.WriteLine($"Your current points: {_earned_points}");
        });
        player_menu.DisplayMenu();
    }
    public void AddGoal()
    {
        Console.WriteLine("To make a new goal, please choose a name for it.");
        string tmpname = Console.ReadLine();
        Console.WriteLine("Now, add a description to it. Alternatively, you may leave it blank.");
        string tmpdesc = Console.ReadLine();
        Console.WriteLine("Please choose the type of goal this is.");
        Console.WriteLine("1: Simple Goal");
        Console.WriteLine("2: Eternal Goal");
        Console.WriteLine("3: Repeated Goal");
        string tmpgoaltype = Console.ReadLine();
        try
        {
            switch (tmpgoaltype)
            {
                case "1":
                    Console.WriteLine("Please input a point value for when this item is completed.");
                    _goal_list.Add(new SimpleGoal(tmpname, tmpdesc, int.Parse(Console.ReadLine())));
                    break;
                case "2":
                    Console.WriteLine("Please input a point value for when this item is completed.");
                    _goal_list.Add(new EternalGoal(tmpname, tmpdesc, int.Parse(Console.ReadLine())));
                    break;
                case "3":
                    Console.WriteLine("Please input the amount of desired repetitions.");
                    int tmpNumRepeat = int.Parse(Console.ReadLine());
                    Console.WriteLine("Please input the value per completetion.");
                    int tmpminorval = int.Parse(Console.ReadLine());
                    Console.WriteLine("Please input the value when it is finished.");
                    int tmpmajorval = int.Parse(Console.ReadLine());
                    _goal_list.Add(new ChecklistGoal(tmpname, tmpdesc, tmpmajorval, tmpNumRepeat, tmpminorval));
                    break;
                default:
                    Console.WriteLine("Type not recognized, please Try again.");
                    break;
            }
        }
        catch
        {
            Console.WriteLine("Something went wrong! Please try again.");
        }
    }
}

class Goal : IFileable, IMenuItem
{
    public const string GOAL_SEPORATOR = "^";
    protected string _name = "Unloaded Error";
    protected string _desc = "Unloaded Error";
    protected Boolean _completed = false;
    protected int _reward_on_complete = 0;

    public Goal(string name, string desc, int reward)
    {
        _name = name;
        _desc = desc;
        _reward_on_complete = reward;
    }
    public Goal(string load_data)
    {
        this.Load(load_data);
    }

    public virtual void UpdateGoal()
    {
        if (!_completed)
        {
            Console.WriteLine("You have completed this goal!");
        }
        else
        {
            Console.WriteLine("You have already completed this goal!");
        }
        _completed = true;
    }
    public virtual int GetPointsToAdd()
    {
        return _reward_on_complete;
    }
    public virtual string GetToken()
    {
        return null;
    }
    public virtual void Load(string to_load)
    {
        string[] tmp_data = to_load.Split(GOAL_SEPORATOR);
        _name = tmp_data[0];
        _desc = tmp_data[1];
        _reward_on_complete = int.Parse(tmp_data[2]);
        _completed = Boolean.Parse(tmp_data[3]);
    }

    public virtual string Save()
    {
        return $"{GetToken()}{GOAL_SEPORATOR}{_name}{GOAL_SEPORATOR}{_desc}{GOAL_SEPORATOR}{_reward_on_complete}{GOAL_SEPORATOR}{_completed}";
    }

    public virtual string GetName()
    {
        return $"[{(_completed ? "X" : " ")}]{_name} - {_desc}";
    }

    public void RunOption()
    {
        UpdateGoal();
    }
}

class SimpleGoal : Goal
{
    public const string TOKEN = "simplegoal";
    public SimpleGoal(string name, string desc, int reward) : base(name, desc, reward) { }
    public SimpleGoal(string load_data) : base(load_data) { }
    public override string GetToken()
    {
        return TOKEN;
    }
}
class EternalGoal : Goal
{
    public const string TOKEN = "eternalgoal";
    public EternalGoal(string name, string desc, int reward) : base(name, desc, reward) { }
    public EternalGoal(string load_data) : base(load_data) { }
    public override void UpdateGoal()
    {
        Console.WriteLine("You have made progress on this goal! Keep up the good work!");
    }
    public override string GetToken()
    {
        return TOKEN;
    }
}
class ChecklistGoal : Goal
{
    public const string TOKEN = "checkgoal";
    protected int _times_to_repeat;
    protected int _reward_on_repeat;
    protected int _times_repeated;
    public ChecklistGoal(string name, string desc, int reward, int times_to_repeat, int reward_on_repeat) : base(name, desc, reward)
    {
        _times_to_repeat = times_to_repeat;
        _reward_on_repeat = reward_on_repeat;
        _times_repeated = 0;
    }
    public ChecklistGoal(string load_data) : base(load_data) { }
    public override void UpdateGoal()
    {
        if (_completed)
        {
            Console.WriteLine("You have already completed this goal!");
            return;
        }
        if (_times_to_repeat > _times_repeated)
        {
            Console.WriteLine("You have made progress on this goal! Keep up the good work!");
            _times_repeated++;
            return;
        }
        Console.WriteLine("You have completed this goal!");
    }
    public override string GetName()
    {
        return $"[{(_completed ? "X" : " ")}] [{_times_repeated} / {_times_to_repeat}] {_name} - {_desc}";
    }
    public override string Save()
    {
        return $"{GetToken()}{GOAL_SEPORATOR}{_name}{GOAL_SEPORATOR}{_desc}{GOAL_SEPORATOR}{_reward_on_complete}{GOAL_SEPORATOR}{_times_to_repeat}{GOAL_SEPORATOR}{_reward_on_repeat}{GOAL_SEPORATOR}{_times_repeated}{GOAL_SEPORATOR}{_completed}";
    }
    public override void Load(string to_load)
    {
        string[] tmp_data = to_load.Split(GOAL_SEPORATOR);
        _name = tmp_data[0];
        _desc = tmp_data[1];
        _reward_on_complete = int.Parse(tmp_data[2]);
        _times_to_repeat = int.Parse(tmp_data[3]);
        _reward_on_repeat = int.Parse(tmp_data[4]);
        _times_repeated = int.Parse(tmp_data[5]);
        _completed = Boolean.Parse(tmp_data[6]);
    }
    public override int GetPointsToAdd()
    {
        if (_times_to_repeat > _times_repeated)
        {
            return _reward_on_repeat;
        }
        return _reward_on_complete;
    }
    public override string GetToken()
    {
        return TOKEN;
    }
}