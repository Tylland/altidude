using System;
using System.Collections.Generic;
using System.Text;

namespace Altidude.Files.Shp
{
  public class ByteHolder : Shape
  {
    private byte[] _bytes = new byte[0];

    public ByteHolder(ShpReader reader)
      : base(reader)
    {
      _bytes = reader.ReadBytes(Header.Length);
    }
  }
}
