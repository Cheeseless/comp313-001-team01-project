public abstract class Ability {

    // this method should display the appropriate UI once an attack is selected.
    // It should also display the valid target hexes
    public abstract void Select();

    // this method should refresh the UI elements when the ability's target is changed
    public abstract void Refresh();

    //this method should play the relevant animations and perform the calculations of all changes to affected units
    public abstract void Execute();

    //todo: do we need a hide method?

}