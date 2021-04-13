
#if MONOGAME
using Microsoft.Xna.Framework;
#elif STRIDE
using Stride.Games;
using Stride.Core.Mathematics;
#endif

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Pleasing
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

        //For one-off tweens
        private static List<TweenTimeline> singleTimelines;
        private static Queue<TweenTimeline> singleTimelinesQueue;
        private static List<TweenTimeline> removeTimelines;

        static Tweening()
        {
            timelines = new List<TweenTimeline>();
            timelinesQueue = new Queue<TweenTimeline>();
            singleTimelines = new List<TweenTimeline>();
            singleTimelinesQueue = new Queue<TweenTimeline>();
            removeTimelines = new List<TweenTimeline>();
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
            
            singleTimelinesQueue.Enqueue(timeline);
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
            singleTimelinesQueue.Enqueue(timeline);
        }
        
        public static void TweenOneShotSequence<T>(object target, string propertyName, TweenSequence<T> sequence)
        {
            if (sequence.keyFrames.Count == 0)
            {
                sequence.onComplete?.Invoke();
                return;
            }

            var timeline = new TweenTimeline();

            TweenableProperty<T> property = timeline.AddProperty<T>(target, propertyName, sequence.lerpFunction, sequence.onComplete);
            foreach (var keyFrame in sequence.keyFrames)
            {
                property.AddFrame(keyFrame);
            }
			
            singleTimelinesQueue.Enqueue(timeline);
        }
		
        public static void TweenLoopSequence<T>(object target, string propertyName, TweenSequence<T> sequence, TweenSequence<T> introSequence = null)
        {

            if (introSequence != null)
            {
                introSequence.onComplete += () => TweenLoopSequence<T>(target, propertyName, sequence, null);
                TweenOneShotSequence(target, propertyName, introSequence);
                return;
            }

            var timeline = new TweenTimeline();

            TweenableProperty<T> property = timeline.AddProperty<T>(target, propertyName, sequence.lerpFunction, sequence.onComplete);
            foreach (var keyFrame in sequence.keyFrames)
            {
                property.AddFrame(keyFrame);
            }
			
            timeline.Loop = true;
            singleTimelinesQueue.Enqueue(timeline);
        }

        public static TweenTimeline NewTimeline()
        {
            var timeline = new TweenTimeline(0);
            timeline.AdaptiveDuration = true;
            timelinesQueue.Enqueue(timeline);
            return timeline;
        }
        /// <summary>
        /// Creates a new timeline.
        /// </summary>
        /// <param name="duration">The length of the timeline in milliseconds.
        /// Leave blank to have tweens set it.</param>
        /// <returns></returns>
        public static TweenTimeline NewTimeline(float duration)
        {
            var timeline = new TweenTimeline(duration);
            timelinesQueue.Enqueue(timeline);
            return timeline;
        }

        public static void Clear()
        {
            timelines.Clear();
            singleTimelines.Clear();
            removeTimelines.Clear();
            
            timelinesQueue.Clear();
            singleTimelinesQueue.Clear();
        }

#if XNA || WINDOWS_PHONE || XBOX || ANDROID || MONOGAME || STRIDE

        public static void Update(GameTime gameTime)
        {
            while (timelinesQueue.Count > 0)
            {
                timelines.Add(timelinesQueue.Dequeue());
            }
            while (singleTimelinesQueue.Count > 0)
            {
                singleTimelines.Add(singleTimelinesQueue.Dequeue());
            }
            
            foreach (var timeline in timelines)
            {
                timeline.Update(gameTime);
            }
            
            foreach (var timeline in singleTimelines)
            {
                timeline.Update(gameTime);
                if (timeline.State == TweenState.Stopped)
                    removeTimelines.Add(timeline);
            }
            
            foreach(var timeline in removeTimelines)
            {
                singleTimelines.Remove(timeline);
            }
            removeTimelines.Clear();
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
        public List<ITweenableProperty> tweeningProperties;
        public float elapsedMilliseconds;
        public bool Loop;
        public float duration;
        public TweenState State;
        public bool AdaptiveDuration;

        public TweenTimeline() : this(0)
        {
            AdaptiveDuration = true;
        }
        public TweenTimeline(float duration)
        {
            this.duration = duration;
            tweeningProperties = new List<ITweenableProperty>();
            State = TweenState.Running;
        }

        public void Start()
        {
            State = TweenState.Running;
        }
        public void Stop()
        {
            elapsedMilliseconds = 0;
            State = TweenState.Stopped;
            ResetProperties();
        }
        public void Pause()
        {
            State = TweenState.Paused;
        }
        public void Restart()
        {
            elapsedMilliseconds = 0;
            State = TweenState.Running;
            ResetProperties();
        }

        /// <summary>
        /// Scans through the timeline backwards, resetting properties to their orginal state.
        /// </summary>
        protected void ResetProperties()
        {
            for (int i = tweeningProperties.Count - 1; i >= 0; i--)
            {
                tweeningProperties[i].Reset();
            }
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
                    tweeningProperties.Add(t);
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
                        tweeningProperties.Add(t);
                        return t;
                    }
                }
            }
            return null;
        }

        public TweenableProperty<T> AddProperty<T>(T initialValue, Action<T> setter, LerpFunction<T> lerpFunction)
        {
            var t = new TweenSetter<T>(initialValue, setter, lerpFunction);
            return t;
        }

        
