using System;

namespace Sorter.Exceptions
{
  public class BadRequestException : Exception
  {
    public BadRequestException(string message) : base(message)
    {
    }
  }
}
