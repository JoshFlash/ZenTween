

**ZenTween** is an easy-to-use C# *tweening* library designed to be used in the Stride Game engine. It aims to be as simple as possible while also being flexible enough for any scenario. ZenTween includes a tweening timeline which makes it simple to plan multiple tweens on multiple objects.


## Installation

**Nuget** 
- Coming soon.

**Zip**

1. Download the repository as a .zip file and extract it somewhere.
2. Copy all files into your project.
3. If using Stride add 'STRIDE' as a DefineConstant to your Game's csproj
  ```
  <PropertyGroup>
    <DefineConstants>STRIDE</DefineConstants>
    <TargetFramework>net5.0</TargetFramework>
  </PropertyGroup>
  ```
4. Add the namespace to any files using it and you're ready to go!

## Usage

1. Add the namespace: `using ZenTween;`
2. Create a keyframe: `var keyFrame = new TweenKeyFrame<Vector3>(4000, new Vector3(2, 5, 2), Easing.GetEasingFunction(EasingType.CubicInOut));`
3. Initiate the tween: `Tweening.Tween(Entity.Transform, "Position", keyFrame, LerpFunctions.Vector3);`
4. Call Update every frame: `Tweening.Update(gameTime);`
5. Watch and enjoy!

In Stride, you may also use the extension methods such as `Player.Transform.TweenMove(new Vector3(2, 5, 2), 4f);`

Consult the Wiki for more in-depth information and tutorials.

## Credits

Forked from the Monogame tweening library "pleasing" written by Frank Norton

Authors:

* Josh Flash
* Frank Norton - Pleasing

## License

Pleasing is under the MIT license (2021). A copy of the license is found in the root of the repository.
