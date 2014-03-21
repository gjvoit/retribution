using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Retribution
{
    
    class Tower : GameObject
    {
        public string state;
        public Tower(Vector2 position, int health = 2, int damage = 0, int attack_range = 0, int movement_range = 0)
            : base (health, position, damage, attack_range, movement_range)
        {
            this.position = position;
            this.state = "Wall";
        }

        public override void Move()
        {
            throw new NotImplementedException();
        }

        public void Morph(string mobiletype = "Wall")
        {
            if (mobiletype == "archer")
                this.state = "RangedTower";
            else if (mobiletype == "warrior")
                this.state = "EarthquakeTower";
        }

        public override void Die()
        {
            throw new NotImplementedException();
        }

        public void Update(GameTime gameTime)
        {
            if (this.health > 0)
            {
                // towers can't move so no movement option
                // TODO: attack option
                // TODO: Morph option
            }

            if (this.health <= 0)
            {
                this.Die();
            }
        }
        /*
        public void GetValues()
        {
            string output = string.Format("{0}:, {1}:, {2}:", this.health, this.damage, this.position);
            Console.WriteLine(output);
        }

        static void Main()
        {
            Tower tower = new Tower(new Vector2(20, 20));
            tower.GetValues();
            tower.Morph("Wall");
            Console.WriteLine(tower.state);
            tower.Morph("archer");
            Console.WriteLine(tower.state);
            tower.Morph("warrior");
            Console.WriteLine(tower.state);
        }
         */
    }
}
