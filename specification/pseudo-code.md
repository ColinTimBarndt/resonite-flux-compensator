# Flux Compensator Pseudo Code

Flux Compensator generates pseudo-code in addition to the compiled IL for an improved debugging experience.
This document defines the general syntax for this pseudo programming language with examples.

Pseudo code should be as close to DotNet IL as possible while being easier to read.

```cs
// Group defines execution context data for the following definitions
group (
  // Stores are bound to groups
  y: int;
)
{
  impulse Hello (
    // Locals are bound to the impulse
    x: int;
    a: int;
    b: int;
  )
  {
    // C-style reference (&) syntax used to signal that the local is passed, not its value
    Write (a, &x);

    // Temporaries defined in code
    // These will either be IL locals or pushed onto the stack
    var d: int;
    // Invocation of a node. In IL, the node will be inlined.
    Mul (x, 2) -> (d);

    var t: int;
    Add (d, 1) -> (t);

    var cond: bool;
    GreaterThan (t, 10) -> (cond);

    // Control flow nodes either receive a..
    // - jump label
    // - * for continuing below
    // - ! for doing nothing (call/continuation not connected)
    If (cond) -> (*, no);

    Write (42, &b);
    return;

  no:
    Write (t, &b);
    return;
  }
}
```
