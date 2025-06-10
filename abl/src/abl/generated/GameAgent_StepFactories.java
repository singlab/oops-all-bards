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
import java.util.HashMap;
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
            // lookForVivCommands_1Step1
            return new MentalStepDebug(0, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, GameAgent.__$mentalExecute0_rfield, null, null, null, (byte)2, "lookForVivCommands_1Step1");
         }
         case 1: {
            // lookForVivCommands_1Step2
            return new WaitStepDebug(1, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, null, GameAgent.__$successTest0_rfield, null, null);
         }
         case 2: {
            // lookForVivCommands_1Step3
            return new MentalStepDebug(2, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, GameAgent.__$mentalExecute0_rfield, null, null, null, (byte)2, "lookForVivCommands_1Step3");
         }
         case 3: {
            // lookForVivCommands_1Step4
            return new MentalStepDebug(3, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, GameAgent.__$mentalExecute0_rfield, null, null, null, (byte)2, "lookForVivCommands_1Step4");
         }
         case 5: {
            // lookForVivCommands-0->ConditionalStep4_IF_MentalStep_GoalStep_1Step1
            return new MentalStepDebug(5, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, GameAgent.__$mentalExecute0_rfield, null, null, null, (byte)2, "lookForVivCommands-0->ConditionalStep4_IF_MentalStep_GoalStep_1Step1");
         }
         case 6: {
            // lookForVivCommands-0->ConditionalStep4_IF_MentalStep_GoalStep_1Step2
            return new GoalStepDebug(6, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, GameAgent.__$argumentExecute0_rfield, null, null, null, "investigateSuspiciousActivity(int, int)", null, (short)2);
         }
         case 4: {
            // lookForVivCommands_1Step5
            return new ConditionalStepDebug(4, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, null, null, null, null, GameAgent.__$conditionalTest0_rfield, null, "lookForVivCommands-0->ConditionalStep4_IF_MentalStep_GoalStep()", null);
         }
         case 8: {
            // lookForVivCommands-0->ConditionalStep7_IF_MentalStep_GoalStep_1Step1
            return new MentalStepDebug(8, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, GameAgent.__$mentalExecute0_rfield, null, null, null, (byte)2, "lookForVivCommands-0->ConditionalStep7_IF_MentalStep_GoalStep_1Step1");
         }
         case 9: {
            // lookForVivCommands-0->ConditionalStep7_IF_MentalStep_GoalStep_1Step2
            return new GoalStepDebug(9, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, GameAgent.__$argumentExecute0_rfield, null, null, null, "neutralizeThreat(int, int)", null, (short)2);
         }
         case 7: {
            // lookForVivCommands_1Step6
            return new ConditionalStepDebug(7, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, null, null, null, null, GameAgent.__$conditionalTest0_rfield, null, "lookForVivCommands-0->ConditionalStep7_IF_MentalStep_GoalStep()", null);
         }
         case 11: {
            // lookForVivCommands-0->ConditionalStep10_IF_MentalStep_GoalStep_1Step1
            return new MentalStepDebug(11, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, GameAgent.__$mentalExecute0_rfield, null, null, null, (byte)2, "lookForVivCommands-0->ConditionalStep10_IF_MentalStep_GoalStep_1Step1");
         }
         case 12: {
            // lookForVivCommands-0->ConditionalStep10_IF_MentalStep_GoalStep_1Step2
            return new GoalStepDebug(12, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, GameAgent.__$argumentExecute0_rfield, null, null, null, "gatherInformation(int, int)", null, (short)2);
         }
         case 10: {
            // lookForVivCommands_1Step7
            return new ConditionalStepDebug(10, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, null, null, null, null, GameAgent.__$conditionalTest0_rfield, null, "lookForVivCommands-0->ConditionalStep10_IF_MentalStep_GoalStep()", null);
         }
         case 14: {
            // lookForVivCommands-0->ConditionalStep13_IF_MentalStep_GoalStep_1Step1
            return new MentalStepDebug(14, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, GameAgent.__$mentalExecute0_rfield, null, null, null, (byte)2, "lookForVivCommands-0->ConditionalStep13_IF_MentalStep_GoalStep_1Step1");
         }
         case 15: {
            // lookForVivCommands-0->ConditionalStep13_IF_MentalStep_GoalStep_1Step2
            return new GoalStepDebug(15, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, GameAgent.__$argumentExecute0_rfield, null, null, null, "maintainGuildSecrecy(int, int)", null, (short)2);
         }
         case 13: {
            // lookForVivCommands_1Step8
            return new ConditionalStepDebug(13, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, null, null, null, null, GameAgent.__$conditionalTest0_rfield, null, "lookForVivCommands-0->ConditionalStep13_IF_MentalStep_GoalStep()", null);
         }
         case 16: {
            // lookForVivCommands_1Step9
            return new MentalStepDebug(16, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, GameAgent.__$mentalExecute0_rfield, null, null, null, (byte)2, "lookForVivCommands_1Step9");
         }
         case 17: {
            // vivAgentRoot_1Step1
            return new WaitStepDebug(17, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, null, GameAgent.__$successTest0_rfield, null, null);
         }
         case 18: {
            // investigateSuspiciousActivity_1Step1
            return new MentalStepDebug(18, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, GameAgent.__$mentalExecute0_rfield, null, null, null, (byte)2, "investigateSuspiciousActivity_1Step1");
         }
         case 19: {
            // investigateSuspiciousActivity_1Step2
            return new PrimitiveStepDebug(19, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, GameAgent.__$argumentExecute0_rfield, null, null, null, new MoveToPosition(), null, "moveToPosition");
         }
         case 20: {
            // investigateSuspiciousActivity_1Step3
            return new WaitStepDebug(20, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, null, GameAgent.__$successTest0_rfield, null, null);
         }
         case 21: {
            // investigateSuspiciousActivity_1Step4
            return new PrimitiveStepDebug(21, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, GameAgent.__$argumentExecute0_rfield, null, null, null, new ObserveTarget(), null, "observeTarget");
         }
         case 22: {
            // investigateSuspiciousActivity_1Step5
            return new WaitStepDebug(22, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, null, GameAgent.__$successTest0_rfield, null, null);
         }
         case 23: {
            // neutralizeThreat_1Step1
            return new MentalStepDebug(23, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, GameAgent.__$mentalExecute0_rfield, null, null, null, (byte)2, "neutralizeThreat_1Step1");
         }
         case 24: {
            // neutralizeThreat_1Step2
            return new PrimitiveStepDebug(24, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, GameAgent.__$argumentExecute0_rfield, null, null, null, new AggressiveConfrontation(), null, "aggressiveConfrontation");
         }
         case 25: {
            // neutralizeThreat_1Step3
            return new WaitStepDebug(25, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, null, GameAgent.__$successTest0_rfield, null, null);
         }
         case 26: {
            // gatherInformation_1Step1
            return new MentalStepDebug(26, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, GameAgent.__$mentalExecute0_rfield, null, null, null, (byte)2, "gatherInformation_1Step1");
         }
         case 27: {
            // gatherInformation_1Step2
            return new PrimitiveStepDebug(27, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, GameAgent.__$argumentExecute0_rfield, null, null, null, new ObserveTarget(), null, "observeTarget");
         }
         case 28: {
            // gatherInformation_1Step3
            return new WaitStepDebug(28, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, null, GameAgent.__$successTest0_rfield, null, null);
         }
         case 29: {
            // maintainGuildSecrecy_1Step1
            return new MentalStepDebug(29, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, GameAgent.__$mentalExecute0_rfield, null, null, null, (byte)2, "maintainGuildSecrecy_1Step1");
         }
         case 30: {
            // maintainGuildSecrecy_1Step2
            return new PrimitiveStepDebug(30, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, GameAgent.__$argumentExecute0_rfield, null, null, null, new CalmConfrontation(), null, "calmConfrontation");
         }
         case 31: {
            // maintainGuildSecrecy_1Step3
            return new WaitStepDebug(31, __$stepFactory, __$behaviorParent, false, false, false, false, false, false, (short)-32768, (short)0, false, null, null, GameAgent.__$successTest0_rfield, null, null);
         }
         case 32: {
            // GameAgent_RootCollectionBehaviorStep1
            return new GoalStepDebug(32, __$stepFactory, __$behaviorParent, true, false, false, false, false, false, (short)-32768, (short)0, false, null, null, null, null, null, "vivAgentRoot()", null, (short)0);
         }
         case 33: {
            // GameAgent_RootCollectionBehaviorStep2
            return new GoalStepDebug(33, __$stepFactory, __$behaviorParent, true, false, false, false, false, false, (short)-32768, (short)0, false, null, null, null, null, null, "lookForVivCommands()", null, (short)0);
         }
      default:
         throw new AblRuntimeError("Unexpected stepID " + __$stepID);
      }
   }
}
