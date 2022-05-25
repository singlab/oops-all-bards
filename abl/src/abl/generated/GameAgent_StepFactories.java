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
import abl.util.*;
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
            return new MentalStepDebug(2, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, GameAgent.__$mentalExecute0_rfield, null, null, null, (byte)2, "lookForAllyAgent_1Step3");
         }
         case 4: {
            // lookForAllyAgent-0->ConditionalStep3_IF_MentalStep_GoalStep_1Step1
            return new MentalStepDebug(4, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, GameAgent.__$mentalExecute0_rfield, null, null, null, (byte)2, "lookForAllyAgent-0->ConditionalStep3_IF_MentalStep_GoalStep_1Step1");
         }
         case 5: {
            // lookForAllyAgent-0->ConditionalStep3_IF_MentalStep_GoalStep_1Step2
            return new GoalStepDebug(5, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, GameAgent.__$argumentExecute0_rfield, null, null, null, "manageAllyAgent(AllyWME)", null, (short)2);
         }
         case 3: {
            // lookForAllyAgent_1Step4
            return new ConditionalStepDebug(3, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, null, null, null, null, GameAgent.__$conditionalTest0_rfield, null, "lookForAllyAgent-0->ConditionalStep3_IF_MentalStep_GoalStep()", null);
         }
         case 6: {
            // lookForAllyAgent_1Step5
            return new MentalStepDebug(6, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, GameAgent.__$mentalExecute0_rfield, null, null, null, (byte)2, "lookForAllyAgent_1Step5");
         }
         case 7: {
            // allyAgentRoot_1Step1
            return new WaitStepDebug(7, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, null, GameAgent.__$successTest0_rfield, null, null);
         }
         case 8: {
            // manageAllyAgent_1Step1
            return new GoalStepDebug(8, __$stepFactory, __$behaviorParent, true, false, false, false, false, false, (short)-32768, (short)0, false, null, GameAgent.__$argumentExecute0_rfield, null, null, null, "manageCombat(AllyWME)", null, (short)0);
         }
         case 9: {
            // manageAllyAgent_1Step2
            return new GoalStepDebug(9, __$stepFactory, __$behaviorParent, true, false, false, false, false, false, (short)-32768, (short)0, false, null, GameAgent.__$argumentExecute0_rfield, null, null, null, "manageNoncombat(AllyWME)", null, (short)0);
         }
         case 10: {
            // manageCombat_1Step1
            return new WaitStepDebug(10, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, null, GameAgent.__$successTest0_rfield, null, null);
         }
         case 11: {
            // manageCombat_1Step2
            return new MentalStepDebug(11, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, GameAgent.__$mentalExecute0_rfield, null, null, null, (byte)2, "manageCombat_1Step2");
         }
         case 12: {
            // manageCombat_1Step3
            return new GoalStepDebug(12, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, GameAgent.__$argumentExecute0_rfield, null, null, null, "lookToAssist(AllyWME)", null, (short)0);
         }
         case 13: {
            // manageCombat_1Step4
            return new GoalStepDebug(13, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, GameAgent.__$argumentExecute0_rfield, null, null, null, "waitForTurn(AllyWME)", null, (short)0);
         }
         case 14: {
            // waitForTurn_1Step1
            return new MentalStepDebug(14, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, GameAgent.__$mentalExecute0_rfield, null, null, null, (byte)2, "waitForTurn_1Step1");
         }
         case 15: {
            // waitForTurn_1Step2
            return new WaitStepDebug(15, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, null, GameAgent.__$successTest0_rfield, null, null);
         }
         case 16: {
            // waitForTurn_1Step3
            return new GoalStepDebug(16, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, GameAgent.__$argumentExecute0_rfield, null, null, null, "takeTurn(AllyWME)", null, (short)0);
         }
         case 17: {
            // lookToAssist_1Step1
            return new MentalStepDebug(17, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, GameAgent.__$mentalExecute0_rfield, null, null, null, (byte)2, "lookToAssist_1Step1");
         }
         case 18: {
            // lookToAssist_1Step2
            return new WaitStepDebug(18, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, null, GameAgent.__$successTest0_rfield, null, null);
         }
         case 19: {
            // lookToAssist_1Step3
            return new GoalStepDebug(19, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, GameAgent.__$argumentExecute0_rfield, null, null, null, "assistAlly(AllyWME, AllyWME)", null, (short)0);
         }
         case 20: {
            // takeTurn_1Step1
            return new MentalStepDebug(20, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, GameAgent.__$mentalExecute0_rfield, null, null, null, (byte)2, "takeTurn_1Step1");
         }
         case 21: {
            // assistAlly_1Step1
            return new MentalStepDebug(21, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, GameAgent.__$mentalExecute0_rfield, null, null, null, (byte)2, "assistAlly_1Step1");
         }
         case 22: {
            // assistAlly_1Step2
            return new PrimitiveStepDebug(22, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, GameAgent.__$argumentExecute0_rfield, null, null, null, new Protect(), null, "protect");
         }
         case 23: {
            // manageNoncombat_1Step1
            return new WaitStepDebug(23, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, null, GameAgent.__$successTest0_rfield, null, null);
         }
         case 24: {
            // manageNoncombat_1Step2
            return new MentalStepDebug(24, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, GameAgent.__$mentalExecute0_rfield, null, null, null, (byte)2, "manageNoncombat_1Step2");
         }
         case 25: {
            // manageNoncombat_1Step3
            return new GoalStepDebug(25, __$stepFactory, __$behaviorParent, true, false, false, false, false, false, (short)-32768, (short)0, false, null, GameAgent.__$argumentExecute0_rfield, null, null, null, "lookForQuip(AllyWME)", null, (short)0);
         }
         case 26: {
            // manageNoncombat_1Step4
            return new GoalStepDebug(26, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, GameAgent.__$argumentExecute0_rfield, null, null, null, "waitForTurn(AllyWME)", null, (short)0);
         }
         case 27: {
            // lookForQuip_1Step1
            return new WaitStepDebug(27, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, null, GameAgent.__$successTest0_rfield, null, null);
         }
         case 28: {
            // lookForQuip_1Step2
            return new MentalStepDebug(28, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, GameAgent.__$mentalExecute0_rfield, null, null, null, (byte)2, "lookForQuip_1Step2");
         }
         case 29: {
            // lookForQuip_1Step3
            return new PrimitiveStepDebug(29, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, GameAgent.__$argumentExecute0_rfield, null, null, null, new Quip(), null, "quip");
         }
         case 30: {
            // GameAgent_RootCollectionBehaviorStep1
            return new MentalStepDebug(30, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)3, (short)0, false, null, GameAgent.__$mentalExecute0_rfield, null, null, null, (byte)2, "GameAgent_RootCollectionBehaviorStep1");
         }
         case 31: {
            // GameAgent_RootCollectionBehaviorStep2
            return new GoalStepDebug(31, __$stepFactory, __$behaviorParent, true, false, false, false, false, false, (short)-32768, (short)0, false, null, null, null, null, null, "allyAgentRoot()", null, (short)0);
         }
         case 32: {
            // GameAgent_RootCollectionBehaviorStep3
            return new GoalStepDebug(32, __$stepFactory, __$behaviorParent, true, false, false, false, false, false, (short)-32768, (short)0, false, null, null, null, null, null, "lookForAllyAgent()", null, (short)0);
         }
      default:
         throw new AblRuntimeError("Unexpected stepID " + __$stepID);
      }
   }
}
