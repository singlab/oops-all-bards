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
public class GameAgent_ConditionalTests {
   static public boolean conditionalTest0(int __$stepID, final Object[] __$behaviorFrame, final BehavingEntity __$thisEntity) {
      switch (__$stepID) {
         case 4: {
            // lookForVivCommands_1Step4
               if (
                  ! ((GameAgent)__$thisEntity).dict.containsKey(((__ValueTypes.IntVar)__$behaviorFrame[2]).i)
               )

               {
                  return true;
               }


            return false;
         }
         case 11: {
            // processSpawnGoals_1Step3
               if (
                  ((VivWME)__$behaviorFrame[1]).hasGoal("investigateSuspiciousActivity")
               )

               {
                  return true;
               }


            return false;
         }
         case 14: {
            // processSpawnGoals_1Step4
               if (
                  ((VivWME)__$behaviorFrame[1]).hasGoal("neutralizeThreat")
               )

               {
                  return true;
               }


            return false;
         }
         case 17: {
            // processSpawnGoals_1Step5
               if (
                  ((VivWME)__$behaviorFrame[1]).hasGoal("gatherInformation")
               )

               {
                  return true;
               }


            return false;
         }
         case 20: {
            // processSpawnGoals_1Step6
               if (
                  ((VivWME)__$behaviorFrame[1]).hasGoal("maintainGuildSecrecy")
               )

               {
                  return true;
               }


            return false;
         }
      default:
         throw new AblRuntimeError("Unexpected stepID " + __$stepID);
      }
   }
}
