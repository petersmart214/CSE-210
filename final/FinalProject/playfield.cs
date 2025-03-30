class Playfield {

    List<Mind> mindsInPlay = new List<Mind>();
    List<Atom> inPlay = new List<Atom>();


    public void registerAtom(Atom atom) {
        if (atom.HasMind()) mindsInPlay.Add(atom.getMind());
        inPlay.Add(atom);
    } //if performance issues (unlikely) then make one that does not check for a mind

    public void deregisterAtom(Atom atom) {
        if (atom.HasMind()) mindsInPlay.Remove(atom.getMind());
        inPlay.Remove(atom);
    }

    public void displayByMind(Mind mind) {
        //TODO: add displaying the playfield.
    }
}