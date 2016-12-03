using System;
using System.Collections.Generic;
using System.Text;

namespace Altidude.Files.Shp
{
  public class FileHeader
  {
    private int _fileCode = 9994;

    public int FileCode
    {
      get { return _fileCode; }
    }

    private int _unused1 = 0;
    private int _unused2 = 0;
    private int _unused3 = 0;
    private int _unused4 = 0;
    private int _unused5 = 0;

    private int _fileLength = 0;

    public int FileLength
    {
      get { return _fileLength; }
      set { _fileLength = value; }
    }

    private int _version = 1000;

    public int Version
    {
      get { return _version; }
      set { _version = value; }
    }

    private int _shapeType = 0;

    public ShapeType ShapeType
    {
      get { return (ShapeType)_shapeType; }
      set { _shapeType = (int)value; }
    }

    private double _xMin = 0;

    public double XMin
    {
      get { return _xMin; }
      set { _xMin = value; }
    }
    private double _yMin = 0;

    public double YMin
    {
      get { return _yMin; }
      set { _yMin = value; }
    }
    private double _xMax = 0;

    public double XMax
    {
      get { return _xMax; }
      set { _xMax = value; }
    }
    private double _yMax = 0;

    public double YMax
    {
      get { return _yMax; }
      set { _yMax = value; }
    }
    private double _zMin = 0;
    private double _zMax = 0;
    private double _mMin = 0;
    private double _mMax = 0;

    public void Save(ShpWriter writer)
    {
      writer.WriteBigEndian(_fileCode);
      writer.WriteBigEndian(_unused1);
      writer.WriteBigEndian(_unused2);
      writer.WriteBigEndian(_unused3);
      writer.WriteBigEndian(_unused4);
      writer.WriteBigEndian(_unused5);
      writer.WriteBigEndian(_fileLength);
      writer.Write(_version);
      writer.Write(_shapeType);
      writer.Write(_xMin);
      writer.Write(_yMin);
      writer.Write(_xMax);
      writer.Write(_yMax);
      writer.Write(_zMin);
      writer.Write(_zMax);
      writer.Write(_mMin);
      writer.Write(_mMax);

    }


    public FileHeader(ShpReader reader)
    {
      _fileCode = reader.ReadIntBigEndian();
      _unused1 = reader.ReadIntBigEndian();
      _unused2 = reader.ReadIntBigEndian();
      _unused3 = reader.ReadIntBigEndian();
      _unused4 = reader.ReadIntBigEndian();
      _unused5 = reader.ReadIntBigEndian();
      _fileLength = reader.ReadIntBigEndian();
      _version = reader.ReadInt();
      _shapeType = reader.ReadInt();
      _xMin = reader.ReadDouble();
      _yMin = reader.ReadDouble();
      _xMax = reader.ReadDouble();
      _yMax = reader.ReadDouble();
      _zMin = reader.ReadDouble();
      _zMax = reader.ReadDouble();
      _mMin = reader.ReadDouble();
      _mMax = reader.ReadDouble();

      reader.AssertPosition(100);
    }

    public FileHeader(){}

  }
}
