using System.Numerics;

namespace ZGeometry.Primitives.Point;

/// <summary>
/// A generic 2D vector with arithmetic operators and common vector math helpers.
/// </summary>
/// <typeparam name="T">The numeric component type.</typeparam>
/// <param name="x">The initial X component.</param>
/// <param name="y">The initial Y component.</param>
public struct Vector2D<T>(T x, T y) : IEquatable<Vector2D<T>>
    where T : struct, INumber<T>
{
    /// <summary>
    /// The X position of the vector.
    /// </summary>
    public T X { get; set; } = x;
    
    /// <summary>
    /// The Y position of the vector.
    /// </summary>
    public T Y { get; set; } = y;

    
    /// <summary>
    /// Creates a <see cref="Vector2D{T}"/> with 0 as X and 0 as Y.
    /// </summary>
    public Vector2D() : this(T.Zero, T.Zero)
    {
    }
    
    /// <summary>
    /// Creates of copy of a vector
    /// </summary>
    /// <param name="vector">The <see cref="Vector2D{T}"/> to copy</param>
    public Vector2D(Vector2D<T> vector) : this(vector.X, vector.Y)
    {
    }
    
    /// <summary>
    /// Creates a <see cref="Vector2D{T}"/>.
    /// </summary>
    /// <param name="x">The X position of the vector.</param>
    /// <param name="y">The Y position of the vector.</param>
    /// <returns>A <see cref="Vector2D{T}"/> with the given coordinates.</returns>
    public static Vector2D<T> Create(T x, T y) => new(x, y);
    
    /// <summary>
    /// Creates a <see cref="Vector2D{T}"/>.
    /// </summary>
    /// <returns>A <see cref="Vector2D{T}"/> with the (0, 0) coordinates.</returns>
    public static Vector2D<T> Create() => new();
    
    /// <summary>
    /// Returns the rectangular area of the vector.
    /// </summary>
    public T Area() => X * Y;

    /// <summary>
    /// Returns the magnitude of the vector.
    /// </summary>
    public T Magnitude() => T.CreateChecked(Math.Sqrt(Convert.ToDouble(X * X + Y * Y)));

    /// <summary>
    /// Returns the magnitude squared of the vector.
    /// </summary>
    public T MagnitudeSquared() => X * X + Y * Y;

    /// <summary>
    /// Returns the normalized version of the vector.
    /// </summary>
    public Vector2D<T> Normalize()
    {
        var magnitude = Magnitude();
        return new Vector2D<T>(X / magnitude, Y / magnitude);
    }

    /// <summary>
    /// Returns the vector at 90 degrees to this one.
    /// </summary>
    public Vector2D<T> Perpendicular() => new(-Y, X);

    /// <summary>
    /// Rounds both components down.
    /// </summary>
    public Vector2D<T> Floor() => new(T.CreateChecked(Math.Floor(Convert.ToDouble(X))), T.CreateChecked(Math.Floor(Convert.ToDouble(Y))));

    /// <summary>
    /// Rounds both components up.
    /// </summary>
    public Vector2D<T> Ceil() => new(T.CreateChecked(Math.Ceiling(Convert.ToDouble(X))), T.CreateChecked(Math.Ceiling(Convert.ToDouble(Y))));

    /// <summary>
    /// Returns the element-wise maximum of this and another vector.
    /// </summary>
    public Vector2D<T> Max(Vector2D<T> v) => new(T.Max(X, v.X), T.Max(Y, v.Y));

    /// <summary>
    /// Returns the element-wise minimum of this and another vector.
    /// </summary>
    public Vector2D<T> Min(Vector2D<T> v) => new(T.Min(X, v.X), T.Min(Y, v.Y));

    /// <summary>
    /// Calculates the scalar dot product between this and another vector.
    /// </summary>
    public T DotProduct(Vector2D<T> rhs) => X * rhs.X + Y * rhs.Y;

    /// <summary>
    /// Calculates the scalar cross product between this and another vector.
    /// </summary>
    public T CrossProduct(Vector2D<T> rhs) => X * rhs.Y - Y * rhs.X;

    /// <summary>
    /// Treats this vector as a polar coordinate (R, Theta) and returns the Cartesian equivalent (X, Y).
    /// </summary>
    public Vector2D<T> ToCartesian()
    {
        var r = Convert.ToDouble(X);
        var theta = Convert.ToDouble(Y);
        return new Vector2D<T>(T.CreateChecked(Math.Cos(theta) * r), T.CreateChecked(Math.Sin(theta) * r));
    }

    /// <summary>
    /// Treats this vector as a Cartesian coordinate (X, Y) and returns the polar equivalent (R, Theta).
    /// </summary>
    public Vector2D<T> ToPolar() =>
        new(Magnitude(), T.CreateChecked(Math.Atan2(Convert.ToDouble(Y), Convert.ToDouble(X))));

    /// <summary>
    /// Clamps the components of this vector between the element-wise minimum and maximum of two other vectors.
    /// </summary>
    public Vector2D<T> Clamp(Vector2D<T> min, Vector2D<T> max) => Max(min).Min(max);

    /// <summary>
    /// Linearly interpolates between this vector and another vector, given a normalized parameter t.
    /// </summary>
    public Vector2D<T> Lerp(Vector2D<T> v1, T t) =>
        new((T.One - t) * X + t * v1.X,
            (T.One - t) * Y + t * v1.Y);

    /// <summary>
    /// Assuming this vector is incident, given a normal vector, returns the reflection.
    /// </summary>
    public Vector2D<T> Reflect(Vector2D<T> normal)
    {
        var dot = DotProduct(normal);
        return new Vector2D<T>(X - T.CreateChecked(2) * dot * normal.X,
            Y - T.CreateChecked(2) * dot * normal.Y);
    }

    // Operator overloads for arithmetic operations with Vector2D<T>

    /// <summary>Returns whether two vectors are component-wise equal.</summary>
    public static bool operator ==(Vector2D<T> lhs, Vector2D<T> rhs) => lhs.Equals(rhs);

    /// <summary>Returns whether two vectors differ.</summary>
    public static bool operator !=(Vector2D<T> lhs, Vector2D<T> rhs) => !(lhs == rhs);

    /// <summary>Adds two vectors component-wise.</summary>
    public static Vector2D<T> operator +(Vector2D<T> lhs, Vector2D<T> rhs) => new(lhs.X + rhs.X, lhs.Y + rhs.Y);

    /// <summary>Adds a scalar to both components.</summary>
    public static Vector2D<T> operator +(Vector2D<T> lhs, T rhs) => new(lhs.X + rhs, lhs.Y + rhs);

    /// <summary>Adds a scalar to both components.</summary>
    public static Vector2D<T> operator +(T lhs, Vector2D<T> rhs) => new(rhs.X + lhs, rhs.Y + lhs);

    /// <summary>Subtracts two vectors component-wise.</summary>
    public static Vector2D<T> operator -(Vector2D<T> lhs, Vector2D<T> rhs) => new(lhs.X - rhs.X, lhs.Y - rhs.Y);

    /// <summary>Multiplies two vectors component-wise.</summary>
    public static Vector2D<T> operator *(Vector2D<T> lhs, Vector2D<T> rhs) => new(lhs.X * rhs.X, lhs.Y * rhs.Y);

    /// <summary>Divides two vectors component-wise.</summary>
    public static Vector2D<T> operator /(Vector2D<T> lhs, Vector2D<T> rhs) => new(lhs.X / rhs.X, lhs.Y / rhs.Y);

    /// <summary>Scales a vector by a scalar.</summary>
    public static Vector2D<T> operator *(Vector2D<T> lhs, T scalar) => new(lhs.X * scalar, lhs.Y * scalar);

    /// <summary>Scales a vector by a scalar.</summary>
    public static Vector2D<T> operator *(T scalar, Vector2D<T> lhs) => new(lhs.X * scalar, lhs.Y * scalar);

    /// <summary>Divides a vector by a scalar.</summary>
    public static Vector2D<T> operator /(Vector2D<T> lhs, T scalar) => new(lhs.X / scalar, lhs.Y / scalar);

    /// <summary>Negates both components.</summary>
    public static Vector2D<T> operator -(Vector2D<T> lhs) => new(-lhs.X, -lhs.Y);

    /// <summary>Converts an (x, y) tuple to a vector.</summary>
    public static implicit operator Vector2D<T>((T, T) values) => new(values.Item1, values.Item2);

    /// <summary>Returns whether this vector equals another component-wise.</summary>
    public bool Equals(Vector2D<T> other) => X.Equals(other.X) && Y.Equals(other.Y);

    /// <summary>Returns a hash code combining both components.</summary>
    // ReSharper disable NonReadonlyMemberInGetHashCode
    public override int GetHashCode() => HashCode.Combine(X, Y);
    
    /// <summary>
    /// Returns this vector as a string in the form "(x,y)".
    /// </summary>
    public override string ToString() => $"({X},{Y})";
    
    /// <summary>
    /// Compares if this vector is numerically equal to another.
    /// </summary>
    public override bool Equals(object obj)
    {
        if (obj is Vector2D<T> other) return X.Equals(other.X) && Y.Equals(other.Y);

        return false;
    }
}