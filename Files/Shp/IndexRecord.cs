using System;
using System.Collections.Generic;
using System.Text;

namespace Altidude.Files.Shp
{
  public class IndexRecord
  {
    private int _offset = 0;

    public int Offset
    {
      get { return _offset; }
      set { _offset = value; }
    }

    private int _length = 0;

    public int Length
    {
      get { return _length; }
      set { _length = value; }
    }

    public void Save(ShpWriter writer)
    {
      writer.WriteBigEndian(_offset);
      writer.WriteBigEndian(_length);
    }

    public IndexRecord()
    {
    }

    public IndexRecord(int offset, int length)
    {
      _offset = offset;
      _length = length;
    }

    public IndexRecord(ShpReader reader)
    {
      _offset = reader.ReadIntBigEndian();
      _length = reader.ReadIntBigEndian();
    }

  }
}
