

**ZenTween** is an easy-to-use C# *tweening* library designed to be used in the Stride Game engine. It aims to be as simple as possible while also being flexible enough for any scenario. ZenTween includes a tweening timeline and multiple helper methods to make tweens simple and easy to work with.


## Installation

**Zip**

1. Download the repository as a .zip file and extract it somewhere.
2. Copy all files into your project.
3. Add the namespace to any files using it and you're ready to go!

## Usage

1. Add the namespace: `using ZenTween;`
2. Create a keyframe: `var keyFrame = new TweenKeyFrame<Vector3>(4000, new Vector3(2, 5, 2), Easing.GetEasingFunction(EasingType.CubicInOut));`
3. Initiate the tween: `Tweening.Tween(Entity.Transform, "Position", keyFrame, LerpFunctions.Vector3);`
4. Call Update every frame: `Tweening.Update(gameTime);`
5. Voila!

You may also use the extension methods such as `Entity.Transform.TweenMove(new Vector3(2, 5, 2), 4f);`

**Wiki**

Coming soon

## Credits

Derived from the Monogame tweening library [Pleasing](https://github.com/franknorton/Pleasing) written by Frank Norton.

Authors:

* Josh Flash
* Frank Norton - Pleasing

## License

Pleasing is under the MIT license (2021). A copy of the license is found in the root of the repository.
