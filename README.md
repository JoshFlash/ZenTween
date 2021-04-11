![Pleasing](/Pleasing.gif?raw=true)

**Pleasing** is an easy-to-use Monogame and Stride *easing* and *tweening* library. It aims to be as simple as possible while also being flexible enough for any scenario. Unlike other tweening libraries, pleasing includes a tweening timeline which makes it simple to plan multiple tweens on multiple objects.

**The Pleasing logo was created using Pleasing.*

## Installation

**Nuget** 
- Coming soon.

**Zip**

1. Download the repository as a .zip file.
2. Extract it somewhere.
3. Copy Tweening.cs and Easing.cs into your project.
4. If using Stride add 'STRIDE' as a DefineConstant to your Game's csproj
  ```
  <PropertyGroup>
    <DefineConstants>STRIDE</DefineConstants>
    <TargetFramework>net5.0</TargetFramework>
  </PropertyGroup>
  ```
5. Add the namespace to any files using it and you're ready to go!

## Usage

1. Add the namespace: `using Pleasing;`
2. Get a new timeline: `var timeline = Tweening.NewTimeline();`
3. Add a property to the timeline: `var positionProperty = timeline.AddProperty(Player.Transform, "Position");`
4. Add Keyframes: `positionProperty.AddFrame(4000, new Vector3(2, 5, 2), Easing.Cubic.InOut, null);`
5. Call Update every frame: `Tweening.Update(gameTime);`
6. Watch and enjoy!

In Stride, you may also use the extension methods such as `Player.Transform.TweenMove(new Vector3(2, 5, 2), 4f);`

Consult the Wiki for more in-depth information and tutorials.

## Credits

Authors:

* Frank Norton
* Josh Flash

## License

Pleasing is under the MIT license (2021). A copy of the license is found in the root of the repository.
