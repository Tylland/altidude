using System;
using System.Collections.Generic;
using System.Text;

namespace Altidude.Files.Shp
{
  public class ShpIO
  {
    protected int SwapEndian(int value)
    {
      byte[] bytes = BitConverter.GetBytes(value);
      return BitConverter.ToInt32(new byte[] { bytes[3], bytes[2], bytes[1], bytes[0] }, 0);
    }

  }
}