#if XNA || WINDOWS_PHONE || XBOX || ANDROID || MONOGAME || STRIDE

        public void Update(GameTime gameTime)
        {
            if(AdaptiveDuration)
            {
#if STRIDE
                elapsedMilliseconds += (float)gameTime.Elapsed.Milliseconds;
#else
                elapsedMilliseconds += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
#endif
                
                if(State == TweenState.Running)
                {
                    var propertiesFinished = 0;
                    foreach (var property in tweeningProperties)
                    {
                        if(!property.Update(elapsedMilliseconds)) //No Frames Left
                        {
                            propertiesFinished++;
                        }
                    }
                    if (propertiesFinished == tweeningProperties.Count)
                    {
                        elapsedMilliseconds = 0;
                        if (!Loop)
                        {
                            State = TweenState.Stopped;
                        }
                    }
                }
            }
            else if (State == TweenState.Running)
            {
#if STRIDE
                elapsedMilliseconds += (float)gameTime.Elapsed.Milliseconds;
#else
                elapsedMilliseconds += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
#endif

                if (elapsedMilliseconds >= duration)
                {
                    if (Loop)
                    {
                        elapsedMilliseconds = elapsedMilliseconds - duration;
                        ResetProperties();
                    }
                    else
                    {
                        Stop();
                    }
                }

                if (State == TweenState.Running)
                {
                    foreach (var property in tweeningProperties)
                    {
                        property.Update(elapsedMilliseconds);                        
                    }                    
                }
            }
        }
