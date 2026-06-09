using System.Numerics;
using ZGeometry.Primitives.Circle;
using ZGeometry.Primitives.Line;
using ZGeometry.Primitives.Point;
using ZGeometry.Primitives.Rectangle;
using ZGeometry.Primitives.Triangle;
using ZGeometry.Utils;

namespace ZGeometry.Logic;

public static partial class Geometry
{
    /// <summary>Returns the point if the two points coincide; otherwise an empty sequence.</summary>
    /// <typeparam name="T1">The numeric type of the vector components.</typeparam>
    public static IEnumerable<Vector2D<T1>> Intersects<T1>(Vector2D<T1> point1, Vector2D<T1> point2)
        where T1 : struct, INumber<T1> => Contains(point1, point2) ? [point1] : [];

    /// <summary>Returns the point if it lies on the line; otherwise an empty sequence.</summary>
    /// <typeparam name="T1">The numeric type of the vector components.</typeparam>
    public static IEnumerable<Vector2D<T1>> Intersects<T1>(Vector2D<T1> point, Line<T1> line)
        where T1 : struct, INumber<T1> => Contains(point, line) ? [point] : [];

    /// <summary>Returns the point if it lies on the circle's boundary; otherwise an empty sequence.</summary>
    /// <typeparam name="T1">The numeric type of the vector components.</typeparam>
    public static IEnumerable<Vector2D<T1>> Intersects<T1>(Vector2D<T1> point, Circle<T1> circle)
        where T1 : struct, INumber<T1>
        => T1.Abs((point - circle.Center).MagnitudeSquared() - (circle.Radius * circle.Radius)) <= T1.CreateChecked(Constants.Epsilon)
            ? [point]
            : [];

    /// <summary>Returns the point if it lies on any edge of the rectangle; otherwise an empty sequence.</summary>
    /// <typeparam name="T1">The numeric type of the vector components.</typeparam>
    public static IEnumerable<Vector2D<T1>> Intersects<T1>(Vector2D<T1> point, Rectangle<T1> rectangle)
        where T1 : struct, INumber<T1>
    {
        for (var i = 0; i < rectangle.SideCount; i++)
            if (Contains(point, rectangle.Side(i)))
                return [point];

        return [];
    }

    /// <summary>Returns the point if it lies on any edge of the triangle; otherwise an empty sequence.</summary>
    /// <typeparam name="T1">The numeric type of the vector components.</typeparam>
    public static IEnumerable<Vector2D<T1>> Intersects<T1>(Vector2D<T1> point, Triangle<T1> triangle)
        where T1 : struct, INumber<T1>
    {
        for (var i = 0; i < Triangle<T1>.SideCount; i++)
            if (Contains(point, triangle.Side(i)))
                return [point];

        return [];
    }
}