# CSharpBasicProjects
A collection of C# console projects, demos, and explorations.

## Highlighted Projects:
1. [ClassInheritanceOperatorOverloading](#classinheritanceoperatoroverloading) featuring operator overloading on generic types.
2. [ClassMethodsOverloading](#classmethodsoverloading) featuring a prime number generator (sieve of Eratosthenes) implemented using a highly efficient bitwise managed array.
3. [ExceptionHandling](#exceptionhandling) featuring error logging & System.Diagnostics tools.
4. [ExceptionHandlingWithInheritance](#exceptionhandlingwithinheritance) featuring custom error classes.
5. [Structs](#structs) (my favorite) featuring a highly robust, custom data type which vastly expands the range and accuracy of the System.Double data type.

## ClassInheritanceOperatorOverloading
A C# console app which implements class inheritance for Employee objects.

Highlights:
1. Class Inheritance
2. Abstract and Virtual Classes
3. Custom Interfaces and Implementation
4. Classes based on Generic Types <T> with operator overloading (very challenging)

## ClassMethodsOverloading
A C# console app which implements a highly efficient sieve of Eratosthenes to generate prime numbers, based on a bitwise manage array of un-signed long integers for optimal speed and memory usage.

Highlights:
1. Static methods
2. Method overloading
3. Various parameter types (out, optional, etc.)

## ExceptionHandling
A C# console app which handles exceptions, generates useful diagnostic data, and writes diagnostic data to an external log file.

Highlights:
1. Use of System.Diagnostic Trace tools
2. Catching multiple different types of exceptions with special handling for each.
3. External file error logging

## ExceptionHandlingWithInheritance
A C# console app which further demonstrates error handling by creating custom classes which inherit from System.Exception.

Highlights:
1. Custom Exception Classes
2. Detailed exception messages and user help
3. Retries in order to attempt avoiding exceptions

## Structs
A C# console app which develops a custom data type, Fraction, which works seamlessly with the System.Double data type and significantly expands its range and accuracy.

Highlights:
1. Converts System.Double and System.Decimal to/from Fraction using bitwise conversions for maximum accuracy and speed.
2. Infinite precision for many very small or very large fractions whose numerators and denominators can be represented as 64 bit unsighted integers.
3. Highly Robust
   -Handles many different scenarios to prioritize exactness and automatically converts to a "floating-point" approximations in order to avoid overflow/underflow when exactness cannot be maintained.
   -Utilizes +/-Infinity and NaN as System.Double does to avoid throwing exceptions such as division by zero.
4. 136 bit datatype. Five extra bits made available as boolean flags for custom use.
5. Automatically tracks states of the Fraction such as exactness and reducedness.
