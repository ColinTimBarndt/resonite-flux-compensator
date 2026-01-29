# High Level Intermediate Representation

The high level intermediate representation is used as an intermediate step between reading the IL of node implementations and writing the compiled/transpiled resulting IL.

## Basic Example

```mermaid
graph LR;

S{{Start}} f1@==> Write[[Write X]];

Const([Constant 1]);
Const -->|value| Write;

Read(Read X);
Read -->|a| Eq(==);
Const -->|b| Eq;

If[[If]];
Write f2@==> If;
Eq -->|condition| If;

If f3@==>|onTrue| ActionA[[Action A]]
If f4@==>|onFalse| ActionB[[Action B]]

f1@{animate: true};
f2@{animate: true};
f3@{animate: true};
f4@{animate: true};
```

```cs
local int X = 0;
state start {
  next b_write;
}
state b_write {
  int value;
  // Eval inputs
  call b_const(&value);
  // Write (value, &X);
  X = value;
  next b_if;
}
b_const(int ref value) {
  value = 1;
}
state b_if {
  bool condition;
  // Eval inputs
  call b_eq(&condition);
  // If(condition, b_onTrue, b_onFalse)
  if (condition)
    next b_action_a;
  else
    next b_action_b;
}
b_eq(bool ref result) {
  int a, b;
  // Eval inputs
  call b_read(&a);
  call b_const(&b);
  // Eq(a, b, &result)
  result = a == b;
}
b_read(int ref value) {
  value = X;
}
state b_action_a { ... }
state b_action_b { ... }
```

## Recursion

```mermaid
graph LR;

S{{Start}} f1@==> Write0[[Write N]];

Const0([Constant 0]);
Const0 -->|value| Write0;

Read(Read N);
Read -->|a| LessThan(<);

Const5([Constant 5]);
Const5 -->|b| LessThan;

If[[If]];
Write0 f2@==> If;
LessThan -->|condition| If;

If f3@==>|onTrue| Inc[[Increment N]]
If f4@==>|onFalse| End{{End}}

Inc f5@==> If

f1@{animate: true};
f2@{animate: true};
f3@{animate: true};
f4@{animate: true};
f5@{animate: true};
```

TODO
