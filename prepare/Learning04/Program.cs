using System;

class Program
{
    static void Main(string[] args)
    {
        MathAssignment tmp_m = new MathAssignment("John Jacob", "Math 102", "Section 7.2", "Problems 1-762");
        WritingAssignment tmp_w = new WritingAssignment("Gerald Greedle", "Writing for Greedles", "The Impact of Greedlization and it's Developments");
        Console.WriteLine(tmp_m.GetSummary() + "\n");
        Console.WriteLine(tmp_w.GetSummary());
    }
}


class Assignment
{

    private string _studentName;
    private string _topic;

    public Assignment(string studentName, string topic)
    {
        _studentName = studentName;
        _topic = topic;
    }

    public virtual string GetSummary()
    {
        return $"{_studentName} - {_topic}";
    }
}


class MathAssignment : Assignment
{
    private string _textbookSection;
    private string _problems;

    public MathAssignment(string studentName, string topic, string textbookSection, string problems) : base(studentName, topic)
    {
        _textbookSection = textbookSection;
        _problems = problems;
    }

    public override string GetSummary()
    {
        return $"{base.GetSummary()}\n{_textbookSection}, {_problems}";
    }
}

class WritingAssignment : Assignment
{
    private string _title;
    public WritingAssignment(string studentName, string topic, string title) : base(studentName, topic)
    {
        _title = title;
    }

    public override string GetSummary()
    {
        return $"{base.GetSummary()}\n{_title}";
    }
}