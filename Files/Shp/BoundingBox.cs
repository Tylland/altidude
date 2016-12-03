using System;
using System.Collections.Generic;
using System.Text;

namespace Altidude.Files.Shp
{
  public class BoundingBox
  {
    private double _xMin = double.MaxValue;

    public double XMin
    {
      get { return _xMin; }
      set { _xMin = value; }
    }

    private double _yMin = double.MaxValue;

    public double YMin
    {
      get { return _yMin; }
      set { _yMin = value; }
    }

    private double _xMax = double.MinValue;

    public double XMax
    {
      get { return _xMax; }
      set { _xMax = value; }
    }

    private double _yMax = double.MinValue;

    public double YMax
    {
      get { return _yMax; }
      set { _yMax = value; }
    }

    public void Union(BoundingBox box)
    {
      _xMin = Math.Min(_xMin, box.XMin);
      _yMin = Math.Min(_yMin, box.YMin);
      _xMax = Math.Max(_xMax, box.XMax);
      _yMax = Math.Max(_yMax, box.YMax);
    }

    public void Update(Point[] points)
    {
      _xMin = double.MaxValue;
      _yMin = double.MaxValue;
      _xMax = double.MinValue;
      _yMax = double.MinValue;

      foreach (Point point in points)
      {
        _xMin = Math.Min(_xMin, point.X);
        _yMin = Math.Min(_yMin, point.Y);
        _xMax = Math.Max(_xMax, point.X);
        _yMax = Math.Max(_yMax, point.Y);
      }
    }

    private static BoundingBox _empty = new BoundingBox();

    public static BoundingBox Empty
    {
      get { return BoundingBox._empty; }
    }

    public void Save(ShpWriter writer)
    {
      writer.Write(_xMin);
      writer.Write(_yMin);
      writer.Write(_xMax);
      writer.Write(_yMax);
    }

    public BoundingBox(ShpReader reader)
    {
      _xMin = reader.ReadDouble();
      _yMin = reader.ReadDouble();
      _xMax = reader.ReadDouble();
      _yMax = reader.ReadDouble();
    }

    public BoundingBox()
    {
    }
  }
}
