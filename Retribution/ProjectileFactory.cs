using System;


{
	public static class ProjectileFactory()
	{
        public ProjectileFactory()
        {
          
        }
        public void create(String s, GameObject start, GameObject obj)
        {
          if(s=="arrow")
          {
            Arrow arr=new Arrow(obj, start);
              //need to add arrow to projectileManager
          }
        }



	}
}
