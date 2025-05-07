// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using System;

// public enum EventType 
// {
//     DelpResponse,  // pass DELPResponse object
//     CheckQueue,
//     EnemyAI,
//     AllyAI,
//     AwaitPlayerInput,
//     CombatLoss,
//     CombatWin
// };

// /// <summary>
// /// Publisher of a type of event
// /// </summary>
// public class Publisher
// { 
//     public event Action Handlers;

//     public void PublishTheEvent()
//     {
//         // Invoke the action when it is not null
//         Handlers?.Invoke();
//     }

//     public void UnSubscribeAllMethods()
//     {
//         // Removes each action associates to this handler
//         foreach (Action handler in Handlers.GetInvocationList())
//         {
//             Handlers -= handler;
//         }
//     }
// }

// /// <summary>
// /// This system will act as a publisher and keep track of a list of subs.
// ///
// /// An event can be invoke by anyone but only the subscribers will be notified.
// /// 
// /// This is a singleton because we are not sure about what parts of the game could potentially
// /// uses this, so making it accessible for everyone seems to be a good idea
// /// and it does not holds any internal state.
// /// </summary>
// public class EventManager : MonoBehaviour
// {
//     private static EventManager instance;
//     private Dictionary<EventType, Publisher> eventPublishers = new Dictionary<EventType, Publisher>();
//     private object eventData = null;

//     // Holds a reference to the invoker (for the log system)
//     // TODO Consider safety issues with this reference 
//     public object EventData => this.eventData;


//     public static EventManager Instance => EventManager.instance;

//     private void Awake()
//     {
//         // if the singleton hasn't been initialized yet
//         if (instance != null && instance != this)
//         {
//             Destroy(this.gameObject);
//         }
//         else
//         {
//             instance = this;

//             // Initialize publishers
//             foreach (EventType eventType in Enum.GetValues(typeof(EventType)))
//             {
//                 this.eventPublishers.Add(eventType, new Publisher());
//             }
//         }
//         DontDestroyOnLoad(gameObject);
//     }

//     /// <summary>
//     /// Unsubscribes all methods for all events
//     /// </summary>
//     /// <remarks>Idlely the subscribers will unsubscribe, it is a fail-safe</remarks>
//     public void ResetSystem()
//     {
//         foreach (Publisher publisher in this.eventPublishers.Values)
//         {
//             publisher.UnSubscribeAllMethods();
//         }
//     }

//     /// <summary>
//     /// Invoke an event.
//     /// </summary>
//     /// <param name="eventType">Type of the event that just happens</param>
//     public void InvokeEvent(EventType eventType, object eventData)
//     {
//         this.eventData = eventData;
//         this.eventPublishers[eventType].PublishTheEvent();
//     }

//     /// <summary>
//     /// Suscribe to an event with a corresponding action.
//     /// </summary>
//     /// <param name="eventType">The event that suscripber is interested in</param>
//     /// <param name="action">What action to be triggered when event happens</param>
//     public void SubscribeToEvent(EventType eventType, Action handler)
//     {
//         this.eventPublishers[eventType].Handlers += handler;
//     }

//     /// <summary>
//     /// Unsubscribes to an event
//     /// </summary>
//     /// <remarks>Forget to unsubcribe would cause errors</remarks>
//     public void UnsubscribeToEvent(EventType eventType, Action handler)
//     {
//         this.eventPublishers[eventType].Handlers -= handler;
//     }
// }

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum EventType
{
    DelpResponse,   // pass DELPResponse object
    CheckQueue,
    EnemyAI,
    AllyAI,
    AwaitPlayerInput,
    CombatLoss,
    CombatWin,
    OnInteraction
};

/// <summary>
/// Publisher of a type of event
/// </summary>
public class Publisher
{
    // Use Action<object> to allow for any number/type of arguments.
    public event Action<object> Handlers;

