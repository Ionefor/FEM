using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MKE
{
    public struct PointD : IEquatable<PointD>
    {
        /// <summary>
        /// Creates a new instance of the <see cref='FEM.PointD'/> class with member data left uninitialized.
        /// </summary>
        public static readonly PointD Empty;
        private double x; // Do not rename (binary serialization)
        private double y; // Do not rename (binary serialization)

        /// <summary>
        /// Initializes a new instance of the <see cref='FEM.PointD'/> class with the specified coordinates.
        /// </summary>
        public PointD(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref='FEM.PointD'/> struct from the specified
        /// <see cref="OpenTK.Mathematics.Vector2d"/>.
        /// </summary>
        public PointD(Vector2d vector)
        {
            x = vector.X;
            y = vector.Y;
        }

        /// <summary>
        /// Creates a new <see cref="OpenTK.Mathematics.Vector2d"/> from this <see cref="FEM.PointD"/>.
        /// </summary>

        public readonly Vector2d ToVector2d() => new(x, y);
        /// <summary>
        /// Gets a value indicating whether this <see cref='FEM.PointD'/> is empty.
        /// </summary>
        [Browsable(false)]
        public readonly bool IsEmpty => x == 0d && y == 0d;

        /// <summary>
        /// Gets the x-coordinate of this <see cref='FEM.PointD'/>.
        /// </summary>
        public double X
        {
            readonly get => x;
            set => x = value;
        }

        /// <summary>
        /// Gets the y-coordinate of this <see cref='FEM.PointD'/>.
        /// </summary>
        public double Y
        {
            readonly get => y;
            set => y = value;
        }

        /// <summary>
        /// Converts the specified <see cref="FEM.PointD"/> to a <see cref="OpenTK.Mathematics.Vector2d"/>.
        /// </summary>


        /// <summary>
        /// Converts the specified <see cref="OpenTK.Mathematics.Vector2d"/> to a <see cref="FEM.PointD"/>.
        /// </summary>
        public static explicit operator PointD(Vector2d vector) => new(vector);

        /// <summary>
        /// Translates a <see cref='FEM.PointD'/> by a given <see cref='System.Drawing.Size'/> .
        /// </summary>
        public static PointD operator +(PointD pt, Size sz) => Add(pt, sz);

        /// <summary>
        /// Translates a <see cref='FEM.PointD'/> by the negative of a given <see cref='System.Drawing.Size'/> .
        /// </summary>
        public static PointD operator -(PointD pt, Size sz) => Subtract(pt, sz);

        /// <summary>
        /// Translates a <see cref='FEM.PointD'/> by a given <see cref='System.Drawing.SizeF'/> .
        /// </summary>
        public static PointD operator +(PointD pt, SizeF sz) => Add(pt, sz);

        /// <summary>
        /// Translates a <see cref='FEM.PointD'/> by the negative of a given <see cref='System.Drawing.SizeF'/> .
        /// </summary>
        public static PointD operator -(PointD pt, SizeF sz) => Subtract(pt, sz);

        /// <summary>
        /// Compares two <see cref='FEM.PointD'/> objects. The result specifies whether the values of the
        /// <see cref='FEM.PointD.X'/> and <see cref='FEM.PointD.Y'/> properties of the two
        /// <see cref='FEM.PointD'/> objects are equal.
        /// </summary>
        public static bool operator ==(PointD left, PointD right) => left.X == right.X && left.Y == right.Y;

        /// <summary>
        /// Compares two <see cref='FEM.PointD'/> objects. The result specifies whether the values of the
        /// <see cref='FEM.PointD.X'/> or <see cref='FEM.PointD.Y'/> properties of the two
        /// <see cref='FEM.PointD'/> objects are unequal.
        /// </summary>
        public static bool operator !=(PointD left, PointD right) => !(left == right);

        /// <summary>
        /// Translates a <see cref='FEM.PointD'/> by a given <see cref='System.Drawing.Size'/> .
        /// </summary>
        public static PointD Add(PointD pt, Size sz) => new(pt.X + sz.Width, pt.Y + sz.Height);

        /// <summary>
        /// Translates a <see cref='FEM.PointD'/> by the negative of a given <see cref='System.Drawing.Size'/> .
        /// </summary>
        public static PointD Subtract(PointD pt, Size sz) => new(pt.X - sz.Width, pt.Y - sz.Height);

        /// <summary>
        /// Translates a <see cref='FEM.PointD'/> by a given <see cref='System.Drawing.SizeF'/> .
        /// </summary>
        public static PointD Add(PointD pt, SizeF sz) => new(pt.X + sz.Width, pt.Y + sz.Height);

        /// <summary>
        /// Translates a <see cref='FEM.PointD'/> by the negative of a given <see cref='System.Drawing.SizeF'/> .
        /// </summary>
        public static PointD Subtract(PointD pt, SizeF sz) => new(pt.X - sz.Width, pt.Y - sz.Height);

        public override readonly bool Equals([NotNullWhen(true)] object? obj) => obj is PointD && Equals((PointD)obj);

        public readonly bool Equals(PointD other) => this == other;

        public override readonly int GetHashCode() => HashCode.Combine(X.GetHashCode(), Y.GetHashCode());

        public override readonly string ToString() => $"{{X={x}, Y={y}}}";
    }
}
