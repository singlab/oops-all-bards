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
public class GameAgent_StepFactories {
   static public Step stepFactory0(int __$stepID, Behavior __$behaviorParent, final Object[] __$behaviorFrame) {
      final Method __$stepFactory = GameAgent.__$stepFactory0_rfield;
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
            // lookForAllyAgent_1Step1
            return new MentalStepDebug(0, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, GameAgent.__$mentalExecute0_rfield, null, null, null, (byte)2, "lookForAllyAgent_1Step1");
         }
         case 1: {
            // lookForAllyAgent_1Step2
            return new WaitStepDebug(1, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, null, GameAgent.__$successTest0_rfield, null, null);
         }
         case 2: {
            // lookForAllyAgent_1Step3
            return new GoalStepDebug(2, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, GameAgent.__$argumentExecute0_rfield, null, null, null, "manageAllyAgent(AllyWME)", null, (short)2);
         }
         case 3: {
            // lookForAllyAgent_1Step4
            return new MentalStepDebug(3, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, GameAgent.__$mentalExecute0_rfield, null, null, null, (byte)2, "lookForAllyAgent_1Step4");
         }
         case 4: {
            // allyAgentRoot_1Step1
            return new WaitStepDebug(4, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, null, GameAgent.__$successTest0_rfield, null, null);
         }
         case 5: {
            // manageAllyAgent_1Step1
            return new GoalStepDebug(5, __$stepFactory, __$behaviorParent, true, false, false, false, false, false, (short)-32768, (short)0, false, null, GameAgent.__$argumentExecute0_rfield, null, null, null, "manageCombat(AllyWME)", null, (short)0);
         }
         case 6: {
            // manageCombat_1Step1
            return new WaitStepDebug(6, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, null, GameAgent.__$successTest0_rfield, null, null);
         }
         case 7: {
            // manageCombat_1Step2
            return new GoalStepDebug(7, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, GameAgent.__$argumentExecute0_rfield, null, null, null, "lookToAssist(AllyWME)", null, (short)0);
         }
         case 8: {
            // manageCombat_1Step3
            return new GoalStepDebug(8, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, GameAgent.__$argumentExecute0_rfield, null, null, null, "waitForTurn(AllyWME)", null, (short)0);
         }
         case 9: {
            // waitForTurn_1Step1
            return new MentalStepDebug(9, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)3, (short)0, false, null, GameAgent.__$mentalExecute0_rfield, null, null, null, (byte)2, "waitForTurn_1Step1");
         }
         case 10: {
            // waitForTurn_1Step2
            return new WaitStepDebug(10, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, null, GameAgent.__$successTest0_rfield, null, null);
         }
         case 11: {
            // waitForTurn_1Step3
            return new GoalStepDebug(11, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, GameAgent.__$argumentExecute0_rfield, null, null, null, "waitForTurn-4->AnonymousStep11()", null, (short)0);
         }
         case 12: {
            // waitForTurn-4->AnonymousStep11_1Step1
            return new GoalStepDebug(12, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, GameAgent.__$argumentExecute0_rfield, null, null, null, "takeTurn(AllyWME)", null, (short)0);
         }
         case 13: {
            // lookToAssist_1Step1
            return new MentalStepDebug(13, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)3, (short)0, false, null, GameAgent.__$mentalExecute0_rfield, null, null, null, (byte)2, "lookToAssist_1Step1");
         }
         case 14: {
            // lookToAssist_1Step2
            return new WaitStepDebug(14, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, null, GameAgent.__$successTest0_rfield, null, null);
         }
         case 15: {
            // lookToAssist_1Step3
            return new GoalStepDebug(15, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, GameAgent.__$argumentExecute0_rfield, null, null, null, "lookToAssist-6->AnonymousStep15()", null, (short)0);
         }
         case 16: {
            // lookToAssist-6->AnonymousStep15_1Step1
            return new GoalStepDebug(16, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, GameAgent.__$argumentExecute0_rfield, null, null, null, "assistAlly(AllyWME, AllyWME)", null, (short)0);
         }
         case 17: {
            // takeTurn_1Step1
            return new MentalStepDebug(17, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, GameAgent.__$mentalExecute0_rfield, null, null, null, (byte)2, "takeTurn_1Step1");
         }
         case 18: {
            // assistAlly_1Step1
            return new MentalStepDebug(18, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, GameAgent.__$mentalExecute0_rfield, null, null, null, (byte)2, "assistAlly_1Step1");
         }
         case 19: {
            // GameAgent_RootCollectionBehaviorStep1
            return new MentalStepDebug(19, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)3, (short)0, false, null, GameAgent.__$mentalExecute0_rfield, null, null, null, (byte)2, "GameAgent_RootCollectionBehaviorStep1");
         }
         case 20: {
            // GameAgent_RootCollectionBehaviorStep2
            return new GoalStepDebug(20, __$stepFactory, __$behaviorParent, true, false, false, false, false, false, (short)-32768, (short)0, false, null, null, null, null, null, "allyAgentRoot()", null, (short)0);
         }
         case 21: {
            // GameAgent_RootCollectionBehaviorStep3
            return new GoalStepDebug(21, __$stepFactory, __$behaviorParent, true, false, false, false, false, false, (short)-32768, (short)0, false, null, null, null, null, null, "lookForAllyAgent()", null, (short)0);
         }
      default:
         throw new AblRuntimeError("Unexpected stepID " + __$stepID);
      }
   }
}
