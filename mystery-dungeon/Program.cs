using System;

class Program {
    static void Main(string[] args) {
        var fieldGenerator = new FieldGenerator();
        var field = fieldGenerator.Generate();
        field.Print();
    }
}

