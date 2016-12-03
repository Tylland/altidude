using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Altidude.Files.Shp
{
  public class ShpFile
  {
    private FileHeader _header = new FileHeader();

    public FileHeader Header
    {
      get { return _header; }
      set { _header = value; }
    }

  }

  public class ShpMainFile : ShpFile
  {
    private List<Shape> _shapes = new List<Shape>();

    public List<Shape> Shapes
    {
      get { return _shapes; }
    }

    private int GetFileLength()
    {
      int fileLength = 50;

      foreach (Shape shape in _shapes)
        fileLength += 4 + shape.GetContentLength();

      return fileLength;
    }

    public void Save(string path)
    {
      Header.FileLength = GetFileLength();

      using (FileStream stream = File.OpenWrite(path))
      {
        using (ShpWriter writer = new ShpWriter(stream))
        {
          Header.Save(writer);

          int number = 1;
          foreach (Shape shape in _shapes)
          {
            shape.Header.Number = number;
            shape.Save(writer);
            number++;
          }
        }
      }
    }

    public void SaveIndex(string path)
    {
      int fileLength = Header.FileLength;

      try
      {
        Header.FileLength = 50 + _shapes.Count * 4;

        using (FileStream stream = File.OpenWrite(path))
        {
          using (ShpWriter writer = new ShpWriter(stream))
          {
            Header.Save(writer);

            int offset = 50;
            foreach (Shape shape in _shapes)
            {
              new IndexRecord(offset, shape.Header.Length).Save(writer);
              offset += 4 + shape.Header.Length;
            }
          }
        }
      }
      finally
      {
        Header.FileLength = fileLength;
      }
    }


    public ShpMainFile(ShapeType shapeType)
    {
      Header.ShapeType = shapeType;
    }

    public ShpMainFile(string path)
    {
      using (FileStream stream = File.OpenRead(path))
      {
        Read(stream);
      }
    }

    public ShpMainFile(Stream stream)
    {
      Read(stream);
    }

    private void Read(Stream stream)
    {
      using (ShpReader reader = new ShpReader(stream))
      {
        Header = new FileHeader(reader);

        while (reader.Position < (Header.FileLength*2))
        {
          if (Header.ShapeType == ShapeType.Polygon)
            _shapes.Add(new Polygon(reader));
          else
            new ByteHolder(reader);
        }
      }
    }
  }
}
