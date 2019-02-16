using System;

namespace GameServer.Model
{
    public class Position
    {
        private float[] _array = new float[3];

        public Position()
        {
        }

        public Position(float x, float y, float z) : this(x, y, z, 0)
        {
        }

        public Position(float x, float y, float z, short heading)
        {
            X = x;
            Y = y;
            Z = z;
            Heading = heading;
        }

        public float X
        {
            get { return _array[0]; }
            set { _array[0] = value; }
        }

        public float Y
        {
            get { return _array[1]; }
            set { _array[1] = value; }
        }

        public float Z
        {
            get { return _array[2]; }
            set { _array[2] = value; }
        }

        public short Heading { get; set; }

        public float[] Array
        {
            get { return _array; }
            set { _array = value; }
        }

        public float DistanceTo(Position pos)
        {
            return (float)Math.Sqrt(SqrDistanceTo(pos));
        }

        public float SqrDistanceTo(Position pos)
        {
            return (pos.X - X) * (pos.X - X) + (pos.Y - Y) * (pos.Y - Y);
        }

        public void XYZ(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public void OffsetByAngle(float angle, float offset)
        {
            X += (float)Math.Cos(angle) * offset;
            Y += (float)Math.Sin(angle) * offset;
        }

      

        public override string ToString()
        {
            return String.Format("[{0:0.000}, {1:0.000}, {2:0.000}]", X, Y, Z);
        }
    }
}