using System.Text.RegularExpressions;

class Program
{
    static int chosenVerse = 0;
    static ScriptureWrapper[] versesList = [new ScriptureWrapper(new Scripture([["5.", "Trust in the Lord with all thine heart; and lean not unto thine own understanding."],["6.", "In all thy ways acknowledge him, and he shall direct thy paths."]]), new ScriptureReference("Proverbs 3:5-6")),
    new ScriptureWrapper(new Scripture([["16.", "For God so loved the world, that he gave his only begotten Son, that whosoever believeth in him should not perish, but have everlasting life."]]), new ScriptureReference("John 3:16"))];
    static void Main(string[] args)
    {
       chosenVerse = new Random().Next(0, versesList.Length);
        while(true) {
            Console.Clear();
            Console.WriteLine(versesList[chosenVerse].getReference().getReference());
            foreach(string[] i in versesList[chosenVerse].getScripture().getVerse()) {
                Console.WriteLine($"{i[0]} {i[1]}");
            }

            Console.WriteLine("Enter nothing to remove a word from each verse, or quit to exit the program.");
            if(Console.ReadLine().Equals("quit")) break;
            if(versesList[chosenVerse].getReduced(1)) break;
        }

    }
}


class Scripture {
    private string[][] verse;

    public Scripture(string[][] verse) {
        this.verse = verse;
    }

    public string[][] getVerse() {
        return this.verse;
    }
    public void setVerse(string[][] newVerse) {
        this.verse = newVerse;
    }

}


class ScriptureReference {

    private string reference;

    public ScriptureReference(string reference) {
        this.reference = reference;
    }
    public string getReference() {
        return reference;
    }
}

class ScriptureWrapper {
    Scripture scrip;
    ScriptureReference scripref;

    public ScriptureWrapper(Scripture scrip, ScriptureReference scripref) {
        this.scrip = scrip;
        this.scripref = scripref;
    }

    public Scripture getScripture() {
        return this.scrip;
    }

    public ScriptureReference getReference() {
        return this.scripref;
    }

    public Boolean getReduced(int amountToCut) {
        //This is horrible, disgusting, and awful. From de-incrementing a loop, to having to check a bool mid-method to ensure it doesnt loop infinitely, to checking every word with a regex function that presumably checks ever char... buuuuuuut it works
        //oops, needs the verses to be the same length for it to not get stuck, Ill fix it later
        Boolean hasLeft = true;
        Random rand = new Random();
        string[][] verses = scrip.getVerse();
        for(int iter = 0; iter < verses.Length; iter ++) { 
            string[] splitScrip = new Regex(" ").Split(verses[iter][1]);
            Regex finishCheck = new Regex("_");
            foreach(string checkStr in splitScrip) {
                if(!finishCheck.IsMatch(checkStr)) hasLeft = false;
            }
            if (hasLeft) continue;
            for(int ii = 0; ii < amountToCut; ii++) {
                int cutIndex = rand.Next(0, splitScrip.Length);
                if(finishCheck.IsMatch(splitScrip[cutIndex])) {
                    ii--;
                    continue;
                }
                hasLeft = false;
                splitScrip[cutIndex] = new Regex(".").Replace(splitScrip[cutIndex], "_");
            }
            verses[iter][1] = string.Join(' ', splitScrip);
        }
        scrip.setVerse(verses);
        return hasLeft;
    }

}