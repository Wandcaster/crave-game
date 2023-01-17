using System;

[Flags]
public enum CardTarget
{
    None = 0,
    Enemy = 1,
    Self= 2,
    TeamMate=4
}
