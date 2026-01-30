# High Level Intermediate Representation

The high level intermediate representation is used as an intermediate step between reading the IL of node implementations and writing the compiled/transpiled resulting IL.

The HIR of an impulse flow is a [finite-state machine]:

- $\Sigma$ is the input alphabet, which consists of all tail calls;
- $\sigma\in\Sigma$ is a tail call;
- $S$ is the finite non-empty set of states;
- $s_0 \in S$ is the initial state;
- $\delta:S\times\Sigma\to S$ is the state-transition function;
- $\tau\subset S\times\Sigma$ is the set of all yielding/async transitions;

$$
\begin{align}
S_n \overset\sigma\to S_k &\iff \delta(S_n, \sigma) = S_k
\\
S_n \overset\sigma\leadsto S_k &\iff S_n \overset\sigma\to S_k~\land~(S_n,\sigma) \in \tau
\\
S_n \overset\sigma\rightarrowtail S_k &\iff S_n \overset\sigma\to S_k~\land~(S_n,\sigma) \notin \tau
\end{align}
$$

To put it in words: $A\rightarrowtail B$ is a synchronous transition while $A\leadsto B$ is a yielding transition. Calls which are not tail calls do not call as transitions.

[finite-state machine]: https://en.wikipedia.org/wiki/Finite-state_machine

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

### Intermediate Representation

```mermaid
graph LR;

S{{Start}} ==> Write[[Write X]] ==> If[[If]]
If ==> A[[Action A]] ==> E{{End}}
If ==> B[[Action B]] ==> E
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

Inc f4@==> If

f1@{animate: true};
f2@{animate: true};
f3@{animate: true};
f4@{animate: true};
```

### Intermediate Representation

```mermaid
graph LR;

S{{Start}} ==> Write[[Write N]] ==> If[[If]]
If ==> Inc[[Increment N]] ==> If
If ==> E{{End}}
```

## Non-Tail Calls

```mermaid
graph LR;

S{{Start}} f1@==> For[[For]] f2@==>|Next| B[[Action B]] f3@==> A[[Action A]]
Const([Constant 10]) --> For
For f4@==>|Step| A

f1@{animate: true}
f2@{animate: true}
f3@{animate: true}
f4@{animate: true}
```

### Intermediate Representation

```mermaid
graph LR;

S{{Start}} ==> For[[For]] ==> B[[Action B]] ==> A[[Action A]] ==> E{{End}}

For -.-> A

```
