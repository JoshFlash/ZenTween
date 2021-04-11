using System;
#if STRIDE
using Stride.Core.Mathematics;
using Stride.Engine;
#endif

namespace Pleasing
{
	
	public static partial class Tweening
	{
#if STRIDE
		private const string POSITION = "Position";
		private const string ROTATION = "Rotation";
		private const string SCALE = "Scale";
        private const float FromSeconds = 1000f;

		public static void TweenMove(this TransformComponent transform, Vector3 destination, float duration, 
			EasingType easingType = EasingType.CubicInOut, float delay = 0, System.Action onComplete = null, LerpFunction<Vector3> lerpFunction = null)
		{
			Tween(transform, POSITION, destination, duration * FromSeconds, GetEasingFunction(easingType), lerpFunction ?? LerpFunctions.Vector3, delay * FromSeconds, onComplete);
		}
        
		public static void TweenLoopMove(this TransformComponent transform, Vector3 destination, float duration, 
			EasingType easingTypeIn = EasingType.SinusoidalInOut, EasingType easingTypeOut = EasingType.SinusoidalInOut, float delay = 0, System.Action onComplete = null, LerpFunction<Vector3> lerpFunction = null)
		{
			TweenLoop(transform, POSITION, destination, duration * FromSeconds, GetEasingFunction(easingTypeIn), GetEasingFunction(easingTypeOut), lerpFunction ?? LerpFunctions.Vector3, delay * FromSeconds, onComplete);
		}
        
        public static void TweenRotate(this TransformComponent transform, Quaternion finalRotation, float duration, 
            EasingType easingType = EasingType.CubicInOut, float delay = 0, System.Action onComplete = null, LerpFunction<Quaternion> lerpFunction = null)
        {
            Tween(transform, ROTATION, finalRotation, duration * FromSeconds, GetEasingFunction(easingType), lerpFunction ?? LerpFunctions.Quaternion, delay * FromSeconds, onComplete);
        }
        
        public static void TweenScale(this TransformComponent transform, Vector3 scale, float duration, 
            EasingType easingType = EasingType.CubicInOut, float delay = 0, System.Action onComplete = null, LerpFunction<Vector3> lerpFunction = null)
        {
            Tween(transform, SCALE, scale, duration * FromSeconds, GetEasingFunction(easingType), lerpFunction ?? LerpFunctions.Vector3, delay * FromSeconds, onComplete);
        }
#endif
        
		private static EasingFunction GetEasingFunction(EasingType easingType)
		{
			switch (easingType)
			{
				case EasingType.Linear:
					return Easing.Linear;
				case EasingType.QuadraticIn:
					return Easing.Quadratic.In;
				case EasingType.QuadraticOut:
					return Easing.Quadratic.Out;
				case EasingType.QuadraticInOut:
					return Easing.Quadratic.InOut;
				case EasingType.CubicIn:
					return Easing.Cubic.In;
				case EasingType.CubicOut:
					return Easing.Cubic.Out;
				case EasingType.CubicInOut:
					return Easing.Cubic.InOut;
				case EasingType.QuarticIn:
					return Easing.Quartic.In;
				case EasingType.QuarticOut:
					return Easing.Quartic.Out;
				case EasingType.QuarticInOut:
					return Easing.Quartic.InOut;
				case EasingType.QuinticIn:
					return Easing.Quintic.In;
				case EasingType.QuinticOut:
					return Easing.Quintic.Out;
				case EasingType.QuinticInOut:
					return Easing.Quintic.InOut;
				case EasingType.SinusoidalIn:
					return Easing.Sinusoidal.In;
				case EasingType.SinusoidalOut:
					return Easing.Sinusoidal.Out;
				case EasingType.SinusoidalInOut:
					return Easing.Sinusoidal.InOut;
				case EasingType.ExponentialIn:
					return Easing.Exponential.In;
				case EasingType.ExponentialOut:
					return Easing.Exponential.Out;
				case EasingType.ExponentialInOut:
					return Easing.Exponential.InOut;
				case EasingType.CircularIn:
					return Easing.Circular.In;
				case EasingType.CircularOut:
					return Easing.Circular.Out;
				case EasingType.CircularInOut:
					return Easing.Circular.InOut;
				case EasingType.ElasticIn:
					return Easing.Elastic.In;
				case EasingType.ElasticOut:
					return Easing.Elastic.Out;
				case EasingType.ElasticInOut:
					return Easing.Elastic.InOut;
				case EasingType.BackIn:
					return Easing.Back.In;
				case EasingType.BackOut:
					return Easing.Back.Out;
				case EasingType.BackInOut:
					return Easing.Back.InOut;
				case EasingType.BounceIn:
					return Easing.Bounce.In;
				case EasingType.BounceOut:
					return Easing.Bounce.Out;
				case EasingType.BounceInOut:
					return Easing.Bounce.InOut;
				case EasingType.Bezier:
				default:
					throw new ArgumentException($"No Easing Functioned defined for Type: {easingType}");
			}
		}
	}
	
}