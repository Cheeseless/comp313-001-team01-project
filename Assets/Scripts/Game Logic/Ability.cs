public interface IAbility {

    // this method should display the appropriate UI once an attack is selected.
    // It should also display the valid target hexes
    void Select(GameUnit source);

    // this method should refresh the UI elements when the ability's target is changed
    void Refresh(GameUnit source);

    //this method should play the relevant animations and perform the calculations of all changes to affected units
    void Execute(GameUnit source);

    //todo: do we need a hide method?

}