package game.input;

import java.awt.event.KeyEvent;
import game.GameEngine;

public class Exit implements IInput {
	
	public int triggerKeyCode = KeyEvent.VK_ESCAPE;
	
	public Exit() {
	}

	@Override
	public void process(GameEngine gameEngine, int keyCode) {
		if(keyCode == this.triggerKeyCode) {
			System.exit(0);
		}
	}
}
