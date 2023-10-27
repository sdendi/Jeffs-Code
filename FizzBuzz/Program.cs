// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var numbers = Enumerable.Range(1,100);

foreach(var num in numbers) {

    if(num % 3 == 0 && num % 5 == 0) {
        Console.WriteLine("FizzBuzz");
        continue;
    }
    if(num % 5 == 0) {
        Console.WriteLine("Fizz");
        continue;
    }
    if(num % 3 == 0) {
        Console.WriteLine("Buzz");
        continue;
    }
    Console.WriteLine(num);
}