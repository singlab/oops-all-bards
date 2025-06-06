package abl.wmes;

import wm.WME;

/**
 * A WME that creates a link between a character's ID and the runtime ID
 * of the ABL behavior that is managing them.
 */
public class CharacterManagerWME extends WME {

    private int characterId;
    public int managerBehaviorId;

    public CharacterManagerWME(int characterId, int managerBehaviorId) {
        this.characterId = characterId;
        this.managerBehaviorId = managerBehaviorId;
    }

    public int getCharacterId() {
        return characterId;
    }

    public int getManagerBehaviorId() {
        return managerBehaviorId;
    }
}
