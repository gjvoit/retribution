using System;

public class Arrow : GameObject
{
    GameObject target;
	public Arrow(GameObject target, Vector2 start)
         : base(health=9999999, position=start, damage=1000, attack_range=1)
	{
        this.target = target;
	}
    public void move()
    {
        Vector2 tpos = target.position;
        Vector2 mov_vec = Vector2.Substract(this.position, tpos);
        Vector2 norm_mov_vec = Vector2.Normalize(mov_vec);
        Vector2 delta_vec = norm_mov_vec * this.moveSpeed;
        position = Vector2.Add(pos, delta_vec);
    }
}
