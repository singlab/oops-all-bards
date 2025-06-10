package abl.wmes;

import abl.util.*;
import java.util.*;
import org.json.simple.JSONArray;
import org.json.simple.JSONObject;
import com.google.gson.Gson;
import wm.WME;

public class VivWME extends WME {
    /** Whether or not this WME is on an active behavior tree */
    private boolean onTree;
    /** ID of the acting character */
    private int id;
    /** An array of strings representing behaviors to be spawned */
    private SpawnGoalData[] toSpawn;
    /** An array of strings representing behaviors to be stopped */
    private String[] toStop;

    public VivWME(JSONObject data) {
        this.onTree = false;
        this.id = (int) (long) data.get("id");
        Gson gson = new Gson();
        JSONArray spawnArray = (JSONArray) data.get("toSpawn");
        System.out.println("[VivWME DEBUG] Received toSpawn JSON: " + spawnArray.toJSONString());
        this.toSpawn = gson.fromJson(spawnArray.toJSONString(), SpawnGoalData[].class);
        if (this.toSpawn != null) {
            System.out.println("[VivWME DEBUG] Successfully parsed " + this.toSpawn.length + " goal(s). Contents:");
            for (int i = 0; i < this.toSpawn.length; i++) {
                // Because SpawnGoalData now has a toString(), this will be very informative.
                System.out.println("  - Goal[" + i + "]: " + this.toSpawn[i]);
            }
        } else {
            System.out.println("[VivWME DEBUG] Parsing resulted in a NULL toSpawn array.");
        }
        JSONArray jsonArray = (JSONArray) data.get("toStop");
        this.toStop = new String[jsonArray.size()];
        for (int i = 0; i < jsonArray.size(); i++) {
            this.toStop[i] = (String) jsonArray.get(i);
        }
    }

    public String toString() {
        StringBuilder builder = new StringBuilder();
        builder.append("VivWME: \n")
                .append("ID: " + this.id + "\n")
                .append("ToSpawn: " + Arrays.toString(this.toSpawn) + "\n")
                .append("ToStop: " + Arrays.toString(this.toStop));
        String result = builder.toString();
        return result;
    }

    public boolean hasGoal(String goalName) {
        System.out.println(String.format("[VivWME DEBUG] ABL is checking hasGoal('%s')...", goalName));
        if (toSpawn == null)
            return false;
        for (SpawnGoalData goal : toSpawn) {
            // Check for null name property, which could be the source of the error
            if (goal != null && goal.name != null && goal.name.equalsIgnoreCase(goalName)) {
                System.out.println(String.format("[VivWME DEBUG] ...hasGoal('%s') returning TRUE.", goalName));
                return true;
            }
        }
        System.out.println(String.format("[VivWME DEBUG] ...hasGoal('%s') returning FALSE.", goalName));
        return false;
    }

    public int getTargetIdForGoal(String goalName) {
        if (toSpawn == null)
            return -1;
        for (SpawnGoalData goal : toSpawn) {
            if (goal.name.equalsIgnoreCase(goalName)) {
                return goal.targetCharacter;
            }
        }
        return -1;
    }

    public boolean getOnTree() {
        return onTree;
    }

    public void setOnTree(boolean onTree) {
        this.onTree = onTree;
    }

    public int getID() {
        return id;
    }

    public SpawnGoalData[] getToSpawn() {
        return toSpawn;
    }

    public String[] getToStop() {
        return toStop;
    }
}
