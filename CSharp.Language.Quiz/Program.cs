﻿using System;
using System.Collections.Generic;
using CSharp.Language.Quiz.Entities;

namespace CSharp.Language.Quiz
{
    public class Program
    {
        private static Random rnd = new Random();

        public static void Main(string[] args)
        {
            Program app = new Program(args);

            app.AssignMarks(30, 80);

            foreach (Student person in app.Students)
            {
                System.Console.WriteLine("\tName: " + person.Name);
                foreach (EarnedMark item in person.Marks)
                    System.Console.WriteLine("\t" + item);
            }

            Console.ReadLine();
        }
        
        private List<Student> _Students = new List<Student>();

        public List<Student> Students
        {
            get { return _Students; }
            set {_Students = value; }
        }

        public Program(string[] StudentNames)
        {
            WeightedMark[] courseMarks = new WeightedMark[4];
            courseMarks[0] = new WeightedMark("Quiz 1", 20);
            courseMarks[1] = new WeightedMark("Quiz 2", 20);
            courseMarks[2] = new WeightedMark("Exercises", 20);
            courseMarks[3] = new WeightedMark("Lab", 35);
            int[] possibleMarks = new int[] { 25, 50, 12, 35 };

            foreach(string name in StudentNames)
            {
                EarnedMark[] marks = new EarnedMark[4];
                for(int i = 0; i < possibleMarks.Length; i++)
                    marks[i] = new EarnedMark(courseMarks[i], possibleMarks[i], 0);
                Students.Add(new Student(name, marks));
            }
        }

        public void AssignMarks(int min, int max)
        {
            foreach (Student person in Students)
                foreach (EarnedMark item in person.Marks)
                    item.Earned = (rnd.Next(min / max) / 100.0) * item.Possible;
        }
    }
}

namespace CSharp.Language.Quiz.Entities
{
    public class Student
    {
        public string Name { get; private set; }
        public EarnedMark[] Marks { get; private set; }

        public Student(string name, EarnedMark[] marks)
            {
            Name = name;
            Marks = marks;
            }
            
    }

    public class WeightedMark
    {
        public int Weight { get; private set; }
        public string Name { get; private set; }
        public WeightedMark(string name, int weight)
        {
            if(weight < 0 || weight > 100)
                throw new Exception("Invalid weight: must be great than 0 and at most 100");
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(name.Trim()))
                throw new Exception("Name can not be empty");
            Weight = weight;
            Name = name;
        }
    }

    public class EarnedMark : WeightedMark
    {
        public int Possible { get; private set; }
        private double _Earned;
        public double Earned
        {
            get { return _Earned; }
            set
            {
                if (value < 0 || value > Possible)
                    throw new Exception("Invalid earned mark assigned");
                _Earned = value;
            }
        }

        public double Percent
        {
            get { return (Earned / Possible) * 100; }
        }

        public double WeightedPercent
        {
            get { return (Percent * Weight) * 100; }
        }

        public EarnedMark(WeightedMark markableItem, int possible, double earned)
            : this(markableItem.Name, markableItem.Weight, possible, earned)
        {

        }

        public EarnedMark(string name, int weight, int possible, double earned)
            : base(name, weight)
        {
            if (possible <= 0)
                throw new Exception("Invalid possible marks");
            Possible = possible;
            Earned = earned;
        }

        public override string ToString()
        {
            return string.Format("{0} (WeightedPercent{3}/{4}) \t - Weighted Mark {5}%",
                                           Name,
                                            Weight,
                                            Percent,
                                            Earned,
                                            Possible,
                                            WeightedPercent);
        }
    }


};
