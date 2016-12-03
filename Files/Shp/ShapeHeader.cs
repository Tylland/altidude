using System;
using System.Collections.Generic;
using System.Text;

namespace Altidude.Files.Shp
{
  public class ShapeHeader
  {
    private int _number = 0;

    public int Number
    {
      get { return _number; }
      set { _number = value; }
    }

    private int _length = 0;

    public int Length
    {
      get { return _length; }
      set { _length = value; }
    }

    public void Save(ShpWriter writer)
    {
      writer.WriteBigEndian(_number);
      writer.WriteBigEndian(_length);
    }

    public ShapeHeader()
    {
    }
    
    public ShapeHeader(ShpReader reader)
    {
      _number = reader.ReadIntBigEndian();
      _length = reader.ReadIntBigEndian();
    }
  }
}
