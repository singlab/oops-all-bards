package abl.sensors;

import game.GameEngine; 
import abl.runtime.BehavingEntity;
import abl.wmes.PlayerWME;
/**
 * Adds a PlayerWME object to working memory when sense in invoked.
 * 
 * @author Ben Weber 3-7-11
 */
public class PlayerSensor extends SerialSensor {

	/**
	 * Adds a Player WME to working memory of the agent and deletes previous player WMEs in memory.
	 */
	protected void sense() {

		BehavingEntity.getBehavingEntity().deleteAllWMEClass("PlayerWME");
		BehavingEntity.getBehavingEntity().addWME(
				new PlayerWME(GameEngine.getInstance().getPlayerLocation(), GameEngine.getInstance().getPlayerTrajectory()));
	}
}
