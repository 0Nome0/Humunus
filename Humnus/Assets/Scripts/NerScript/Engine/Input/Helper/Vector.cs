

using System;
using System.Globalization;

namespace UnityEngine
{
    public struct Vector : IEquatable<Vector>
    {
        public float x;

        /// <summary>
        /// Access the /x/  component using [0] respectively.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public float this[int index]
        {
            get {
                switch (index)
                {
                    case 0: return x;
                    default:
                        throw new IndexOutOfRangeException("Invalid Vector index!");
                }
            }

            set {
                switch (index)
                {
                    case 0: x = value; break;
                    default:
                        throw new IndexOutOfRangeException("Invalid Vector index!");
                }
            }
        }

        // Constructs a new vector with given x components.
        public Vector(float x) { this.x = x; }

        // Set x components of an existing Vector.
        public void Set(float newX) { x = newX; }

        // Linearly interpolates between two vectors.
        public static Vector Lerp(Vector a, Vector b, float t)
        {
            t = Mathf.Clamp01(t);
            return new Vector(a.x + (b.x - a.x) * t);
        }

        // Linearly interpolates between two vectors without clamping the interpolant
        public static Vector LerpUnclamped(Vector a, Vector b, float t)
        {
            return new Vector(a.x + (b.x - a.x) * t);
        }

        // Moves a point /current/ towards /target/.
        public static Vector MoveTowards(Vector current, Vector target, float maxDistanceDelta)
        {
            // avoid vector ops because current scripting backends are terrible at inlining
            float dist = target.x - current.x;

            if (dist == 0 || (maxDistanceDelta >= 0 && dist <= maxDistanceDelta))
                return target;

            return new Vector(current.x + maxDistanceDelta);
        }

        // Multiplies two vectors component-wise.
        public static Vector Scale(Vector a, Vector b) { return new Vector(a.x * b.x); }

        // Multiplies every component of this vector by the same component of /scale/.
        public void Scale(Vector scale) { x *= scale.x; }

        // Makes this vector have a ::ref::magnitude of 1.
        public void Normalize() { x = 1.0f; }

        // Returns this vector with a ::ref::magnitude of 1 (RO).
        public Vector normalized { get { return new Vector(1); } }

        /// *listonly*
        public override string ToString() { return string.Format(CultureInfo.InvariantCulture.NumberFormat, "({0:F1})", x); }
        // Returns a nicely formatted string for this vector.
        public string ToString(string format)
        {
            return string.Format(CultureInfo.InvariantCulture.NumberFormat, "({0})", x.ToString(format, CultureInfo.InvariantCulture.NumberFormat));
        }

        // used to allow Vectors to be used as keys in hash tables
        public override int GetHashCode()
        {
            return x.GetHashCode();
        }

        // also required for being able to use Vectors as keys in hash tables
        public override bool Equals(object other)
        {
            if (!(other is Vector)) return false;

            return Equals((Vector)other);
        }

        public bool Equals(Vector other)
        {
            return x == other.x;
        }

        public static Vector Reflect(Vector inDirection, Vector inNormal)
        {
            return new Vector(inDirection.x * -1);
        }

        public static Vector Perpendicular(Vector inDirection)
        {
            return new Vector(0);
        }

        // Dot Product of two vectors.
        public static float Dot(Vector lhs, Vector rhs) { return lhs.x * rhs.x; }

        // Returns the length of this vector (RO).
        public float magnitude { get { return x; } }
        // Returns the squared length of this vector (RO).
        public float sqrMagnitude { get { return x * x; } }

        // Returns the angle in degrees between /from/ and /to/.
        public static float Angle(Vector from, Vector to) { return 0; }

        // Returns the signed angle in degrees between /from/ and /to/. Always returns the smallest possible angle
        public static float SignedAngle(Vector from, Vector to) { return 0; }

