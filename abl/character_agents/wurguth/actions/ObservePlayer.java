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
    }
}