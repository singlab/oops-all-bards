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
public class GameAgent_BehaviorFactories {
   static public Behavior behaviorFactory0(int __$behaviorID, Object[] __$args, Map __$boundVars, GoalStep __$parentGoal, String __$signature, BehavingEntity __$thisEntity, __BehaviorDesc __$behaviorDesc) {
      switch (__$behaviorID) {
         case 0: {
            // lookForAllyAgent_1
               final Object[] __$behaviorFrame = new Object[2];

            final __StepDesc[] __$steps = {new __StepDesc(0, GameAgent.__$stepFactory0_rfield), new __StepDesc(1, GameAgent.__$stepFactory0_rfield), new __StepDesc(2, GameAgent.__$stepFactory0_rfield), new __StepDesc(3, GameAgent.__$stepFactory0_rfield)};
            return new SequentialBehaviorDebug(__$parentGoal, null, null, null, null, false, __$signature, (short)0, 0, __$behaviorFrame, __$steps, __$behaviorDesc);
         }
         case 1: {
            // createAllyAgent_1
               final Object[] __$behaviorFrame = new Object[1];
               __$behaviorFrame[0] = (AllyWME)__$args[0];

            final __StepDesc[] __$steps = null;
            return new SequentialBehaviorDebug(__$parentGoal, null, null, null, null, false, __$signature, (short)0, 1, __$behaviorFrame, __$steps, __$behaviorDesc);
         }
         case 2: {
            // manageAllyAgent_1

            final __StepDesc[] __$steps = null;
            return new SequentialBehaviorDebug(__$parentGoal, null, null, null, null, false, __$signature, (short)0, 2, null, __$steps, __$behaviorDesc);
         }
         case 3: {
            // GameAgent_RootCollectionBehavior

            final __StepDesc[] __$steps = {new __StepDesc(4, GameAgent.__$stepFactory0_rfield), new __StepDesc(5, GameAgent.__$stepFactory0_rfield), new __StepDesc(6, GameAgent.__$stepFactory0_rfield)};
            return new CollectionBehaviorDebug(__$parentGoal, null, null, null, null, false, __$signature, (short)0, 3, null, __$steps, 3, __$behaviorDesc);
         }
         case 4: {
            // __$defaultMemoryExecuteBehavior_1
               final Object[] __$behaviorFrame = __$args;

            final __StepDesc[] __$steps = null;
            return new MemoryExecuteBehaviorDebug(__$parentGoal, null, null, null, null, false, __$signature, (short)0, 4, __$behaviorFrame, __$steps, 0, __$behaviorDesc);
         }
      default:
         throw new AblRuntimeError("Unexpected behaviorID " + __$behaviorID);
      }
   }
}
