﻿using System.Text;

namespace FilesToXml.Core;

public static class Defaults
{
    public static char[] SearchingDelimiters { get; } = [';', '|', '\t', ','];
    public static string Delimiter { get; } = ";";
    public static Encoding Encoding { get; } = Encoding.UTF8;
}