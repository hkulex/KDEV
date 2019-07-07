namespace alternatereality
{
    public class TankEnemy : BaseEnemy
    {
        

        protected override void Awake()
        {
            base.Awake();

            health = 5;
            points = 25;
        }
    }
}