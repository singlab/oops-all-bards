package abl.generated;

import abl.runtime.*;
import wm.WME;
import wm.WorkingMemorySet;
import wm.WMEIndex;
import wm.TrackedWorkingMemory;
import java.util.*;
import java.lang.reflect.Method;
import java.lang.reflect.Field;
import abl.learning.*;
import abl.wmes.*;
import abl.actions.*;
import abl.sensors.*;
public class GameAgent_MentalStepExecute {
   static public void mentalExecute0(int __$stepID, final Object[] __$behaviorFrame, final BehavingEntity __$thisEntity, MentalStep __$thisStep) {
      switch (__$stepID) {
         case 3: {
            // lookForAllyAgent_1Step4
            ((AllyWME)__$behaviorFrame[0]).setOnTree(true);
            break;
         }
         case 4: {
            // GameAgent_RootCollectionBehaviorStep1
            System.out.println("Starting game agent...");
            break;
         }
      default:
         throw new AblRuntimeError("Unexpected stepID " + __$stepID);
      }
   }
}
