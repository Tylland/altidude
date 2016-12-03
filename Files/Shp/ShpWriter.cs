using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Altidude.Files.Shp
{
  public class ShpWriter : ShpIO, IDisposable 
  {
    private BinaryWriter _writer = null;

    internal void WriteBigEndian(int value)
    {
      _writer.Write(SwapEndian(value));
    }

    internal void Write(int value)
    {
      _writer.Write(value);
    }

    internal void Write(double value)
    {
      _writer.Write(value);
    }

    internal void Write(int[] ints)
    {
      for (int i = 0; i < ints.Length; i++)
        Write(ints[i]);
    }

    internal void Write(Point point)
    {
      Write((double)point.X);
      Write((double)point.Y); 
    }

    internal void Write(Point[] points)
    {
      for (int i = 0; i < points.Length; i++)
        Write(points[i]);
    }

    internal ShpWriter(Stream stream)
    {
      _writer = new BinaryWriter(stream);
    }

    public void Dispose()
    {
      _writer.Close();
    }


  }
}
