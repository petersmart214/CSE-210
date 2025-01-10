


using System.Dynamic;

class Fraction {

    private int topInt;
    private int bottomInt;

    public Fraction() {
        this.topInt = 1;
        this.bottomInt = 1;
    }
    public Fraction(int topInt) {
        this.topInt = topInt;
        this.bottomInt = 1;
    }
    public Fraction(int topInt, int bottomInt) {
        this.topInt = topInt;
        this.bottomInt = bottomInt;
    }

    public string getFractionString() {
        return $"{topInt}/{bottomInt}";
    }
    public double getDecimalValue() {
        return (double) topInt / (double) bottomInt;
    }

    public int getTopInt() {
        return topInt;
    }
    public int getBottomInt() {
        return bottomInt;
    }
    public void setTopInt(int newTopInt) {
        this.topInt = newTopInt;
    }
    public void setBottomInt(int newBottomInt) {
        this.bottomInt = newBottomInt;
    }
}


