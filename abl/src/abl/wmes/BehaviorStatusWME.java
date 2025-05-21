package abl.wmes;

import org.json.simple.JSONObject;
import wm.WME;

public class BehaviorStatusWME extends WME {
    /** Whether or not this WME is on an active behavior tree */
    private boolean onTree = false;

    /** ID of the character this status pertains to */
    private int characterID;

    /** Name of the behavior that completed or was terminated */
    private String behaviorName;

    /** Status of the behavior (e.g., "Success", "Failure", "Interrupted") */
    private String status;

    /**
     * Constructor that populates the WME fields from a JSONObject.
     * This JSONObject is expected to be parsed from the JSON data
     * sent by the Unity C# BehaviorStatusWME.
     *
     * @param data The JSONObject containing the WME data.
     *             Expected keys: "characterID", "behaviorName", "status".
     */
    public BehaviorStatusWME(JSONObject data) {
        this.onTree = false;
        this.characterID = (int) (long) data.get("characterID");
        this.behaviorName = (String) data.get("behaviorName");
        this.status = (String) data.get("status");
    }

    /**
     * Provides a string representation of this WME, useful for logging and
     * debugging.
     *
     * @return A string detailing the WME's fields.
     */
    @Override
    public String toString() {
        StringBuilder builder = new StringBuilder();
        builder.append("BehaviorStatusWME: \n")
                .append("  CharacterID: ").append(this.characterID).append("\n")
                .append("  BehaviorName: ").append(this.behaviorName).append("\n")
                .append("  Status: ").append(this.status);
        return builder.toString();
    }

    // Getter methods for the WME's fields

    public int getCharacterID() {
        return characterID;
    }

    public String getBehaviorName() {
        return behaviorName;
    }

    public String getStatus() {
        return status;
    }

    public boolean isOnTree() {
        return onTree;
    }
}
