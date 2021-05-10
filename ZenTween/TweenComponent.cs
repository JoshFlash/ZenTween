using System;
using System.Collections.Generic;
using Stride.Animations;
using Stride.Core;
using Stride.Core.Mathematics;
using Stride.Engine;
using TattvaCCG;

namespace ZenTween
{
	public class TweenComponent : StartupScript
	{
		public bool Enabled { get; set; } = true;

		[DataMember] public ComputeCurveSampler<Vector3> TweenCurve { get; set; } = new ComputeCurveSamplerVector3();
		public EasingType EasingType { get; set; } = EasingType.Linear;
		public bool LoopTween = false;

		private ComputeAnimationCurve<Vector3> computeAnimationCurve { get; set; } = new ComputeAnimationCurveVector3();
		private List<TweenKeyFrame<Vector3>> tweenKeyFrames = new();
		
		public override void Start()
		{
			if (!Enabled)
				return;
			
			computeAnimationCurve = TweenCurve.Curve as ComputeAnimationCurve<Vector3>;
			if (computeAnimationCurve != null)
			{
				foreach (var keyFrame in computeAnimationCurve.KeyFrames)
				{
					tweenKeyFrames.Add(new TweenKeyFrame<Vector3>(keyFrame.Key * 1000, keyFrame.Value, Easing.GetEasingFunction(EasingType)));
				}
			}

			if (tweenKeyFrames.Count > 1)
			{
				TweenSequence<Vector3> tweenSequence = new TweenSequence<Vector3>(tweenKeyFrames, LerpFunctions.Vector3);
				if (LoopTween)
				{
					Entity.Transform.TweenLoopMoveSequence(tweenSequence);
				}
				else
				{
					Entity.Transform.TweenMoveSequence(tweenSequence);
				}
			}
			else if (tweenKeyFrames.Count == 1)
			{
				if (LoopTween)
				{
					Entity.Transform.TweenLoopMove(tweenKeyFrames[0].value, tweenKeyFrames[0].frame, EasingType);
				}
				else
				{
					Entity.Transform.TweenMove(tweenKeyFrames[0].value, tweenKeyFrames[0].frame, EasingType);
				}
			}
		}
	}
}