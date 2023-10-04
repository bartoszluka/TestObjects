# TestObjects

This project was made to simplify test running in C#.
Because most other test running frameworks require a lot of specific knowledge to create and run tests.

Problems with other frameworks:

- "0 references" on test methods, they need to be discovered through reflections
- Documentation needed for things like running something before or after each test
- Complex "fixtures"

All of those are caused because a test is not treated like a value or an **object**.
With this simple mindset, you can compose your tests, create them dynamically, filter on specific conditions, and so on using your C# knowledge.
