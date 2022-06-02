package abl.sensors;

import abl.runtime.DefaultAsyncSensor;
/**
 * Provides a base class for asynchronous parallel sensors. 
 * 
 * @author Ben Weber 3-7-11
 */
public abstract class SerialSensor extends DefaultAsyncSensor{

	/** 
	 * Sense game state.
	 * 
     * Note: This is invoked for success tests and context conditions.
	 */
    public void senseContinuous(Object[] args) {
        sense(args);
    }
	
    /**
	 * Sense game state.
	 * 
     * Note: This is invoked via senseOneShot, which is used in preconditions.
     */
    protected void sense(Object[] args) {
        sense();        
    }

    /**
     * Enable parallel sensing.
     */
    public boolean canBeParallel() {
    	return true;
    }
    
    /**
     * Updates working memory with game state.
     */
    protected abstract void sense(); 
}
