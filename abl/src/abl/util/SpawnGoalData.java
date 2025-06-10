package abl.util;

public class SpawnGoalData {
    public String name;
    public int actingCharacter;
    public int targetCharacter;

    @Override
    public String toString() {
        return String.format(
                "SpawnGoalData(name: '%s', actor: %d, target: %d)",
                name,
                actingCharacter,
                targetCharacter);
    }
}
