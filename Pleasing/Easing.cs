using System;

namespace Pleasing
{
    public enum EasingType
    {
        Linear,
        QuadraticIn,
        QuadraticOut,
        QuadraticInOut,
        CubicIn,
        CubicOut,
        CubicInOut,
        QuarticIn,
        QuarticOut,
        QuarticInOut,
        QuinticIn,
        QuinticOut,
        QuinticInOut,
        SinusoidalIn,
        SinusoidalOut,
        SinusoidalInOut,
        ExponentialIn,
        ExponentialOut,
        ExponentialInOut,
        CircularIn,
        CircularOut,
        CircularInOut,
        ElasticIn,
        ElasticOut,
        ElasticInOut,
        BackIn,
        BackOut,
        BackInOut,
        BounceIn,
        BounceOut,
        BounceInOut,
        Bezier
    }

    public static class Easing
    {
        internal static EasingFunction GetEasingFunction(EasingType easingType)
		{
			switch (easingType)
			{
				case EasingType.Linear:
					return Linear;
				case EasingType.QuadraticIn:
					return Quadratic.In;
				case EasingType.QuadraticOut:
					return Quadratic.Out;
				case EasingType.QuadraticInOut:
					return Quadratic.InOut;
				case EasingType.CubicIn:
					return Cubic.In;
				case EasingType.CubicOut:
					return Cubic.Out;
				case EasingType.CubicInOut:
					return Cubic.InOut;
				case EasingType.QuarticIn:
					return Quartic.In;
				case EasingType.QuarticOut:
					return Quartic.Out;
				case EasingType.QuarticInOut:
					return Quartic.InOut;
				case EasingType.QuinticIn:
					return Quintic.In;
				case EasingType.QuinticOut:
					return Quintic.Out;
				case EasingType.QuinticInOut:
					return Quintic.InOut;
				case EasingType.SinusoidalIn:
					return Sinusoidal.In;
				case EasingType.SinusoidalOut:
					return Sinusoidal.Out;
				case EasingType.SinusoidalInOut:
					return Sinusoidal.InOut;
				case EasingType.ExponentialIn:
					return Exponential.In;
				case EasingType.ExponentialOut:
					return Exponential.Out;
				case EasingType.ExponentialInOut:
					return Exponential.InOut;
				case EasingType.CircularIn:
					return Circular.In;
				case EasingType.CircularOut:
					return Circular.Out;
				case EasingType.CircularInOut:
					return Circular.InOut;
				case EasingType.ElasticIn:
					return Elastic.In;
				case EasingType.ElasticOut:
					return Elastic.Out;
				case EasingType.ElasticInOut:
					return Elastic.InOut;
				case EasingType.BackIn:
					return Back.In;
				case EasingType.BackOut:
					return Back.Out;
				case EasingType.BackInOut:
					return Back.InOut;
				case EasingType.BounceIn:
					return Bounce.In;
				case EasingType.BounceOut:
					return Bounce.Out;
				case EasingType.BounceInOut:
					return Bounce.InOut;
				case EasingType.Bezier:
				default:
					throw new ArgumentException($"No Easing Functioned defined for Type: {easingType}");
			}
		}

        public static float Linear(float k)
        {
            return k;
        }

        public class Quadratic
        {
            public static float In(float k)
            {
                return k * k;
            }

            public static float Out(float k)
            {
                return k * (2f - k);
            }

            public static float InOut(float k)
            {
                if ((k *= 2f) < 1f) return 0.5f * k * k;
                return -0.5f * ((k -= 1f) * (k - 2f) - 1f);
            }
        };

        public class Cubic
        {
            public static float In(float k)
            {
                return k * k * k;
            }

            public static float Out(float k)
            {
                return 1f + ((k -= 1f) * k * k);
            }

            public static float InOut(float k)
            {
                if ((k *= 2f) < 1f) return 0.5f * k * k * k;
                return 0.5f * ((k -= 2f) * k * k + 2f);
            }
        };

        public class Quartic
        {
            public static float In(float k)
            {
                return k * k * k * k;
            }

            public static float Out(float k)
            {
                return 1f - ((k -= 1f) * k * k * k);
            }

            public static float InOut(float k)
            {
                if ((k *= 2f) < 1f) return 0.5f * k * k * k * k;
                return -0.5f * ((k -= 2f) * k * k * k - 2f);
            }
        };

        public class Quintic
        {
            public static float In(float k)
            {
                return k * k * k * k * k;
            }

            public static float Out(float k)
            {
                return 1f + ((k -= 1f) * k * k * k * k);
            }

            public static float InOut(float k)
            {
                if ((k *= 2f) < 1f) return 0.5f * k * k * k * k * k;
                return 0.5f * ((k -= 2f) * k * k * k * k + 2f);
            }
        };

        public class Sinusoidal
        {
            public static float In(float k)
            {
                return 1f - (float)Math.Cos(k * Math.PI / 2f);
            }

            public static float Out(float k)
            {
                return (float)Math.Sin(k * Math.PI / 2f);
            }

            public static float InOut(float k)
            {
                return 0.5f * (1f - (float)Math.Cos(Math.PI * k));
            }
        };

        public class Exponential
        {
            public static float In(float k)
            {
                return k == 0f ? 0f : (float)Math.Pow(1024f, k - 1f);
            }

            public static float Out(float k)
            {
                return k == 1f ? 1f : 1f - (float)Math.Pow(2f, -10f * k);
            }

            public static float InOut(float k)
            {
                if (k == 0f) return 0f;
                if (k == 1f) return 1f;
                if ((k *= 2f) < 1f) return 0.5f * (float)Math.Pow(1024f, k - 1f);
                return 0.5f * (float)(-Math.Pow(2f, -10f * (k - 1f)) + 2f);
            }
        };