        // Returns the distance between /a/ and /b/.
        public static float Distance(Vector a, Vector b)
        {
            float diff = a.x - b.x;
            return diff;
        }

        // Returns a copy of /vector/ with its magnitude clamped to /maxLength/.
        public static Vector ClampMagnitude(Vector vector, float maxLength)
        {
            float mag = vector.x;
            if (mag > maxLength)
            {
                return new Vector(maxLength);
            }
            return vector;
        }

        // [Obsolete("Use Vector.sqrMagnitude")]
        public static float SqrMagnitude(Vector a) { return a.x * a.x; }
        // [Obsolete("Use Vector.sqrMagnitude")]
        public float SqrMagnitude() { return x * x; }

        // Returns a vector that is made from the smallest components of two vectors.
        public static Vector Min(Vector lhs, Vector rhs) { return new Vector(Mathf.Min(lhs.x, rhs.x)); }

        // Returns a vector that is made from the largest components of two vectors.
        public static Vector Max(Vector lhs, Vector rhs) { return new Vector(Mathf.Max(lhs.x, rhs.x)); }

        // Adds two vectors.
        public static Vector operator +(Vector a, Vector b) { return new Vector(a.x + b.x); }
        // Subtracts one vector from another.
        public static Vector operator -(Vector a, Vector b) { return new Vector(a.x - b.x); }
        // Multiplies one vector by another.
        public static Vector operator *(Vector a, Vector b) { return new Vector(a.x * b.x); }
        // Divides one vector over another.
        public static Vector operator /(Vector a, Vector b) { return new Vector(a.x / b.x); }
        // Negates a vector.
        public static Vector operator -(Vector a) { return new Vector(-a.x); }
        // Multiplies a vector by a number.
        public static Vector operator *(Vector a, float d) { return new Vector(a.x * d); }
        // Multiplies a vector by a number.
        public static Vector operator *(float d, Vector a) { return new Vector(a.x * d); }
        // Divides a vector by a number.
        public static Vector operator /(Vector a, float d) { return new Vector(a.x / d); }
        // Returns true if the vectors are equal.
        public static bool operator ==(Vector lhs, Vector rhs)
        {
            // Returns false in the presence of NaN values.
            float diff_x = lhs.x - rhs.x;
            return diff_x < kEpsilon;
        }

        // Returns true if vectors are different.
        public static bool operator !=(Vector lhs, Vector rhs)
        {
            // Returns true in the presence of NaN values.
            return !(lhs == rhs);
        }

        // Converts a [[Vector2]] to a Vector.
        public static implicit operator Vector(Vector2 v)
        {
            return new Vector(v.x);
        }

        // Converts a [[Vector3]] to a Vector.
        public static implicit operator Vector(Vector3 v)
        {
            return new Vector(v.x);
        }

        // Converts a Vector to a [[Vector2]].
        public static implicit operator Vector2(Vector v)
        {
            return new Vector2(v.x, 0);
        }

        // Converts a Vector to a [[Vector3]].
        public static implicit operator Vector3(Vector v)
        {
            return new Vector3(v.x, 0, 0);
        }

        // Shorthand for writing @@Vector(0)@@
        public static Vector zero { get; } = new Vector(0F);
        // Shorthand for writing @@Vector(1)@@
        public static Vector positive { get; } = new Vector(1F);
        // Shorthand for writing @@Vector(-1)@@
        public static Vector negative { get; } = new Vector(-1F);
        // Shorthand for writing @@Vector(float.PositiveInfinity)@@
        public static Vector positiveInfinity { get; } = new Vector(float.PositiveInfinity);
        // Shorthand for writing @@Vector(float.NegativeInfinity)@@
        public static Vector negativeInfinity { get; } = new Vector(float.NegativeInfinity);

        // *Undocumented*
        public const float kEpsilon = 0.00001F;
        // *Undocumented*
        public const float kEpsilonNormalSqrt = 1e-15f;
    }
}