# Assignment - 2: Calculatior
### Assignment was about creating a console application that will execute basic ariphmetic operations.

[![Build Status](https://travis-ci.org/joemccann/dillinger.svg?branch=master)](https://travis-ci.org/joemccann/dillinger)

## Completed items

* Basic REPL (read-eval-print loop) for an expression evaluator
* Added lexer, a parser, and an evaluator
* Handle `+`, `-`, `*`, `/`, `^` and parenthesized expressions
* Print syntax trees
* Handling unary expressions `-1`, `-(1+2)` etc.


## Interesting aspects

### Operator precedence

When parsing the expression `1 + 2 * 3` we need to parse it into a tree
structure that honors priorties, i.e. that `*` binds stronger than `+`:

```
└──BinaryExpression
    ├──NumberExpression
    │   └──NumberToken 1
    ├──PlusToken
    └──BinaryExpression
        ├──NumberExpression
        │   └──NumberToken 2
        ├──StarToken
        └──NumberExpression
            └──NumberToken 3
```

A naive parser might yield something like this:

```
└──BinaryExpression
    ├──BinaryExpression
    │   ├──NumberExpression
    │   │   └──NumberToken 1
    │   ├──PlusToken
    │   └──NumberExpression
    │       └──NumberToken 2
    ├──StarToken
    └──NumberExpression
        └──NumberToken 3
```

The problem with having incorrect trees is that you interpret results
incorrectly. For instance, when walking the first tree one would compute the
(correct) result `7` while the latter one would compute `9`.

In this parser ,which is a handwritten recursive descent parser, it was
achieved by structuring our method calls accordingly.


### Fabricating tokens

In some cases, the parser asserts that specific tokens are present. For example,
when parsing a parenthesized expression, it will assert that after consuming a
`(` and an `<expression>`, a `)` token follows. If the current token doesn't match
the expectation, it will fabricate a token out of thin air.

This is useful as it avoids cases where later parts of the compiler that walk
the syntax tree have to assume anything could be null.
