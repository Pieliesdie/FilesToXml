using System;

namespace b2xtranslator.doc.WordprocessingMLMapping;

public class MappingException : Exception
{
    public MappingException(string message)
        : base(message) { }
}