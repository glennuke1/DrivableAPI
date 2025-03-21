# Drivable API

The *Drivable API* simplifies the process of creating drivable vehicles in *My Summer Car*.

## Features

- Allows the player to sit inside the car  
- Adds a gear indicator HUD  
- Enables interactive doors  

## Tutorial & Example Code  

### Adding Driving Mode (Unity)

1. Create a new `GameObject` named **PlayerTrigger** as a child of your car's root object.  
   - Add a collider and set `isTrigger` to `true`.  
   - This defines the interior where the player crouches, so size it accordingly.  

2. Add another `GameObject` named **DriveTrigger** as a child of `PlayerTrigger`.  
   - Add a collider and set `isTrigger` to `true`.  
   - Position it above the seat.  

### Adding Driving Mode (Code)

After your car's `GameObject` is loaded, add the following code:

```csharp
// Parameters: car root GameObject, custom car name, player's sitting position offset
DrivableAPI.DrivableAPI.AddDrivingMode(carRoot, "carName", new Vector3(0, -0.25f, 0));
```

To execute an action when the player sits down in the car:

```csharp
// Parameters: car root GameObject, custom car name, player's sitting position offset
DrivableAPI.DrivingMode drivingMode = DrivableAPI.DrivableAPI.AddDrivingMode(carRoot, "carName", new Vector3(0, -0.25f, 0));
drivingMode.OnEnterDrivingMode += OnEnterDrivingMode;
```

### Adding a Gear Indicator  

After setting up the driving mode, add the gear indicator with:

```csharp
// Parameters: car root GameObject, custom car name, is automatic (true/false)
DrivableAPI.DrivableAPI.AddGearIndicator(carRoot, "carName", false);
```

### Adding Doors (Unity)

1. Add a **HingeJoint** component to the door.  
   - Align the yellow arrow to define the opening spot.  
   - Enable **Use Limits**.  

2. Add a **Rigidbody** component to the door.  

### Adding Doors (Code)

In your script, reference the doors and configure their movement:

```csharp
doorL = car.transform.Find("example/Cabin/doorL").gameObject;
doorR = car.transform.Find("example/Cabin/doorR").gameObject;

// Parameters: door GameObject, min angle, max angle, force to apply
DrivableAPI.DrivableAPI.AddDoor(doorL, 0, 80f, 20f);
DrivableAPI.DrivableAPI.AddDoor(doorR, -80f, 0f, -20f);
```

> **Note:** Right-side door values are usually the inverse of left-side values.  
> Example: `(0, 80f, 20f)` → `(-80f, 0, -20f)`

### Adding steer limiter

Add this line of code

```csharp
DrivableAPI.DrivableAPI.AddSteerLimiter(carRoot, 70f, 0f);
```
