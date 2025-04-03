class Playfield {

    List<Mind> _minds_in_play = new List<Mind>();
    List<Atom> _in_play = new List<Atom>();


    public void RegisterAtom(Atom atom) {
        if (atom.HasMind()) _minds_in_play.Add(atom.GetMind());
        _in_play.Add(atom);
    } //if performance issues (unlikely) then make one that does not check for a mind

    public void DeregisterAtom(Atom atom) {
        if (atom.HasMind()) _minds_in_play.Remove(atom.GetMind());
        _in_play.Remove(atom);
    }

    public void DisplayByMind(Mind mind) {
        //TODO: add displaying the playfield.
    }
    /// <summary>
    /// This [will] use the atom provided as an origin of what to enumerate. [Here to allow future optimizations!]
    /// </summary>
    /// <param name="loc"></param>
    public void EnumerateByLoc(Loc loc, Action<Atom> en_action) {
        _in_play.ForEach(en_action);
    }
    public Atom AtomAtLoc(Loc loc) {
        Atom to_return = null;
        EnumerateByLoc(loc, a=>{if(a.GetLoc().Equals(loc)) to_return = a;});
        return to_return;
    }

    public List<Atom> GetInPlay() {
        return _in_play;
    }
}