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
public class ChaserAgent_StepFactories {
   static public Step stepFactory0(int __$stepID, Behavior __$behaviorParent, final Object[] __$behaviorFrame) {
      final Method __$stepFactory = ChaserAgent.__$stepFactory0_rfield;
      switch (__$stepID) {
         case -3: {
            // default wait step
            return new WaitStepDebug(-3, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, null, null, null, null);
         }
         case -2: {
            // default fail step
            return new FailStepDebug(-2, __$stepFactory, __$behaviorParent, false, false, false, (short)-32768, (short)0, false, null, null);
         }
         case -1: {
            // default succeed step
            return new SucceedStepDebug(-1, __$stepFactory, __$behaviorParent, false, false, (short)-32768, (short)0, false, null, null);
         }
         case 0: {
            // manageFiring_1Step1
            return new GoalStepDebug(0, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, ChaserAgent.__$argumentExecute0_rfield, null, null, null, "Wait(int)", null, (short)0);
         }
         case 1: {
            // manageFiring_1Step2
            return new GoalStepDebug(1, __$stepFactory, __$behaviorParent, true, false, false, false, false, false, (short)-32768, (short)0, false, null, null, null, null, null, "fire()", null, (short)0);
         }
         case 2: {
            // fire_1Step1
            return new PrimitiveStepDebug(2, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, ChaserAgent.__$argumentExecute0_rfield, null, null, null, new Fire(), null, "fire");
         }
         case 3: {
            // fire_1Step2
            return new GoalStepDebug(3, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, ChaserAgent.__$argumentExecute0_rfield, null, null, null, "Wait(int)", null, (short)0);
         }
         case 4: {
            // manageMovement_1Step1
            return new GoalStepDebug(4, __$stepFactory, __$behaviorParent, true, false, false, false, false, false, (short)-32768, (short)0, false, null, null, null, null, null, "move()", null, (short)0);
         }
         case 5: {
            // move_1Step1
            return new PrimitiveStepDebug(5, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, ChaserAgent.__$argumentExecute0_rfield, null, null, null, new MoveUp(), null, "moveUp");
         }
         case 6: {
            // move_2Step1
            return new PrimitiveStepDebug(6, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, ChaserAgent.__$argumentExecute0_rfield, null, null, null, new MoveDown(), null, "moveDown");
         }
         case 7: {
            // move_3Step1
            return new PrimitiveStepDebug(7, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, ChaserAgent.__$argumentExecute0_rfield, null, null, null, new MoveLeft(), null, "moveLeft");
         }
         case 8: {
            // move_4Step1
            return new PrimitiveStepDebug(8, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, ChaserAgent.__$argumentExecute0_rfield, null, null, null, new MoveRight(), null, "moveRight");
         }
         case 9: {
            // move_5Step1
            return new PrimitiveStepDebug(9, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, ChaserAgent.__$argumentExecute0_rfield, null, null, null, new Stop(), null, "stop");
         }
         case 10: {
            // Wait_1Step1
            return new MentalStepDebug(10, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, ChaserAgent.__$mentalExecute0_rfield, null, null, null, (byte)2, "Wait_1Step1");
         }
         case 11: {
            // Wait_1Step2
            return new WaitStepDebug(11, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, null, ChaserAgent.__$successTest0_rfield, null, null);
         }
         case 12: {
            // ChaserAgent_RootCollectionBehaviorStep1
            return new MentalStepDebug(12, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)3, (short)0, false, null, ChaserAgent.__$mentalExecute0_rfield, null, null, null, (byte)2, "ChaserAgent_RootCollectionBehaviorStep1");
         }
         case 13: {
            // ChaserAgent_RootCollectionBehaviorStep2
            return new GoalStepDebug(13, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)2, (short)0, false, null, null, null, null, null, "manageFiring()", null, (short)0);
         }
         case 14: {
            // ChaserAgent_RootCollectionBehaviorStep3
            return new GoalStepDebug(14, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)1, (short)0, false, null, null, null, null, null, "manageMovement()", null, (short)0);
         }
      default:
         throw new AblRuntimeError("Unexpected stepID " + __$stepID);
      }
   }
}
