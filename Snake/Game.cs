﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace Snake
{
    class Game
    {

        public Game()
        {
            this.currentKey = ConsoleKey.Delete;
            this.lastKey = ConsoleKey.Delete;
            this.lastDirectionKey = ConsoleKey.Delete;
            this.points = 20;
            this.speed = 300;
            this.randFoodPosition = new Random();
            this.bestScore = 0;
        }

        public bool GameOver { get; set; }
        private ConsoleKey currentKey;
        private ConsoleKey lastKey;
        private ConsoleKey lastDirectionKey;
        private int points;
        private int speed;
        private int foodPositionX;
        private int foodPositionY;
        private Random randFoodPosition;
        private int bestScore;
        public int level { get; set; }
        Snake snake = new Snake(1, 1);
        public void StartGame()
        {
            
            snake.SnakeTail = Snake.InitializeSnake(2, this.level);
            this.points = 20;
            this.GameOver = false;
            this.currentKey = ConsoleKey.Delete;
            this.speed = 300;
            GenerateFood();

            // Get the best score for the current level
            this.bestScore = int.Parse(File.ReadAllText(Directory.GetCurrentDirectory() + @"\level" + this.level + ".txt"));


            while (this.GameOver == false)
            {
                if (Console.KeyAvailable)
                {
                    this.currentKey = Console.ReadKey(true).Key;

                    if (this.lastKey == this.currentKey)
                    {
                        if (this.speed > 100)
                        {
                            this.speed -= 40;
                        }
                        else if (this.speed > 30)
                        {
                            this.speed -= 10;
                        }

                        continue;
                    }
                    this.speed = 300;


                }

                Console.Clear();
                Environment.DrawArea(this.level);
                DrawSpeed();
                DrawPoints();
                Environment.DrawFood(this.foodPositionX, this.foodPositionY);
                currentKey = snake.Update(currentKey, lastDirectionKey);
                snake.Draw();
                if (snake.CollisionWithSnake())
                {
                    this.GameOver = true;
                    break;
                }
                if (snake.CollisionWithFood(this.foodPositionX, this.foodPositionY))
                {
                    this.points += 10;
                    GenerateFood();
                }
                snake.DrawLength();
                if (snake.CollisionWithWall(this.level))
                {
                    this.GameOver = true;
                    break;
                }
                Thread.Sleep(this.speed);
                this.lastKey = this.currentKey;
                lastDirectionKey = currentKey;
            }
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(51, 10);
            Console.Write("Game Over!");
            if (points > bestScore)
            {
                File.WriteAllText(Directory.GetCurrentDirectory() + @"\level" + this.level + ".txt", points.ToString());
                Console.SetCursorPosition(51, 11);
                Console.Write("New record: " + points);
            }
            else
            {
                Console.SetCursorPosition(51, 11);
                Console.Write("Current score: " + points);
                Console.SetCursorPosition(51, 12);
                Console.Write("Best score: " + bestScore);
            }
            Thread.Sleep(3000);
        }

        public void GenerateFood()
        {
            switch (this.level)
            {
                case 1:
                    this.foodPositionX = this.randFoodPosition.Next(31, 90);
                    this.foodPositionY = this.randFoodPosition.Next(3, 23);
                    break;
                case 2:
                    while (true)
                    {
                        this.foodPositionX = this.randFoodPosition.Next(31, 90);
                        this.foodPositionY = this.randFoodPosition.Next(3, 23);

                        if (this.foodPositionX < 50 || this.foodPositionX > 70 && this.foodPositionY != 10)
                        {
                            break;
                        }
                        else if (this.foodPositionX < 50 || this.foodPositionX > 70 && this.foodPositionY != 15)
                        {
                            break;
                        }
                    }

                    break;
                case 3:
                    this.foodPositionX = this.randFoodPosition.Next(42, 77);
                    this.foodPositionY = this.randFoodPosition.Next(7, 20);
                    break;
                default:
                    break;
            }
        }

        public void Menu()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Please enter level number");
            Console.WriteLine("1 - Classic");
            Console.WriteLine("2 - Tunnel");
            Console.WriteLine("3 - Classic small");
            Console.WriteLine("4 - Snake color");
            Console.WriteLine("5 - Environment color");
            Console.WriteLine("6 - Scores");
            Console.WriteLine("7 - Exit the game");
            this.level = 0;
            while (this.level <= 0 || this.level >= 8)
            {
                try
                {
                    this.level = int.Parse(Console.ReadLine());
                }
                catch (Exception)
                {
                    Console.WriteLine("Please enter a number in range 1 - 7");
                }
                switch (this.level)
                    {
                        case 1:
                            Environment.DrawArea(this.level);
                            StartGame();
                            break;
                        case 2:
                            Console.Clear();
                            Environment.DrawArea(this.level);
                            StartGame();
                            break;
                        case 3:
                            Console.Clear();
                            Environment.DrawArea(this.level);
                            StartGame();
                            break;
                    case 4:
                        Console.Clear();
                        snake.SetColor();
                        Menu();
                        break;
                    case 5:
                        Console.Clear();
                        Environment.SetColor();
                        Menu();
                        break;
                    case 6:
                        Console.Clear();
                        DrawScores();
                        Menu();
                        break;
                    case 7:
                        return;
                        default:
                        Console.WriteLine("Please enter a number in range 1 - 7");
                        break;
                    }
            }
        }

        public void DrawPoints()
        {
            Console.SetCursorPosition(100, 1);
            Console.Write("Points: {0}", this.points);
        }

        private void DrawSpeed()
        {
            Console.SetCursorPosition(100, 3);
            int convertedSpeed = 0;
            switch (this.speed)
            {
                case 300:
                    convertedSpeed = 1;
                    break;
                case 260:
                    convertedSpeed = 2;
                    break;
                case 220:
                    convertedSpeed = 3;
                    break;
                case 180:
                    convertedSpeed = 4;
                    break;
                case 140:
                    convertedSpeed = 5;
                    break;
                case 100:
                    convertedSpeed = 6;
                    break;
                case 90:
                    convertedSpeed = 7;
                    break;
                case 80:
                    convertedSpeed = 8;
                    break;
                case 70:
                    convertedSpeed = 9;
                    break;
                case 60:
                    convertedSpeed = 10;
                    break;
                case 50:
                    convertedSpeed = 11;
                    break;
                case 40:
                    convertedSpeed = 12;
                    break;
                case 30:
                    convertedSpeed = 13;
                    break;
                default:
                    break;
            }
            Console.Write("Speed: " + convertedSpeed);
        }

        private void DrawScores()
        {
            Console.WriteLine("Classic - " + File.ReadAllText(Directory.GetCurrentDirectory() + @"\level" + 1 + ".txt"));
            Console.WriteLine("Tunnel - " + File.ReadAllText(Directory.GetCurrentDirectory() + @"\level" + 2 + ".txt"));
            Console.WriteLine("Classic small - " + File.ReadAllText(Directory.GetCurrentDirectory() + @"\level" + 3 + ".txt"));
            Console.ReadKey();
        }
    }
}
