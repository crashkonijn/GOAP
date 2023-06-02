# Config > ScriptableObjects

The ScriptableObjects are the main way to configure the GOAP system. They are used to define the goals, actions, sensors, world keys and target keys. This method of configuration is the most simple way to configure the GOAP system and is done by creating scriptable objects through the Unity Editor. 

{% hint style="warning" %}
**Warning** Please keep in mind that this method prevents you from using generic classes. If you need to use generic classes, you should use the code configuration method.
{% endhint %}

{% hint style="info" %}
**Example** The simple demo uses the ScriptableObjects configuration method.
{% endhint %}

![scriptable_configs.png](../images/scriptable_configs.png)

## Sets
To create a set, right click in the project window and select `Create > Goap > Goap Set Config`. This will create a new set config. On this config you must reference all other configs that are part of this set.

![goap-set.png](../images/goap-set.png)

## Goals
To create a goal, right click in the project window and select `Create > Goap > Goal Config`. This will create a new goal config.

![goal-config.png](../images/scriptable_goal.png)

## Actions
To create an action, right click in the project window and select `Create > Goap > Action Config`. This will create a new action config.

![action-config.png](../images/scriptable_action.png)

## World Keys
To create an world key, right click in the project window and select `Create > Goap > World Key`. This will create a new world key.

## World Sensors
To create an world sensor, right click in the project window and select `Create > Goap > World Sensor Config`. This will create a new world sensor config.

![world-sensor-config.png](../images/scriptable_world_sensor.png)

## Target Keys
To create an action, right click in the project window and select `Create > Goap > Target Key`. This will create a new world key.

## Target Sensors
To create a target sensor, right click in the project window and select `Create > Goap > Target Sensor Config`. This will create a new world sensor config.

![target-sensor-config.png](../images/scriptable_target_sensor.png)