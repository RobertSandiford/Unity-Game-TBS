using UnityEditor;
using UnityEngine;
using System;

public static class Funcs
{
    public static string Percentage(double d)
    {
        d *= 100;
        d = Math.Round(d);
        return d.ToString() + "%";
    }
}
