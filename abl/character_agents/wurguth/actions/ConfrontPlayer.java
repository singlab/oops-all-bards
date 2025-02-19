package abl.actions;

import org.json.simple.JSONObject;
import server.Message;
import server.TCPServer;

// Wurguth confronts the player.
// Args:
//  [0] - int characterId (Wurguth's ID)
public class ConfrontPlayer extends BaseAction {

    @Override
    public void execute(Object[] args) {
        int characterId = (int) args[0];

        System.out.println("Wurguth (ID " + characterId + ") is confronting the player.");

        int code = 2; // Could be combat or non-combat, depending on how you handle it.
        String msg = "ConfrontPlayer";
        JSONObject data = new JSONObject();
        data.put("characterId", characterId);
		//Could add more to send here.

        Message toSend = new Message(code, msg, data);
        JSONObject jo = toSend.toJSON();
        TCPServer.getInstance().sendOutgoingMessage(jo);

        // In Unity:
        // 1. Receive message.
        // 2. Find Wurguth.
        // 3. Initiate a confrontation sequence. This would likely involve:
        //    - Making Wurguth move to the player.
        //    - Playing a "confrontation" animation (e.g., aggressive pose).
        //    - Starting a dialogue with the player.  This dialogue might
        //      be a demand for information, a threat, or an accusation.
        //    - The outcome of the dialogue (or the player's actions)
        //      could lead to combat (transitioning to the NeutralizeThreat
        //      supertask) or to Wurguth backing down (updating his
        //      DELPEntity with information learned from the confrontation).
    }
}
