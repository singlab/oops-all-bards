package abl.actions;

import org.json.simple.JSONObject;
import server.Message;
import server.TCPServer;

// Wurguth questions an NPC.
// Args:
//  [0] - int characterId (Wurguth's ID)
//  [1] - String currentNpc (The name of the NPC to question)
public class QuestionLocals extends BaseAction {

    @Override
    public void execute(Object[] args) {
        int characterId = (int) args[0];
        String currentNpc = (String) args[1];

        System.out.println("Wurguth (ID " + characterId + ") is questioning NPC: " + currentNpc);

        // 1 -- combat action, 2 -- noncombat action
        int code = 2; // Assuming questioning is a non-combat action
        String msg = "QuestionLocals"; // Class name
        JSONObject data = new JSONObject();
        data.put("characterId", characterId);
        data.put("npcName", currentNpc);
        //Could add more to send here.

        Message toSend = new Message(code, msg, data);
        JSONObject jo = toSend.toJSON();
        TCPServer.getInstance().sendOutgoingMessage(jo);
    }
}
