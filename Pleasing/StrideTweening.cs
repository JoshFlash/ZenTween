#if STRIDE
using Stride.Core.Mathematics;
using Stride.Engine;
#endif

namespace ZenTween
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
			var keyFrame = new TweenKeyFrame<Vector3>(duration * FromSeconds, destination, Easing.GetEasingFunction(easingType));
			Tween(transform, POSITION, keyFrame, lerpFunction ?? LerpFunctions.Vector3, delay * FromSeconds, onComplete);
		}
		
		public static void TweenLoopMove(this TransformComponent transform, Vector3 destination, float duration,
			EasingType easingTypeIn = EasingType.SinusoidalInOut, EasingType easingTypeOut = EasingType.SinusoidalInOut, float delay = 0, System.Action onComplete = null,
			LerpFunction<Vector3> lerpFunction = null)
		{
			var keyFrameIn = new TweenKeyFrame<Vector3>((duration / 2) * FromSeconds, destination, Easing.GetEasingFunction(easingTypeIn));
			var keyFrameOut = new TweenKeyFrame<Vector3>(duration * FromSeconds, transform.Position, Easing.GetEasingFunction(easingTypeOut));
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
			var keyFrame = new TweenKeyFrame<Quaternion>(duration * FromSeconds, rotation, Easing.GetEasingFunction(easingType));
			Tween(transform, ROTATION, keyFrame, lerpFunction ?? LerpFunctions.Quaternion, delay * FromSeconds, onComplete);
		}
		
		public static void TweenLoopRotate(this TransformComponent transform, Quaternion rotation, float duration,
			EasingType easingTypeIn = EasingType.SinusoidalInOut, EasingType easingTypeOut = EasingType.SinusoidalInOut, float delay = 0, System.Action onComplete = null,
			LerpFunction<Quaternion> lerpFunction = null)
		{
			var keyFrameIn = new TweenKeyFrame<Quaternion>((duration / 2) * FromSeconds, rotation, Easing.GetEasingFunction(easingTypeIn));
			var keyFrameOut = new TweenKeyFrame<Quaternion>(duration * FromSeconds, transform.Rotation, Easing.GetEasingFunction(easingTypeOut));
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
			var keyFrame = new TweenKeyFrame<Vector3>(duration * FromSeconds, scale, Easing.GetEasingFunction(easingType));
			Tween(transform, SCALE, keyFrame, lerpFunction ?? LerpFunctions.Vector3, delay * FromSeconds, onComplete);
		}
		
		public static void TweenLoopScale(this TransformComponent transform, Vector3 scale, float duration,
			EasingType easingTypeIn = EasingType.SinusoidalInOut, EasingType easingTypeOut = EasingType.SinusoidalInOut, float delay = 0, System.Action onComplete = null, 
			LerpFunction<Vector3> lerpFunction = null)
		{
			var keyFrameIn = new TweenKeyFrame<Vector3>((duration / 2) * FromSeconds, scale, Easing.GetEasingFunction(easingTypeIn));
			var keyFrameOut = new TweenKeyFrame<Vector3>(duration * FromSeconds, transform.Scale, Easing.GetEasingFunction(easingTypeOut));
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
	}
}