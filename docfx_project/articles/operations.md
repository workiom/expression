# Built-In Operations
Infix Operation | Class | Operation Type
--- | --- | ---
- | [Albatross.Expression.Operations.Minus](xref:Albatross.Expression.Operations.Minus) | infix
- | [Albatross.Expression.Operations.Negative](xref:Albatross.Expression.Operations.Negative) | Unary
% | [Albatross.Expression.Operations.Mod](xref:Albatross.Expression.Operations.Mod) | infix
* | [Albatross.Expression.Operations.Multiply](xref:Albatross.Expression.Operations.Multiply) | infix
/ | [Albatross.Expression.Operations.Divide](xref:Albatross.Expression.Operations.Divide) | infix
@ | [Albatross.Expression.Operations.Array](xref:Albatross.Expression.Operations.Array) | Unary
^ | [Albatross.Expression.Operations.Power](xref:Albatross.Expression.Operations.Power) | infix
+ | [Albatross.Expression.Operations.Plus](xref:Albatross.Expression.Operations.Plus) | infix
+ | [Albatross.Expression.Operations.Positive](xref:Albatross.Expression.Operations.Positive) | Unary
< | [Albatross.Expression.Operations.LessThan](xref:Albatross.Expression.Operations.LessThan) | infix
<= | [Albatross.Expression.Operations.LessEqual](xref:Albatross.Expression.Operations.LessEqual) | infix
<> | [Albatross.Expression.Operations.NotEqual](xref:Albatross.Expression.Operations.NotEqual) | infix
= | [Albatross.Expression.Operations.Equal](xref:Albatross.Expression.Operations.Equal) | infix
> | [Albatross.Expression.Operations.GreaterThan](xref:Albatross.Expression.Operations.GreaterThan) | infix
>= | [Albatross.Expression.Operations.GreaterEqual](xref:Albatross.Expression.Operations.GreaterEqual) | infix
and | [Albatross.Expression.Operations.And](xref:Albatross.Expression.Operations.And) | infix
avg | [Albatross.Expression.Operations.Avg](xref:Albatross.Expression.Operations.Avg) | prefix
Coalesce | [Albatross.Expression.Operations.Coalesce](xref:Albatross.Expression.Operations.Coalesce) | prefix
CurrentApp | [Albatross.Expression.Operations.CurrentApp](xref:Albatross.Expression.Operations.CurrentApp) | prefix
CurrentMachine | [Albatross.Expression.Operations.CurrentMachine](xref:Albatross.Expression.Operations.CurrentMachine) | prefix
CurrentUser | [Albatross.Expression.Operations.CurrentUser](xref:Albatross.Expression.Operations.CurrentUser) | prefix
date | [Albatross.Expression.Operations.Date](xref:Albatross.Expression.Operations.Date) | prefix
Format | [Albatross.Expression.Operations.Format](xref:Albatross.Expression.Operations.Format) | prefix
If | [Albatross.Expression.Operations.If](xref:Albatross.Expression.Operations.If) | prefix
IsBlank | [Albatross.Expression.Operations.IsBlank](xref:Albatross.Expression.Operations.IsBlank) | prefix
Left | [Albatross.Expression.Operations.Left](xref:Albatross.Expression.Operations.Left) | prefix
len | [Albatross.Expression.Operations.Len](xref:Albatross.Expression.Operations.Len) | prefix
max | [Albatross.Expression.Operations.Max](xref:Albatross.Expression.Operations.Max) | prefix
min | [Albatross.Expression.Operations.Min](xref:Albatross.Expression.Operations.Min) | prefix
Month | [Albatross.Expression.Operations.Month](xref:Albatross.Expression.Operations.Month) | prefix
MonthName | [Albatross.Expression.Operations.MonthName](xref:Albatross.Expression.Operations.MonthName) | prefix
not | [Albatross.Expression.Operations.Not](xref:Albatross.Expression.Operations.Not) | prefix
Now | [Albatross.Expression.Operations.Now](xref:Albatross.Expression.Operations.Now) | prefix
or | [Albatross.Expression.Operations.Or](xref:Albatross.Expression.Operations.Or) | infix
PadLeft | [Albatross.Expression.Operations.PadLeft](xref:Albatross.Expression.Operations.PadLeft) | prefix
PadRight | [Albatross.Expression.Operations.PadRight](xref:Albatross.Expression.Operations.PadRight) | prefix
pi | [Albatross.Expression.Operations.Pi](xref:Albatross.Expression.Operations.Pi) | prefix
Right | [Albatross.Expression.Operations.Right](xref:Albatross.Expression.Operations.Right) | prefix
ShortMonthName | [Albatross.Expression.Operations.ShortMonthName](xref:Albatross.Expression.Operations.ShortMonthName) | prefix
Text | [Albatross.Expression.Operations.Text](xref:Albatross.Expression.Operations.Text) | prefix
Today | [Albatross.Expression.Operations.Today](xref:Albatross.Expression.Operations.Today) | prefix
Year | [Albatross.Expression.Operations.Year](xref:Albatross.Expression.Operations.Year) | prefix

## What is an **Infix operation**?
It is a math expression with an operator in the middle and two operands, one on each side of the operator.  Here are some examples.
1. `1 + 2`
1. `1 + 2 + 3 `
1. `2 + 5 * 4 `
1. `1 > 2 or 3 > 2 `
1. `(1 + 2) * 3 `
1. ` 1 * -1 `

Infix operation can be chained as the examples shown above.  When chained, the precedence of the operator will decide who gets executed first.  If two operator has the same precedence, the operator should be executed from left to right.  For example ``2 + 5 * 4`` will execute ``5 * 4`` first because the `*` operator has a precedence of 200 which is larger than the precedence of the `+` operator.  The `+` operator will then execute `2 + 20`.  The second operand `20` comes from the output of the `*` operator.

The **parentheses** can be used to supersede the precedence of the operators as show in #5.

A **unary operator** such as the negative sign in #6 will always have precedence over any infix operators.

## The Precedence of Infix Operations
Infix Operation | Class | Precedence
--- | --- | ---
or | [Albatross.Expression.Operations.Or](xref:Albatross.Expression.Operations.Or) | 20
and | [Albatross.Expression.Operations.And](xref:Albatross.Expression.Operations.And) | 30
= | [Albatross.Expression.Operations.Equal](xref:Albatross.Expression.Operations.Equal) | 50
>= | [Albatross.Expression.Operations.GreaterEqual](xref:Albatross.Expression.Operations.GreaterEqual) | 50
> | [Albatross.Expression.Operations.GreaterThan](xref:Albatross.Expression.Operations.GreaterThan) | 50
<= | [Albatross.Expression.Operations.LessEqual](xref:Albatross.Expression.Operations.LessEqual) | 50
< | [Albatross.Expression.Operations.LessThan](xref:Albatross.Expression.Operations.LessThan) | 50
- | [Albatross.Expression.Operations.Minus](xref:Albatross.Expression.Operations.Minus) | 100
<> | [Albatross.Expression.Operations.NotEqual](xref:Albatross.Expression.Operations.NotEqual) | 100
+ | [Albatross.Expression.Operations.Plus](xref:Albatross.Expression.Operations.Plus) | 100
/ | [Albatross.Expression.Operations.Divide](xref:Albatross.Expression.Operations.Divide) | 200
% | [Albatross.Expression.Operations.Mod](xref:Albatross.Expression.Operations.Mod) | 200
* | [Albatross.Expression.Operations.Multiply](xref:Albatross.Expression.Operations.Multiply) | 200
^ | [Albatross.Expression.Operations.Power](xref:Albatross.Expression.Operations.Power) | 300

