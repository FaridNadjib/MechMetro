hey so this is how the pickup system works.

basically you got two main things: the `pickupcontroller` on the roomba and the `pickupable` script on the things you wanna pick up.

**how to make something pickupable:**

1.  slap the `pickupable.cs` script on it. or even better, make a new script that inherits from it (like `rocketlauncher.cs`) so you can do custom stuff.
2.  make sure it has a rigidbody.
3.  put it on the "pickupable" layer.
4.  give it a trigger collider.

**how the roomba picks things up:**

1.  the roomba has a `pickupcontroller.cs` script.
2.  it also needs a trigger collider to find stuff.
3.  and it needs an empty gameobject called "pickupholder" as a child. this is where the picked up item will live.

when the roomba's trigger touches a pickupable thing's trigger, the `pickupcontroller` grabs it and makes it a child of the `pickupholder`.

**using the picked up item:**

*   left-click calls `onprimaryuse()` on the item's script.
*   right-click calls `ondrop()` to drop it.

for the rocket launcher, it uses the `playercontrolledentity` to know where you're looking so it can aim.

that's pretty much it. just make sure you set up the layers and colliders right in unity and you should be good to go.