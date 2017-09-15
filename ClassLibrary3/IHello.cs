///////////////////////////////////////////////////////////////////////////
// IHello.cs - hello class interface                                     //
//                                                                       //
// Jim Fawcett, CSE681 - Software Modeling and Analysis, Fall 2004       //
///////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
namespace TestHarness1
{
    public interface IHello
    {
        int say(List<string> names);
    }
}
