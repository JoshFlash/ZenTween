#if STRIDE
using Stride.Games;
using Stride.Core.Mathematics;
using InterpolatorModule = Stride.Core.Mathematics; 
#elif MONOGAME || XNA || XBOX
using Microsoft.Xna.Framework;
using InterpolatorModule = Microsoft.Xna.Framework; 
#endif

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ZenTween
{
    public enum TweenState
    {
        Running,
        Paused,
        Stopped
    }

    /// <summary>
    /// The entry point for tweens. Call Tweening.Update() once per frame.
    /// </summary> 
    public static partial class Tweening
    {
        private static List<TweenTimeline> timelines;
        private static Queue<TweenTimeline> timelinesQueue;

        static Tweening()
        {
            timelines = new List<TweenTimeline>();
            timelinesQueue = new Queue<TweenTimeline>();
        }

        /// <summary>
        /// Creates a new tween and timeline.
        /// </summary>
        /// <param name="target">The object to tween.</param>
        /// <param name="keyFrame">the value, duration, and easing of the tween</param>
        /// <param name="delay">A timed delay before the start of the tween</param>
        /// <param name="onComplete">A callback executed when the tween has completed</param>
        /// <returns>A TweenTimeline with a tween attached.</returns>
        public static void Tween<T>(object target, string propertyName, TweenKeyFrame<T> keyFrame, LerpFunction<T> lerpFunction, float delay = 0, Action onComplete = null)
        {
            var timeline = new TweenTimeline();
            
            var property = timeline.AddProperty<T>(target, propertyName, lerpFunction, onComplete);
            keyFrame.frame += delay;
            property.AddFrame(new TweenKeyFrame<T>(delay, property.initialValue, Easing.Linear));
            property.AddFrame(keyFrame);
            
            timelinesQueue.Enqueue(timeline);
        }

        /// <summary>
        /// Creates a new tween and looped timeline.
        /// </summary>
        /// <param name="target">The object to tween.</param>
        /// <param name="keyFrameIn">the value, duration, and easing of the first half of the tween</param>
        /// <param name="keyFrameOut">the value, duration, and easing of the second half of the tween</param>
        /// <param name="delay">A timed delay before the start of each loop</param>
        /// <param name="onComplete">A callback executed when the loop has completed</param>
        /// <returns>A looped TweenTimeline with a tween attached.</returns>
        public static void TweenLoop<T>(object target, string propertyName, TweenKeyFrame<T> keyFrameIn, TweenKeyFrame<T> keyFrameOut, LerpFunction<T> lerpFunction, float delay = 0, Action onComplete = null)
        {
            var timeline = new TweenTimeline();
            var property = timeline.AddProperty<T>(target, propertyName, lerpFunction, onComplete);

            if (delay > 0)
            {
                property.AddFrame(new TweenKeyFrame<T>(0, keyFrameOut.value, Easing.Linear));
            }
            property.AddFrame(new TweenKeyFrame<T>(delay, keyFrameOut.value, Easing.Linear));
            property.AddFrame(new TweenKeyFrame<T>(delay + keyFrameIn.frame, keyFrameIn.value, keyFrameIn.easingFunction));
            property.AddFrame(new TweenKeyFrame<T>(delay + keyFrameOut.frame, keyFrameOut.value, keyFrameOut.easingFunction));
            
            timeline.Loop = true;
            timelinesQueue.Enqueue(timeline);
        }
        
        public static void TweenOneShotSequence<T>(object target, string propertyName, TweenSequence<T> sequence)
        {
            if (sequence.KeyFrames.Count == 0)
            {
                sequence.OnComplete?.Invoke();
                return;
            }

            var timeline = new TweenTimeline();

            TweenableProperty<T> property = timeline.AddProperty<T>(target, propertyName, sequence.LerpFunction, sequence.OnComplete);
            foreach (var keyFrame in sequence.KeyFrames)
            {
                property.AddFrame(keyFrame);
            }
			
            timelinesQueue.Enqueue(timeline);
        }
		
        public static void TweenLoopSequence<T>(object target, string propertyName, TweenSequence<T> sequence, TweenSequence<T> introSequence = null)
        {

            if (introSequence != null)
            {
                introSequence.OnComplete += () => TweenLoopSequence<T>(target, propertyName, sequence, null);
                TweenOneShotSequence(target, propertyName, introSequence);
                return;
            }

            var timeline = new TweenTimeline();

            TweenableProperty<T> property = timeline.AddProperty<T>(target, propertyName, sequence.LerpFunction, sequence.OnComplete);
            foreach (var keyFrame in sequence.KeyFrames)
            {
                property.AddFrame(keyFrame);
            }
			
            timeline.Loop = true;
            timelinesQueue.Enqueue(timeline);
        }

        public static void Clear()
        {
            timelines.Clear();
            timelinesQueue.Clear();
        }

#if XNA || XBOX || MONOGAME || STRIDE

        public static void Update(GameTime gameTime)
        {
            while (timelinesQueue.Count > 0)
            {
                timelines.Add(timelinesQueue.Dequeue());
            }
            
            foreach (var timeline in timelines)
            {
                timeline.Update(gameTime);
            }

            timelines.RemoveAll(timeline => timeline.State == TweenState.Stopped);
        }
        
#endif

        public static void Update(float deltaTime)
        {
            while (timelinesQueue.Count > 0)
            {
                timelines.Add(timelinesQueue.Dequeue());
            }
            foreach(var timeline in timelines)
            {
                timeline.Update(deltaTime);
            }
        }
    }

    /// <summary>
    /// The TweenTimeline holds tweens and runs them in sequence based on the elapsed time.
    /// </summary>
    public class TweenTimeline
    {
        public List<ITweenableProperty> TweeningProperties;
        public float ElapsedMilliseconds;
        public bool Loop;
        public TweenState State;

        public TweenTimeline()
        {
            TweeningProperties = new List<ITweenableProperty>();
            State = TweenState.Running;
        }

        public void Start()
        {
            State = TweenState.Running;
        }

        public void Stop()
        {
            State = TweenState.Stopped;
        }

        public void Pause()
        {
            State = TweenState.Paused;
        }

        public void Restart()
        {
            ElapsedMilliseconds = 0;
            State = TweenState.Running;
        }

        public TweenableProperty<T> AddProperty<T>(object target, string propertyName, LerpFunction<T> lerpFunction, Action onComplete)
        {
            var properties = target.GetType().GetProperties();
            var fields = target.GetType().GetFields();

            var property = properties.FirstOrDefault(x => x.Name == propertyName);
            if (property != null)
            {
                if (property.PropertyType == typeof(T))
                {
                    var t = new TweenProperty<T>(target, property, lerpFunction, onComplete);
                    TweeningProperties.Add(t);
                    return t;
                }
            }
            else
            {
                var field = fields.FirstOrDefault(x => x.Name == propertyName);
                if (field != null)
                {
                    if (field.FieldType == typeof(T))
                    {
                        var t = new TweenField<T>(target, field, lerpFunction, onComplete);
                        TweeningProperties.Add(t);
                        return t;
                    }
                }
            }
            return null;
        }

        public TweenableProperty<T> AddProperty<T>(T initialValue, Action<T> setter, LerpFunction<T> lerpFunction, Action onComplete)
            where T : unmanaged
        {
            var t = new TweenSetter<T>(initialValue, setter, lerpFunction, onComplete);
            return t;
        }

#if XNA || XBOX || MONOGAME || STRIDE

        public void Update(GameTime gameTime)
        {
#if STRIDE
            ElapsedMilliseconds += (float) gameTime.Elapsed.Milliseconds;
#else
            ElapsedMilliseconds += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
#endif

            if (State == TweenState.Running)
            {
                var propertiesFinished = 0;
                foreach (var property in TweeningProperties)
                {
                    if (!property.Update(ElapsedMilliseconds)) //No Frames Left
                    {
                        propertiesFinished++;
                    }
                }

                if (propertiesFinished == TweeningProperties.Count)
                {
                    ElapsedMilliseconds = 0;
                    if (Loop)
                    {
                        Restart();
                    }
                    else
                    {
                        Stop();
                    }
                }
            }
        }
#endif

        public void Update(float deltaTime)
        {
            ElapsedMilliseconds += deltaTime;
            if (State == TweenState.Running)
            {
                var propertiesFinished = 0;
                foreach (var property in TweeningProperties)
                {
                    if (!property.Update(ElapsedMilliseconds)) //No Frames Left
                    {
                        propertiesFinished++;
                    }
                }

                if (propertiesFinished == TweeningProperties.Count)
                {
                    ElapsedMilliseconds = 0;
                    if (Loop)
                    {
                        Restart();
                    }
                    else
                    {
                        Stop();
                    }
                }
            }
        }
    }

    public interface ITweenableProperty
    {
        bool Update(float timelineElapsed);
    }
    
    public abstract class TweenableProperty<T> : ITweenableProperty
    {
        internal T initialValue { get; init; }

        protected object target { get; init; }
        protected Action onComplete { get; init; }
        protected List<TweenKeyFrame<T>> keyFrames { get; init; }
        protected LerpFunction<T> lerpFunction { get; init; }
        protected bool Done;

        protected TweenableProperty(object target, Action onComplete,LerpFunction<T> lerpFunction)
        {
            this.target = target;
            this.onComplete = onComplete;
            keyFrames = new List<TweenKeyFrame<T>>();
            this.lerpFunction = lerpFunction;
        }

        public TweenableProperty<T> AddFrame(TweenKeyFrame<T> keyFrame)
        {
            keyFrames.Add(keyFrame);
            keyFrames.Sort((x,y) => x.frame.CompareTo(y.frame));
            return this;
        }

        public TweenableProperty<T> RemoveFrame(TweenKeyFrame<T> keyFrame)
        {
            keyFrames.Remove(keyFrame);
            keyFrames.Sort((x,y) => x.frame.CompareTo(y.frame));
            return this;
        }

        public bool Update(float timelineElapsed)
        {
            if (keyFrames.Count == 0)
            {
                Done = true;
                return false;
            }

            TweenKeyFrame<T> lastFrame = null;
            TweenKeyFrame<T> nextFrame = null;
            foreach(var frame in keyFrames)
            {
                if(frame.frame <= timelineElapsed)
                {
                    lastFrame = frame;
                }
                else
                {
                    nextFrame = frame;
                    break;
                }
            }

            if(nextFrame == null)
            {
                if (!Done)
                {
                    SetValue(lastFrame.value);
                    onComplete?.Invoke();
                }

                Done = true;
                return false;
            }
            
            if (lastFrame == null)
            {
                var progress = timelineElapsed / nextFrame.frame;
                var easedProgress = nextFrame.easingFunction(progress);
                var newValue = lerpFunction(initialValue, nextFrame.value, easedProgress);
                SetValue(newValue);
            }
            else
            {
                var progress = (timelineElapsed - lastFrame.frame) / (nextFrame.frame - lastFrame.frame);
                var easedProgress = nextFrame.easingFunction(progress);
                var newValue = lerpFunction(lastFrame.value, nextFrame.value, easedProgress);
                SetValue(newValue);
            }

            return true;
        }

        public abstract void SetValue(T value);
    }
    
    internal class TweenProperty<T> : TweenableProperty<T>
    {
        private PropertyInfo property { get; init; }

        public TweenProperty(object target, PropertyInfo property, LerpFunction<T> lerpFunction, Action onComplete)
            :base(target, onComplete, lerpFunction)
        {
            this.property = property;
            initialValue = (T)property.GetValue(target);
        }
        
        public override void SetValue(T value)
        {
            property.SetValue(target, value);
        }
    }
    
    internal class TweenField<T> : TweenableProperty<T>
    {
        private FieldInfo field { get; init; }

        public TweenField(object target, FieldInfo field, LerpFunction<T> lerpFunction, Action onComplete)
            :base(target, onComplete, lerpFunction)
        {
            this.field = field;
            initialValue = (T)field.GetValue(target);
        }
        
        public override void SetValue(T value)
        {
            field.SetValue(target, value);
        }
    }
    
    internal class TweenSetter<T> : TweenableProperty<T>
        where T : unmanaged
    {
        private Action<T> setter { get; init; }

        public TweenSetter(T initialValue, Action<T> setter, LerpFunction<T> lerpFunction, Action onComplete)
            : base(initialValue, onComplete, lerpFunction)
        {
            this.initialValue = initialValue;
            this.setter = setter;
        }
        
        public override void SetValue(T value)
        {
            setter.Invoke(value);
        }
    }

    public class TweenKeyFrame<T>
    {
        public float frame { get; set; }
        public T value { get; set; }
        public EasingFunction easingFunction { get; set; }

        public TweenKeyFrame(float frame, T value, EasingFunction easingFunction)
        {
            this.frame = frame;
            this.value = value;
            this.easingFunction = easingFunction;
        }
    }

    public class TweenSequence<T>
    {
        public List<TweenKeyFrame<T>> KeyFrames = default;
        public LerpFunction<T> LerpFunction = default;
        public System.Action OnComplete = null;

        public TweenSequence(List<TweenKeyFrame<T>> keyFrames, LerpFunction<T> lerpFunction, Action onComplete = null)
        {
            KeyFrames = keyFrames;
            LerpFunction = lerpFunction;
            OnComplete = onComplete;
        }
    }

    public delegate float EasingFunction(float val);
    public delegate T LerpFunction<T>(T start, T end, float progress);
    public static class LerpFunctions
    {
        public static LerpFunction<float> Float = (s, e, p) => s + (e - s) * p;
#if XNA || XBOX || MONOGAME || STRIDE
        public static readonly LerpFunction<Vector2>    Vector2     = InterpolatorModule.Vector2.Lerp;
        public static readonly LerpFunction<Vector3>    Vector3     = InterpolatorModule.Vector3.Lerp;
        public static readonly LerpFunction<Vector4>    Vector4     = InterpolatorModule.Vector4.Lerp;
        public static readonly LerpFunction<Color>      Color       = InterpolatorModule.Color.Lerp;
        public static readonly LerpFunction<Quaternion> Quaternion  = InterpolatorModule.Quaternion.Lerp;
        public static readonly LerpFunction<Rectangle>  Rectangle   = (s, e, p) =>
        {
            var pX = s.X + (e.X - s.X) * p;
            var pY = s.Y + (e.Y - s.Y) * p;
            var width = s.Width + (e.Width - s.Width) * p;
            var height = s.Height + (e.Height - s.Height) * p;
            return new InterpolatorModule.Rectangle((int)pX, (int)pY, (int)width, (int)height);
        };  
#endif
    }


}
