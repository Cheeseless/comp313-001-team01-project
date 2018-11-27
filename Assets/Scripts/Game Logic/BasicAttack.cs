internal class BasicAttack : IAbility {

    public void Select(GameUnit source) {
        var defender = source.Target;
    }

    public void Refresh(GameUnit source) {
    }

    public void Execute(GameUnit source) {
    }

}