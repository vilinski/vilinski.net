#### RX related

```C#
// event is NOT A FIRST CLASS C# CITIZEN 
form1.MouseMove += (sender, args) => {
  if (args.Location.X == args.Location.Y) 
  {
    // I'd like to raise another event here...
    // NOT COMPOSABLE
  }
};

form1.MouseMove -= // was soll hier sein
// umständliche Syntax, resource leaks
```
