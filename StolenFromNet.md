#### RX related

```csharp
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

```csharp

  interface IEnumerable<out T> 
  {
    IEnumerator<T> GetEnumerator();
  }
  
  interface IEnumerator<out T> : IDisposable
  {
    bool MoveNext(); // <= BLOCKING!
    T Current { get; }
    void Reset();
  }
  
  
  interface IObservable<out T>
  {
    IDisposable Subscribe(IObserver<T> observer);
  }
  
  interface IObserver<out T>
  {
    void OnNExt(T value); // push the next value
    void OnError(Exception ex); // an error occurred!
    void OnCompleted(); // we're done!
  }

```
