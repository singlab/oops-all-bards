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
public class ChaserAgent_BehaviorFactories {
   static public Behavior behaviorFactory0(int __$behaviorID, Object[] __$args, Map __$boundVars, GoalStep __$parentGoal, String __$signature, BehavingEntity __$thisEntity, __BehaviorDesc __$behaviorDesc) {
      switch (__$behaviorID) {
         case 0: {
            // manageFiring_1

            final __StepDesc[] __$steps = {new __StepDesc(0, ChaserAgent.__$stepFactory0_rfield), new __StepDesc(1, ChaserAgent.__$stepFactory0_rfield)};
            return new SequentialBehaviorDebug(__$parentGoal, null, null, null, null, false, __$signature, (short)0, 0, null, __$steps, __$behaviorDesc);
         }
         case 1: {
            // fire_1
               final Object[] __$behaviorFrame = new Object[4];
               __$behaviorFrame[0] = new __ValueTypes.IntVar((Integer)__$boundVars.get("playerX"));
               __$behaviorFrame[1] = new __ValueTypes.IntVar((Integer)__$boundVars.get("playerY"));
               __$behaviorFrame[2] = new __ValueTypes.IntVar((Integer)__$boundVars.get("botY"));
               __$behaviorFrame[3] = new __ValueTypes.IntVar((Integer)__$boundVars.get("botX"));

            final __StepDesc[] __$steps = {new __StepDesc(2, ChaserAgent.__$stepFactory0_rfield), new __StepDesc(3, ChaserAgent.__$stepFactory0_rfield)};
            return new SequentialBehaviorDebug(__$parentGoal, null, null, null, null, false, __$signature, (short)0, 1, __$behaviorFrame, __$steps, __$behaviorDesc);
         }
         case 2: {
            // manageMovement_1

            final __StepDesc[] __$steps = {new __StepDesc(4, ChaserAgent.__$stepFactory0_rfield)};
            return new SequentialBehaviorDebug(__$parentGoal, null, null, null, null, false, __$signature, (short)0, 2, null, __$steps, __$behaviorDesc);
         }
         case 3: {
            // move_1
               final Object[] __$behaviorFrame = new Object[3];
               __$behaviorFrame[0] = new __ValueTypes.IntVar((Integer)__$boundVars.get("playerY"));
               __$behaviorFrame[1] = new __ValueTypes.IntVar((Integer)__$boundVars.get("id"));
               __$behaviorFrame[2] = new __ValueTypes.IntVar((Integer)__$boundVars.get("botY"));

            final __StepDesc[] __$steps = {new __StepDesc(5, ChaserAgent.__$stepFactory0_rfield)};
            return new SequentialBehaviorDebug(__$parentGoal, null, null, null, null, false, __$signature, (short)3, 3, __$behaviorFrame, __$steps, __$behaviorDesc);
         }
         case 4: {
            // move_2
               final Object[] __$behaviorFrame = new Object[3];
               __$behaviorFrame[0] = new __ValueTypes.IntVar((Integer)__$boundVars.get("playerY"));
               __$behaviorFrame[1] = new __ValueTypes.IntVar((Integer)__$boundVars.get("id"));
               __$behaviorFrame[2] = new __ValueTypes.IntVar((Integer)__$boundVars.get("botY"));

            final __StepDesc[] __$steps = {new __StepDesc(6, ChaserAgent.__$stepFactory0_rfield)};
            return new SequentialBehaviorDebug(__$parentGoal, null, null, null, null, false, __$signature, (short)3, 4, __$behaviorFrame, __$steps, __$behaviorDesc);
         }
         case 5: {
            // move_3
               final Object[] __$behaviorFrame = new Object[3];
               __$behaviorFrame[0] = new __ValueTypes.IntVar((Integer)__$boundVars.get("playerX"));
               __$behaviorFrame[1] = new __ValueTypes.IntVar((Integer)__$boundVars.get("id"));
               __$behaviorFrame[2] = new __ValueTypes.IntVar((Integer)__$boundVars.get("botX"));

            final __StepDesc[] __$steps = {new __StepDesc(7, ChaserAgent.__$stepFactory0_rfield)};
            return new SequentialBehaviorDebug(__$parentGoal, null, null, null, null, false, __$signature, (short)2, 5, __$behaviorFrame, __$steps, __$behaviorDesc);
         }
         case 6: {
            // move_4
               final Object[] __$behaviorFrame = new Object[3];
               __$behaviorFrame[0] = new __ValueTypes.IntVar((Integer)__$boundVars.get("playerX"));
               __$behaviorFrame[1] = new __ValueTypes.IntVar((Integer)__$boundVars.get("id"));
               __$behaviorFrame[2] = new __ValueTypes.IntVar((Integer)__$boundVars.get("botX"));

            final __StepDesc[] __$steps = {new __StepDesc(8, ChaserAgent.__$stepFactory0_rfield)};
            return new SequentialBehaviorDebug(__$parentGoal, null, null, null, null, false, __$signature, (short)2, 6, __$behaviorFrame, __$steps, __$behaviorDesc);
         }
         case 7: {
            // move_5
               final Object[] __$behaviorFrame = new Object[1];
               __$behaviorFrame[0] = new __ValueTypes.IntVar((Integer)__$boundVars.get("id"));

            final __StepDesc[] __$steps = {new __StepDesc(9, ChaserAgent.__$stepFactory0_rfield)};
            return new SequentialBehaviorDebug(__$parentGoal, null, null, null, null, false, __$signature, (short)1, 7, __$behaviorFrame, __$steps, __$behaviorDesc);
         }
         case 8: {
            // Wait_1
               final Object[] __$behaviorFrame = new Object[2];
               __$behaviorFrame[0] = new __ValueTypes.IntVar((Integer)__$args[0]);
               __$behaviorFrame[1] = new __ValueTypes.LongVar();

            final __StepDesc[] __$steps = {new __StepDesc(10, ChaserAgent.__$stepFactory0_rfield), new __StepDesc(11, ChaserAgent.__$stepFactory0_rfield)};
            return new SequentialBehaviorDebug(__$parentGoal, null, null, null, null, false, __$signature, (short)0, 8, __$behaviorFrame, __$steps, __$behaviorDesc);
         }
         case 9: {
            // ChaserAgent_RootCollectionBehavior

            final __StepDesc[] __$steps = {new __StepDesc(12, ChaserAgent.__$stepFactory0_rfield), new __StepDesc(13, ChaserAgent.__$stepFactory0_rfield), new __StepDesc(14, ChaserAgent.__$stepFactory0_rfield)};
            return new CollectionBehaviorDebug(__$parentGoal, null, null, null, null, false, __$signature, (short)0, 9, null, __$steps, 3, __$behaviorDesc);
         }
         case 10: {
            // __$defaultMemoryExecuteBehavior_1
               final Object[] __$behaviorFrame = __$args;

            final __StepDesc[] __$steps = null;
            return new MemoryExecuteBehaviorDebug(__$parentGoal, null, null, null, null, false, __$signature, (short)0, 10, __$behaviorFrame, __$steps, 0, __$behaviorDesc);
         }
      default:
         throw new AblRuntimeError("Unexpected behaviorID " + __$behaviorID);
      }
   }
}