    public void PublishTheEvent(object eventData)
    {
        // Invoke the action when it is not null
        Handlers?.Invoke(eventData);
    }

    public void UnSubscribeAllMethods()
    {
        if (Handlers != null)
        {
            // Removes each action associates to this handler
            foreach (Action<object> handler in Handlers.GetInvocationList())
            {
                Handlers -= handler;
            }
        }
    }
}

/// <summary>
/// This system will act as a publisher and keep track of a list of subs.
///
/// An event can be invoke by anyone but only the subscribers will be notified.
///
/// This is a singleton because we are not sure about what parts of the game could potentially
/// uses this, so making it accessible for everyone seems to be a good idea
/// and it does not holds any internal state.
/// </summary>
public class EventManager : MonoBehaviour
{
    private static EventManager instance;
    private Dictionary<EventType, Publisher> eventPublishers = new Dictionary<EventType, Publisher>();
    private object eventData = null;

    // Holds a reference to the invoker (for the log system)
    // TODO Consider safety issues with this reference
    public object EventData => this.eventData;


    public static EventManager Instance => EventManager.instance;

    private void Awake()
    {
        // if the singleton hasn't been initialized yet
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return; //Early return to avoid initializing publishers.
        }

        instance = this;

        // Initialize publishers
        foreach (EventType eventType in Enum.GetValues(typeof(EventType)))
        {
            eventPublishers.Add(eventType, new Publisher());
        }
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Unsubscribes all methods for all events
    /// </summary>
    /// <remarks>Idly the subscribers will unsubscribe, it is a fail-safe</remarks>
    public void ResetSystem()
    {
        foreach (Publisher publisher in this.eventPublishers.Values)
        {
            publisher.UnSubscribeAllMethods();
        }
    }

    /// <summary>
    /// Invoke an event.
    /// </summary>
    /// <param name="eventType">Type of the event that just happens</param>
    //Modified to include event data.
    public void InvokeEvent(EventType eventType, object eventData)
    {
        this.eventData = eventData;
        this.eventPublishers[eventType].PublishTheEvent(eventData);
        this.eventData = null; //Clear event data after.
    }

    /// <summary>
    /// Suscribe to an event with a corresponding action.
    /// </summary>
    /// <param name="eventType">The event that suscripber is interested in</param>
    /// <param name="action">What action to be triggered when event happens</param>
    //Modified to include additional data.
    public void SubscribeToEvent(EventType eventType, Action<object> handler)
    {
        this.eventPublishers[eventType].Handlers += handler;
    }

    /// <summary>
    /// Unsubscribes to an event
    /// </summary>
    /// <remarks>Forget to unsubcribe would cause errors</remarks>
    public void UnsubscribeToEvent(EventType eventType, Action<object> handler)
    {
        this.eventPublishers[eventType].Handlers -= handler;
    }

    // --- Generic Interaction Event Methods ---

    // This is the method other scripts will call to trigger interactions.
    public void TriggerInteraction(GameObject actor, GameObject target, string interactionType, string outcome)
    {
        Debug.Log($"EventManager: TriggerInteraction called with actor: {actor}, target: {target}, interactionType: {interactionType}, outcome: {outcome}");

        if (actor == null) { Debug.LogError("TriggerInteraction called with null actor."); return; }
        if (string.IsNullOrEmpty(interactionType)) { Debug.LogError("TriggerInteraction called with null or empty interactionType."); return; }
        if (string.IsNullOrEmpty(outcome)) { Debug.LogError("TriggerInteraction called with null or empty outcome string."); return; }

        // Package the data into a dictionary.  This is flexible and avoids creating a new class.
        Dictionary<string, object> eventData = new Dictionary<string, object>()
        {
            { "actor", actor },
            { "target", target },
            { "interactionType", interactionType },
            { "outcome", outcome }
        };
        //Trigger the event
        InvokeEvent(EventType.OnInteraction, eventData);
    }
}