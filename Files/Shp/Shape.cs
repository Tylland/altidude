using System;
using System.Collections.Generic;
using System.Text;

namespace Altidude.Files.Shp
{
  public class Shape
  {
    private ShapeHeader _header = null;

    public ShapeHeader Header
    {
      get { return _header; }
      set { _header = value; }
    }

    public virtual int GetContentLength()
    {
      return 0;
    }

    public virtual void Save(ShpWriter writer)
    {
      _header.Length = GetContentLength();
      _header.Save(writer);
    }

    public Shape()
    {
      _header = new ShapeHeader();
    }

    public Shape(ShpReader reader)
    {
      _header = new ShapeHeader(reader);
    }
  }
}
