using System;

namespace TestProject
{
    public class Dog: Animal, IBark
    {
        public void Bark()
        {
            // Code to bark.
        }

        public void Fight(Cat cat)
        {
            // Code to fight cat.
        }
    }
}
