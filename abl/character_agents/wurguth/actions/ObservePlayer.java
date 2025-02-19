package abl.actions;

import org.json.simple.JSONObject;
import server.Message;
import server.TCPServer;

// Wurguth observes the player.
// Args:
//  [0] - int characterId (Wurguth's ID)
public class ObservePlayer extends BaseAction {

    @Override
    public void execute(Object[] args) {
        int characterId = (int) args[0];

        System.out.println("Wurguth (ID " + characterId + ") is observing the player.");

        int code = 2; // Non-combat action
        String msg = "ObservePlayer";
        JSONObject data = new JSONObject();
        data.put("characterId", characterId);
		//Could add more to send here

        Message toSend = new Message(code, msg, data);
        JSONObject jo = toSend.toJSON();
        TCPServer.getInstance().sendOutgoingMessage(jo);

        // In Unity:
        // 1. Receive message.
        // 2. Find Wurguth (using characterId).
        // 3. Initiate an "observation" sequence.  This *might* involve:
        //    - Setting Wurguth's state to "observing".
        //    - Making Wurguth move to a hidden location near the player.
        //    - Playing a "stealth" animation.
        //    - Periodically checking if the player performs any
        //      "suspicious" actions.  If so, update Wurguth's DELPEntity
        //      (e.g., addFact("playerIsSuspicious(wurguth)")).
        //    - If Wurguth is detected, you might transition to a
        //      different behavior (e.g., confront or flee).
    }
}