# Exceptions and State Management

An exception is when a member fails to complete the task it is supposed to perform as indicated by its name.

```
try {}
catch(InvalidOperationException ex) {}
catch {}
finally {}
```

#### The ___try___ block

The ___try block___ contains code that requires common cleanup operations, exception-recovery operations, or both. A try
block
must be associated with at least one ___catch___ or ___finally___ block.

### The ___catch___ block

A ___catch block___ contains code to execute in response to an exception. A try block can have zero or more catch blocks
associated with it.

### The ___finally___ block

The ___finally block___ contains code that's guarantee to execute.

## Exception Guidelines and Best Practices

- Use ___finally___ blocks liberally
- Don't ___catch___ everything
- Recover gracefully from an Exception
- Backing out of a partially completed operation when an unrecoverable exception occurs - maintaining state
- Hiding an implementation detail to maintain a 'Contract'
- 