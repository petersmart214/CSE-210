using System.Diagnostics;
using System.Text.RegularExpressions;

class Program {
    public const string MASK_CHAR = "_";
    static void Main(string[] args) {
        List<Verse> tmp_verses = new List<Verse>();
        tmp_verses.Add(new Verse("Trust in the Lord with all thine heart; and lean not unto thine own understanding.", new ScriptureReference(3, 5, "Proverbs")));
        tmp_verses.Add(new Verse("In all thy ways acknowledge him, and he shall direct thy paths.", new ScriptureReference(3, 6, "Proverbs")));
        Scripture tmp = new Scripture(tmp_verses);
        Boolean cont = true;
        while (cont) {
            cont = Process(tmp);
        }
    }

    static Boolean Process(Scripture open_scrip) {
        Console.Clear();
        open_scrip.Display();
        Console.WriteLine("\n\n");
        Console.WriteLine("Press Enter to hide words, type b to revert a step, or quit to end the program");
        string tmp_out = Console.ReadLine();
        switch (tmp_out) {
            case "": {
                if(!open_scrip.HideWords()) return false;
                break;
            }
            case "b": {
                open_scrip.UnhideWords();
                break;
            }
            case "quit": {
                return false;
            }
            default: {
                Console.WriteLine("Unknown command!");
                Console.ReadLine();
                break;
            }

            }
        return true;
    }
}
class Word {
    private string _word;
    private Boolean _hidden; 

    public Word(string word) {
        _word = word;
        _hidden = false;
    }

    public void SetHidden() {
        _hidden = true;
    }

    public void SetUnhidden() {
        _hidden = false;
    }

    public Boolean GetHidable() {
        //gets if this word can be hidden (normally just if its not hidden currently)
        return !_hidden;
    }

    public string GetWordOrMask() {
        return !_hidden ? _word : GetMask(_word, Program.MASK_CHAR);
    }

    private string GetMask(string to_mask, string mask) {
        return new Regex("\\S").Replace(to_mask, mask);
    }
}

class Verse : IDisplayable {
    private List<Word> _words;
    private List<Word> _hidden_order;
    private ScriptureReference _reference;

    public Verse (string unformatted_words, ScriptureReference reference) {
        _words = new List<Word>();
        _hidden_order = new List<Word>();
        foreach (string word in unformatted_words.Split(" ")) {
            _words.Add(new Word(word));
        }
        _reference = reference;
    }

    public ScriptureReference GetScriptureReference() {
        return _reference;
    }

    public Boolean HideWord() {
        List<Word> unhidden_words = new List<Word>();
        foreach (Word word in _words) {
            if (word.GetHidable()) unhidden_words.Add(word);
        }
        if (unhidden_words.Count <= 0) return false;
        Random tmp_rand = new Random();
        Word tmp_word = unhidden_words.ElementAt(tmp_rand.Next(unhidden_words.Count));
        tmp_word.SetHidden();
        _hidden_order.Add(tmp_word);
        return true;
    }

    public Boolean UnhideWord() {
        if (_hidden_order.Count <= 0) return false; 
        _hidden_order.Last().SetUnhidden();
        _hidden_order.RemoveAt(_hidden_order.Count - 1);
        return true;
    }

    public void Display()
    {
        _reference.Display();
        List<string> tmp_body = new List<string>();
        foreach (Word word in _words) {
            tmp_body.Add(word.GetWordOrMask());
        }
        Console.Write(string.Join(" ", tmp_body.ToArray()));
    }
}

class ScriptureReference : IDisplayable {
    private int _chapter;
    private int _verse;
    private string _book;

    public ScriptureReference(int chapter, int verse, string book) {
        _chapter = chapter;
        _verse = verse;
        _book = book;
    }

    public int GetVerse() {
        return _verse;
    }

    public string GetPrefix() {
        return $"{_book} {_chapter}: ";
    }

    public void Display()
    {
        Console.Write($"\n{_verse}: ");
    }
}

class Scripture : IDisplayable, ILoadable {
    private List<Verse> _verses;
    private string _formated_ref;

    public Scripture(List<Verse> verses) {
        _verses = verses;
        _formated_ref = GetFormatedRefs();
    }

    /// <summary>
    /// Returns the prettied scripture references for this whole group. THIS MUST HAVE ALL REFS IN ORDER AND HAVE NO DUPLICATES to work
    /// </summary>
    /// <returns></returns>
    public string GetFormatedRefs() {
        //Assumes all verses are from the same chapter and book
        List<int> tmp = new List<int>();
        string tmp_prefix = "Unknown";


        foreach (Verse verse in _verses) {
            ScriptureReference tmp_ref = verse.GetScriptureReference();
            tmp.Add(tmp_ref.GetVerse());
            tmp_prefix = tmp_ref.GetPrefix();
        }
        string tmp_suffix = "";
        int largest_buffer = -1;
        int smallest_buffer = -1;
        for (int i = 0; i < tmp.Count; i++) {
            if(i + 1 < tmp.Count) {
                if(tmp.ElementAt(i) + 1 == tmp.ElementAt(i + 1) || tmp.ElementAt(i) - 1 == tmp.ElementAt(i + 1)) {
                    if(tmp.ElementAt(i) > largest_buffer || largest_buffer == -1) largest_buffer = tmp.ElementAt(i);
                    if(tmp.ElementAt(i) < smallest_buffer || smallest_buffer == -1) smallest_buffer = tmp.ElementAt(i);
                } else if (largest_buffer != smallest_buffer) {
                    if(tmp.ElementAt(i) > largest_buffer || largest_buffer == -1) largest_buffer = tmp.ElementAt(i);
                    if(tmp.ElementAt(i) < smallest_buffer || smallest_buffer == -1) smallest_buffer = tmp.ElementAt(i);
                    tmp_suffix += $"{smallest_buffer}-{largest_buffer}, ";
                    largest_buffer = -1;
                    smallest_buffer = -1;
                } else {
                    tmp_suffix += $"{tmp.ElementAt(i)}, ";
                }
            } else if (largest_buffer != tmp.ElementAt(i) || tmp.ElementAt(i) != smallest_buffer) {
                if(tmp.ElementAt(i) > largest_buffer || largest_buffer == -1) largest_buffer = tmp.ElementAt(i);
                if(tmp.ElementAt(i) < smallest_buffer || smallest_buffer == -1) smallest_buffer = tmp.ElementAt(i);
                tmp_suffix += $"{smallest_buffer}-{largest_buffer}, ";
                largest_buffer = -1;
                smallest_buffer = -1;
            } else {
                tmp_suffix += $"{tmp.ElementAt(i)}, ";
            }
        }
        Regex trimmer = new Regex(", $");
        return trimmer.Replace($"{tmp_prefix}{tmp_suffix}", "");
    }

    public Boolean HideWords() {
        Boolean tmp_bool = false;
        foreach(Verse verse in _verses) {
            tmp_bool = verse.HideWord() | tmp_bool;
        }
        return tmp_bool;
    }
    public void UnhideWords() {
        foreach(Verse verse in _verses) {
            verse.UnhideWord();
        }
    }
    public void Display()
    {
        Console.WriteLine($"    {GetFormatedRefs()}");
        foreach (Verse verse in _verses) {
            verse.Display();
        }
    }

    public void LoadFromFile(string file_name)
    {
        throw new NotImplementedException();
    }
}