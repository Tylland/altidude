using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;

namespace Altidude.Files.Shp
{
  public class Polygon : Shape
  {
    private int _shapeType = (int)ShapeType.Polygon;

    public ShapeType ShapeType
    {
      get { return (ShapeType)_shapeType; }
    }

    private BoundingBox _box = BoundingBox.Empty;

    public BoundingBox Box
    {
      get { return _box; }
      set { _box = value; }
    }

    private int _numParts = 0;

    public int NumParts
    {
      get { return _numParts; }
      set { _numParts = value; }
    }

    private int _numPoints = 0;

    public int NumPoints
    {
      get { return _numPoints; }
      set { _numPoints = value; }
    }

    private int[] _parts = new int[0];

    public int[] Parts
    {
      get { return _parts; }
      set { _parts = value; }
    }

    private Point[] _points = new Point[0];

    public Point[] Points
    {
      get { return _points; }
      set { _points = value; }
    }

    public void AddPoints(Point[] points)
    {
      List<int> partList = new List<int>(_parts);
      List<Point> pointList = new List<Point>(_points);

      partList.Add(_points.Length);
      pointList.AddRange(points);

      _numParts = partList.Count;
      _numPoints = pointList.Count;
      _parts = partList.ToArray();
      _points = pointList.ToArray();

      _box.Update(_points);
    }

    public override int GetContentLength()
    {
      return (2 + 16 + 2 + 2 + _numParts * 2 + _numPoints * 8);
    }

    public override void Save(ShpWriter writer)
    {
      base.Save(writer);

      writer.Write(_shapeType);

      _box.Save(writer);

      writer.Write(_numParts);
      writer.Write(_numPoints);
      writer.Write(_parts);
      writer.Write(_points);
    }

    public Polygon()
    {
      _box = new BoundingBox();
    }

    public Polygon(ShpReader reader)
      : base(reader)
    {
      _shapeType = reader.ReadInt();
      Debug.Assert(_shapeType == 5);

      _box = new BoundingBox(reader);
      _numParts = reader.ReadInt();
      _numPoints = reader.ReadInt();
      _parts = reader.ReadInts(_numParts);
      _points = reader.ReadPoints(_numPoints); 
    }
  }
}