#endif
        
        public void Update(float deltaTime)
        {
            if (AdaptiveDuration)
            {
                elapsedMilliseconds += deltaTime;
                if (State == TweenState.Running)
                {
                    var propertiesFinished = 0;
                    foreach (var property in tweeningProperties)
                    {
                        if (!property.Update(elapsedMilliseconds)) //No Frames Left
                        {
                            propertiesFinished++;
                        }
                    }
                    if (propertiesFinished == tweeningProperties.Count)
                    {
                        elapsedMilliseconds = 0;
                        if (!Loop)
                        {
                            State = TweenState.Stopped;
                        }
                    }
                }
            }
            else if (State == TweenState.Running)
            {
                elapsedMilliseconds += deltaTime;

                if (elapsedMilliseconds >= duration)
                {
                    if (Loop)
                    {
                        elapsedMilliseconds = elapsedMilliseconds - duration;
                        ResetProperties();
                    }
                    else
                    {
                        Stop();
                    }
                }

                if (State == TweenState.Running)
                {
                    foreach (var property in tweeningProperties)
                    {
                        property.Update(elapsedMilliseconds);
                    }
                }
            }
        }
    }

    public interface ITweenableProperty
    {
        bool Update(float timelineElapsed);
        void Reset();
    }
    public abstract class TweenableProperty<T> : ITweenableProperty
    {
        public T initialValue { get; protected init; }
        protected Action onComplete { get; init; }
        protected List<TweenKeyFrame<T>> keyFrames;
        protected LerpFunction<T> lerpFunction;
        protected bool Done;

        public TweenableProperty(LerpFunction<T> lerpFunction)
        {
            keyFrames = new List<TweenKeyFrame<T>>();
            this.lerpFunction = lerpFunction;
        }

        public TweenableProperty<T> AddFrame(TweenKeyFrame<T> keyFrame)
        {
            keyFrames.Add(keyFrame);
            keyFrames = keyFrames.OrderBy(x => x.frame).ToList();
            return this;
        }

        public TweenableProperty<T> RemoveFrame(TweenKeyFrame<T> keyFrame)
        {
            keyFrames.Remove(keyFrame);
            keyFrames = keyFrames.OrderBy(x => x.frame).ToList();
            return this;
        }

        public bool Update(float timelineElapsed)
        {
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
            else
            {
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
            }

            return true;
            
        }

        public abstract void SetValue(T value);

        public void Reset()
        {
            
        }
    }
    internal class TweenProperty<T> : TweenableProperty<T>
    {
        public object target;
        public PropertyInfo property;

        public TweenProperty(object target, PropertyInfo property, LerpFunction<T> lerpFunction, Action onComplete)
            :base(lerpFunction)
        {
            this.target = target;
            this.property = property;
            this.onComplete = onComplete;
            initialValue = (T)property.GetValue(target);
        }
        
        public override void SetValue(T value)
        {
            property.SetValue(target, value);
        }
    }
    internal class TweenField<T> : TweenableProperty<T>
    {
        public object target;
        public FieldInfo field;

        public TweenField(object target, FieldInfo field, LerpFunction<T> lerpFunction, Action onComplete)
            :base(lerpFunction)
        {
            this.target = target;
            this.field = field;
            this.onComplete = onComplete;
            initialValue = (T)field.GetValue(target);
        }
        
        public override void SetValue(T value)
        {
            field.SetValue(target, value);
        }
    }
    internal class TweenSetter<T> : TweenableProperty<T>
    {
        public Action<T> setter;

        public TweenSetter(T initialValue, Action<T> setter, LerpFunction<T> lerpFunction)
            : base(lerpFunction)
        {
            this.setter = setter;
        }
        
        public override void SetValue(T value)
        {
            setter.Invoke(value);
        }
    }

    public class TweenKeyFrame<T>
    {
        public float frame;
        public T value;
        public EasingFunction easingFunction;

        public TweenKeyFrame(float frame, T value, EasingFunction easingFunction)
        {
            this.frame = frame;
            this.value = value;
            this.easingFunction = easingFunction;
        }
    }

    public class TweenSequence<T>
    {
        public List<TweenKeyFrame<T>> keyFrames = default;
        public LerpFunction<T> lerpFunction = default;
        public System.Action onComplete = null;

        public TweenSequence(List<TweenKeyFrame<T>> keyFrames, LerpFunction<T> lerpFunction, Action onComplete = null)
        {
            this.keyFrames = keyFrames;
            this.lerpFunction = lerpFunction;
            this.onComplete = onComplete;
        }
    }

    public delegate float EasingFunction(float val);
    public delegate T LerpFunction<T>(T start, T end, float progress);
    public static class LerpFunctions
    {
        public static LerpFunction<float> Float = (s, e, p) => s + (e - s) * p;
#if XNA || WINDOWS_PHONE || XBOX || ANDROID || MONOGAME
        public static LerpFunction<Vector2> Vector2 = (s, e, p) => { return Microsoft.Xna.Framework.Vector2.Lerp(s, e, p); };
        public static LerpFunction<Vector3> Vector3 = (s, e, p) => { return Microsoft.Xna.Framework.Vector3.Lerp(s, e, p); };
        public static LerpFunction<Vector4> Vector4 = (s, e, p) => { return Microsoft.Xna.Framework.Vector4.Lerp(s, e, p); };
        public static LerpFunction<Color> Color = (s, e, p) => { return Microsoft.Xna.Framework.Color.Lerp(s, e, p); };
        public static LerpFunction<Quaternion> Quaternion = (s, e, p) => { return Microsoft.Xna.Framework.Quaternion.Lerp(s, e, p); };
        public static LerpFunction<Rectangle> Rectangle = (s, e, p) =>
            {
                var pX = s.X + (e.X - s.X) * p;
                var pY = s.Y + (e.Y - s.Y) * p;
                var width = s.Width + (e.Width - s.Width) * p;
                var height = s.Height + (e.Height - s.Height) * p;
                return new Microsoft.Xna.Framework.Rectangle((int)pX, (int)pY, (int)width, (int)height);
            };
#elif STRIDE
        public static readonly LerpFunction<Vector2>    Vector2     = Stride.Core.Mathematics.Vector2.Lerp;
        public static readonly LerpFunction<Vector3>    Vector3     = Stride.Core.Mathematics.Vector3.Lerp;
        public static readonly LerpFunction<Vector4>    Vector4     = Stride.Core.Mathematics.Vector4.Lerp;
        public static readonly LerpFunction<Color>      Color       = Stride.Core.Mathematics.Color.Lerp;
        public static readonly LerpFunction<Quaternion> Quaternion  = Stride.Core.Mathematics.Quaternion.Lerp;
        public static readonly LerpFunction<Rectangle>  Rectangle   = (s, e, p) =>
        {
            var pX = s.X + (e.X - s.X) * p;
            var pY = s.Y + (e.Y - s.Y) * p;
            var width = s.Width + (e.Width - s.Width) * p;
            var height = s.Height + (e.Height - s.Height) * p;
            return new Stride.Core.Mathematics.Rectangle((int)pX, (int)pY, (int)width, (int)height);
        };  
#endif
    }


}
