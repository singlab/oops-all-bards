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
public class GameAgent_ConditionalTests {
   static public boolean conditionalTest0(int __$stepID, final Object[] __$behaviorFrame, final BehavingEntity __$thisEntity) {
      switch (__$stepID) {
         case 4: {
            // lookForVivCommands_1Step5
               if (
                  ((VivWME)__$behaviorFrame[0]).hasGoal("investigateSuspiciousActivity")
               )

               {
                  return true;
               }


            return false;
         }
         case 7: {
            // lookForVivCommands_1Step6
               if (
                  ((VivWME)__$behaviorFrame[0]).hasGoal("neutralizeThreat")
               )

               {
                  return true;
               }


            return false;
         }
         case 10: {
            // lookForVivCommands_1Step7
               if (
                  ((VivWME)__$behaviorFrame[0]).hasGoal("gatherInformation")
               )

               {
                  return true;
               }


            return false;
         }
         case 13: {
            // lookForVivCommands_1Step8
               if (
                  ((VivWME)__$behaviorFrame[0]).hasGoal("maintainGuildSecrecy")
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
