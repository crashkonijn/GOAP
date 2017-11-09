# GOAP
A multi-threaded GOAP (Goal Oriented Action Planning) system for Unity3D

This library was used to create the AI in the game [Basher Beatdown](https://youtu.be/x653mVuNP0A?t=12s), but has been written to function on it's own and as such should be usable in any game.

## Functions
* Simple
* Fast
* Multi-Threaded
* GOAP Visualizer

---

# Warning

This project uses [CeilaSpike's Thread Ninja](https://www.assetstore.unity3d.com/en/#!/content/15717) for it's multithreading and won't work without it. Please download and import it first before importing this project.

---

# Usage

The goap system consists mainly out of 3 parts; The GoapAgent, GoapGoal and GoapAction classes. The examples folder contains a very basic example of a basic implementation

## GoapAgent

The GoapAgent is the core of the AI and handles all goals, actions and calculations.

### Minimal implementation
```C#
using SwordGC.AI.Goap;

public class MyGoapAgent : GoapAgent {

    public override void Awake()
    {
        base.Awake();

        // Add goals and actions here
    }

    protected override void Move(GoapAction nextAction)
    {
        // Called when the AI needs to move
    }
}
```
### Variables
```C#
// When set this action will override anything else when it's conditions are true
protected GoapAction interveneAction;

// Can contain an action that needs to be performed when all other actions fail (idle)
protected GoapAction idleAction;

```
### Functions
```C#
// Adds an action to the AI
public void AddAction(GoapAction){}

// Removes an action to the AI
// Note: This is reference based, not type based
public void RemoveAction(GoapAction){}

// Implement for a callback when the AI needs to move
protected override void Move(GoapAction){}
```

## GoapGoal
The start of each AI's decision making, actions are always (sub) child of a goal.
Best practice is to implement multiple goals, not just a "win" goal with a huge tree beneath it.

### Minimal implementation
```C#
using SwordGC.AI.Goap;

public class MyGoapGoal : GoapGoal
{
    public MyGoapGoal(string key, float multiplier = 1) : base(key, multiplier)
    {

    }

    public override void UpdateMultiplier(DataSet data)
    {
        // fancy function that lowers the multiplier if another player kills this AI often
    }
}
```
### Variables
```C#
// The cost of a goal is multiplied by this value. Default is 1, lower value to make this goal more attractive;
public float multiplier;
```
### Functions
```C#
// Implement to update the multiplier when needed
public virtual void UpdateMultiplier(DataSet){}
```
## GoapAction

The GoapAction class is probably the most important in this system. Based on the data in this class the GOAP will create an action tree and choose the right action.

### Minimal/Example implementation

```C#
using SwordGC.AI.Goap;

namespace SwordGC.AI.Actions
{
    public class MyGoapAction : GoapAction
    {
        private int targetPlayerId;

        public MyGoapAction(GoapAgent agent, int targetPlayerId) : base(agent)
        {
            effects.Add(Effects.KNOCKED_OUT_PLAYER + targetPlayerId, true);
            
            requiredRange = 4f;
            cost = 50;

            this.targetPlayerId = targetPlayerId;
            this.targetString = "Player_" + targetPlayerId;
        }

        protected override bool CheckProceduralPreconditions(DataSet data)
        {
            // Check all procedural preconditions
            // Example:
            // if(!needAmmo || !ammoInRange) return false;

            return base.CheckProceduralPreconditions(data);
        }

        public override void Perform()
        {
            // Code to perform this action
        }

        public override GoapAction Clone()
        {
            return new KnockoutPlayerAction(agent, targetPlayerId).SetClone(originalObjectGUID);
        }
    }
}
```
### Variables
```C#
// Optional: When set to the key of a matching goal it will parent itself directly under that goal
public string goal;

// Optional: Add the preconditions to this dictionary
public Dictionary<string, bool> preconditions ;

// Required: Add the effects of the action to this, is used to chain with other actions that have preconditions
public Dictionary<string, bool> effects;

// Required: The cost of this actions 
public float cost;

// Optional: How much delay should be aplied before performing this action (value between 0 (slow) and 1 (fast))
protected float delay = 0.5f;

// Optional: The delay in seconds when the delay value == 0
protected float delaySlow = 0.3f;

// Optional: The delay in seconds when the delay value == 1
protected float delayFast = 0.1f;

// Optional: The action get's disabled shortly after running for this long to prevent getting stuck
protected float maxRunTime = 3f;

// Required: When not in range, the AI wil try to move closer
public float requiredRange;

// Required: Is used to search for a target when no gameObject target is cached
public string targetString;

// Optional: Can be used to easilly remove this action when it's target is destroyed
public bool removeWhenTargetless = false;

// Optional: A string describing this action, usefull for debugging
protected string description;

// There's loads more variables, but are mostly for internal caching and usage

```
### Functions

```C#
// Should be implemented to check if all procedural preconditions are true
protected virtual bool CheckProceduralPreconditions(DataSet data){}

// Required: Is called when this action needs to be performed (During FixedUpdate)
public virtual void Perform(){}

// Required: Implement as in the example, don't forget the .SetClone(originalObjectGUID);
public virtual void Clone(){}

// Called when this actions is first run
public virtual void OnStart(){}

// Called when another action is becoming active
public virtual void OnStop(){}

```

## DataSet

DataSet should contain all the data that the GoapAction.preconditions are checked against.

### Functions

```C#
// Used to update data
public void SetData(string, bool){}

// Used to check data
public void Equals(string, bool){}
```

## GOAP visualizer
The GOAP visualizer gives a good overview of exactly what is going on within the GOAP system.

It can be found under Window -> GOAP Visualizer
![alt text](http://i.imgur.com/8qmyrSQ.png "Visual overview of all node trees")




