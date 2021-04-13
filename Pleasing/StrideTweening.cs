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
			var keyFrame = new TweenKeyFrame<Vector3>(duration * FromSeconds, destination, GetEasingFunction(easingType));
			Tween(transform, POSITION, keyFrame, lerpFunction ?? LerpFunctions.Vector3, delay * FromSeconds, onComplete);
		}
		
		public static void TweenLoopMove(this TransformComponent transform, Vector3 destination, float duration,
			EasingType easingTypeIn = EasingType.SinusoidalInOut, EasingType easingTypeOut = EasingType.SinusoidalInOut, float delay = 0, System.Action onComplete = null,
			LerpFunction<Vector3> lerpFunction = null)
		{
			var keyFrameIn = new TweenKeyFrame<Vector3>((duration / 2) * FromSeconds, destination, GetEasingFunction(easingTypeIn));
			var keyFrameOut = new TweenKeyFrame<Vector3>(duration * FromSeconds, transform.Position, GetEasingFunction(easingTypeOut));
			TweenLoop(transform, POSITION, keyFrameIn, keyFrameOut, lerpFunction ?? LerpFunctions.Vector3, delay * FromSeconds, onComplete);
		}

		public static void TweenLoopMoveSequence(this TransformComponent transform, TweenSequence<Vector3> tweenSequence, TweenSequence<Vector3> introSequence = null)
		{
			TweenLoopSequence(transform, POSITION, tweenSequence, introSequence);
		}

		public static void TweenMoveSequence(this TransformComponent transform, TweenSequence<Vector3> tweenSequence)
		{
			TweenOneShotSequence(transform, POSITION, tweenSequence);
		}

		public static void TweenRotate(this TransformComponent transform, Quaternion rotation, float duration,
			EasingType easingType = EasingType.CubicInOut, float delay = 0, System.Action onComplete = null, LerpFunction<Quaternion> lerpFunction = null)
		{
			var keyFrame = new TweenKeyFrame<Quaternion>(duration * FromSeconds, rotation, GetEasingFunction(easingType));
			Tween(transform, ROTATION, keyFrame, lerpFunction ?? LerpFunctions.Quaternion, delay * FromSeconds, onComplete);
		}
		
		public static void TweenLoopRotate(this TransformComponent transform, Quaternion rotation, float duration,
			EasingType easingTypeIn = EasingType.SinusoidalInOut, EasingType easingTypeOut = EasingType.SinusoidalInOut, float delay = 0, System.Action onComplete = null,
			LerpFunction<Quaternion> lerpFunction = null)
		{
			var keyFrameIn = new TweenKeyFrame<Quaternion>((duration / 2) * FromSeconds, rotation, GetEasingFunction(easingTypeIn));
			var keyFrameOut = new TweenKeyFrame<Quaternion>(duration * FromSeconds, transform.Rotation, GetEasingFunction(easingTypeOut));
			TweenLoop(transform, ROTATION, keyFrameIn, keyFrameOut, lerpFunction ?? LerpFunctions.Quaternion, delay * FromSeconds, onComplete);
		}

		public static void TweenLoopRotateSequence(this TransformComponent transform, TweenSequence<Quaternion> tweenSequence, TweenSequence<Quaternion> introSequence = null)
		{
			TweenLoopSequence(transform, ROTATION, tweenSequence, introSequence);
		}

		public static void TweenRotateSequence(this TransformComponent transform, TweenSequence<Quaternion> tweenSequence)
		{
			TweenOneShotSequence(transform, ROTATION, tweenSequence);
		}
		
		public static void TweenScale(this TransformComponent transform, Vector3 scale, float duration,
			EasingType easingType = EasingType.CubicInOut, float delay = 0, System.Action onComplete = null, LerpFunction<Vector3> lerpFunction = null)
		{
			var keyFrame = new TweenKeyFrame<Vector3>(duration * FromSeconds, scale, GetEasingFunction(easingType));
			Tween(transform, SCALE, keyFrame, lerpFunction ?? LerpFunctions.Vector3, delay * FromSeconds, onComplete);
		}
		
		public static void TweenLoopScale(this TransformComponent transform, Vector3 scale, float duration,
			EasingType easingTypeIn = EasingType.SinusoidalInOut, EasingType easingTypeOut = EasingType.SinusoidalInOut, float delay = 0, System.Action onComplete = null, 
			LerpFunction<Vector3> lerpFunction = null)
		{
			var keyFrameIn = new TweenKeyFrame<Vector3>((duration / 2) * FromSeconds, scale, GetEasingFunction(easingTypeIn));
			var keyFrameOut = new TweenKeyFrame<Vector3>(duration * FromSeconds, transform.Scale, GetEasingFunction(easingTypeOut));
			TweenLoop(transform, SCALE, keyFrameIn, keyFrameOut, lerpFunction ?? LerpFunctions.Vector3, delay * FromSeconds, onComplete);
		}
		
		public static void TweenLoopScaleSequence(this TransformComponent transform, TweenSequence<Vector3> tweenSequence, TweenSequence<Vector3> introSequence = null)
		{
			TweenLoopSequence(transform, SCALE, tweenSequence, introSequence);
		}

		public static void TweenScaleSequence(this TransformComponent transform, TweenSequence<Vector3> tweenSequence)
		{
			TweenOneShotSequence(transform, SCALE, tweenSequence);
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