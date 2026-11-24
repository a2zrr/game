using System;
    public class PlayerSettings 
    
    {
        private int Speed;        // PI
        private int coffeeBoost;  // PI
        private int coffeeTime;   // PI

        public PlayerSettings(int Speed, int coffeeBoost, int coffeeTime)
        {
            SetSpeed(Speed);
            SetCoffeeBoost(coffeeBoost);
            SetCoffeeTime(coffeeTime);
        }
        
        public int GetSpeed()
        {
            return Speed;
        }

        public void SetSpeed(int Speed)
        {
            if (Speed <= 0)
                throw new ArgumentOutOfRangeException(nameof(Speed), "Speed doit etre un entier positif.");
            this.Speed = Speed;
        }

        public int GetCoffeeBoost()
        {
            return coffeeBoost;
        }

        public void SetCoffeeBoost(int coffeeBoost)
        {
            if (coffeeBoost <= 0)
                throw new ArgumentOutOfRangeException(nameof(coffeeBoost), "coffeeBoost doit etre un entier positif.");
            this.coffeeBoost = coffeeBoost;
        }

        public int GetCoffeeTime()
        {
            return coffeeTime;
        }

        public void SetCoffeeTime(int coffeeTime)
        {
            if (coffeeTime <= 0)
                throw new ArgumentOutOfRangeException(nameof(coffeeTime), "coffeeTime doit etre un entier positif.");
            this.coffeeTime = coffeeTime;
        }
    }