using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;

namespace Altidude.Files.Shp
{

  public class ShpReader : ShpIO, IDisposable
  {
    private BinaryReader _reader = null;

    internal void AssertPosition(long position)
    {
      Debug.Assert(position == Position, "Förväntad position: " + position.ToString() + " verklig position: " + Position.ToString());
    }

    internal long Position
    {
      get
      {
        return _reader.BaseStream.Position;
      }
    }

    //internal int SwapEndian(int value)
    //{
    //  byte[] bytes = BitConverter.GetBytes(value);
    //  return BitConverter.ToInt32(new byte[] { bytes[3], bytes[2], bytes[1], bytes[0] }, 0);
    //}


    internal byte[] ReadBytes(int count)
    {
      return _reader.ReadBytes(count);
    }

    internal int ReadIntBigEndian()
    {
      return SwapEndian(_reader.ReadInt32());
    }

    internal int ReadInt()
    {
      return _reader.ReadInt32();
    }

    internal int[] ReadInts(int count)
    {
      int[] ints = new int[count];

      for (int i = 0; i < count; i++)
        ints[i] = _reader.ReadInt32();

      return ints;
    }

    internal Point ReadPoint()
    {
      return new Point(ReadDouble(), ReadDouble());
    }

    internal Point[] ReadPoints(int count)
    {
      Point[] points = new Point[count];

      for (int i = 0; i < count; i++)
        points[i] = ReadPoint();

      return points;
    }

    internal double ReadDouble()
    {
      return _reader.ReadDouble();
    }

    internal ShpReader(Stream stream)
    {
      _reader = new BinaryReader(stream);
    }

    public void Dispose()
    {
      _reader.Close();
    }
  }
}