        public class Circular
        {
            public static float In(float k)
            {
                return 1f - (float)Math.Sqrt(1f - k * k);
            }

            public static float Out(float k)
            {
                return (float)Math.Sqrt(1f - ((k -= 1f) * k));
            }

            public static float InOut(float k)
            {
                if ((k *= 2f) < 1f) return -0.5f * (float)(Math.Sqrt(1f - k * k) - 1);
                return 0.5f * (float)(Math.Sqrt(1f - (k -= 2f) * k) + 1f);
            }
        };

        public class Elastic
        {
            public static float In(float k)
            {
                if (k == 0) return 0;
                if (k == 1) return 1;
                return (float)-Math.Pow(2f, 10f * (k -= 1f)) * (float)Math.Sin((k - 0.1f) * (2f * Math.PI) / 0.4f);
            }

            public static float Out(float k)
            {
                if (k == 0) return 0;
                if (k == 1) return 1;
                return (float)Math.Pow(2f, -10f * k) * (float)Math.Sin((k - 0.1f) * (2f * Math.PI) / 0.4f) + 1f;
            }

            public static float InOut(float k)
            {
                if ((k *= 2f) < 1f) return -0.5f * (float)Math.Pow(2f, 10f * (k -= 1f)) * (float)Math.Sin((k - 0.1f) * (2f * Math.PI) / 0.4f);
                return (float)Math.Pow(2f, -10f * (k -= 1f)) * (float)Math.Sin((k - 0.1f) * (2f * Math.PI) / 0.4f) * 0.5f + 1f;
            }
        };

        public class Back
        {
            static float s = 1.70158f;
            static float s2 = 2.5949095f;

            public static float In(float k)
            {
                return k * k * ((s + 1f) * k - s);
            }

            public static float Out(float k)
            {
                return (k -= 1f) * k * ((s + 1f) * k + s) + 1f;
            }

            public static float InOut(float k)
            {
                if ((k *= 2f) < 1f) return 0.5f * (k * k * ((s2 + 1f) * k - s2));
                return 0.5f * ((k -= 2f) * k * ((s2 + 1f) * k + s2) + 2f);
            }
        };

        public class Bounce
        {
            public static float In(float k)
            {
                return 1f - Out(1f - k);
            }

            public static float Out(float k)
            {
                if (k < (1f / 2.75f))
                {
                    return 7.5625f * k * k;
                }
                else if (k < (2f / 2.75f))
                {
                    return 7.5625f * (k -= (1.5f / 2.75f)) * k + 0.75f;
                }
                else if (k < (2.5f / 2.75f))
                {
                    return 7.5625f * (k -= (2.25f / 2.75f)) * k + 0.9375f;
                }
                else
                {
                    return 7.5625f * (k -= (2.625f / 2.75f)) * k + 0.984375f;
                }
            }

            public static float InOut(float k)
            {
                if (k < 0.5f) return In(k * 2f) * 0.5f;
                return Out(k * 2f - 1f) * 0.5f + 0.5f;
            }
        };


        /// <summary>
        /// Implementation adapted from http://www.flong.com/texts/code/shapers_bez/
        /// </summary>
        /// <param name="time">The value to ease</param>
        /// <param name="aX"></param>
        /// <param name="aY"></param>
        /// <param name="bX"></param>
        /// <param name="bY"></param>
        /// <returns></returns>
        public static float Bezier(float time, float aX, float aY, float bX, float bY)
        {
            float y0a = 0.00f; // initial y
            float x0a = 0.00f; // initial x 
            float y1a = aY;    // 1st influence y   
            float x1a = aX;    // 1st influence x 
            float y2a = bY;    // 2nd influence y
            float x2a = bX;    // 2nd influence x
            float y3a = 1.00f; // final y 
            float x3a = 1.00f; // final x 

            float A = x3a - 3 * x2a + 3 * x1a - x0a;
            float B = 3 * x2a - 6 * x1a + 3 * x0a;
            float C = 3 * x1a - 3 * x0a;
            float D = x0a;

            float E = y3a - 3 * y2a + 3 * y1a - y0a;
            float F = 3 * y2a - 6 * y1a + 3 * y0a;
            float G = 3 * y1a - 3 * y0a;
            float H = y0a;

            // Solve for t given x (using Newton-Raphelson), then solve for y given t.
            // Assume for the first guess that t = x.
            float currentt = time;
            int nRefinementIterations = 5;
            for (int i = 0; i < nRefinementIterations; i++)
            {
                float currentx = xFromT(currentt, A, B, C, D);
                float currentslope = slopeFromT(currentt, A, B, C);
                currentt -= (currentx - time) * (currentslope);
                currentt = Constrain(currentt, 0, 1);
            }

            float y = yFromT(currentt, E, F, G, H);
            return y;
        }

        public static Func<float, float> BezierFunction(float aX, float aY, float bX, float bY)
        {
            return (x) => Bezier(x, aX, aY, bX, bY);
        }

        //Helper functions for Bezier
        private static float Constrain(float value, float min, float max)
        {
            return value < min ? min : value > max ? max : value;
        }
        private static float slopeFromT(float t, float A, float B, float C)
        {
            float dtdx = 1.0f / (3.0f * A * t * t + 2.0f * B * t + C);
            return dtdx;
        }
        private static float xFromT(float t, float A, float B, float C, float D)
        {
            float x = A * (t * t * t) + B * (t * t) + C * t + D;
            return x;
        }
        private static float yFromT(float t, float E, float F, float G, float H)
        {
            float y = E * (t * t * t) + F * (t * t) + G * t + H;
            return y;
        }
    }
}
