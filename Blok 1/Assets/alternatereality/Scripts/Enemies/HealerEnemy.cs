namespace alternatereality
{
    public class HealerEnemy : BaseEnemy
    {


        protected override void Awake()
        {
            base.Awake();

            health = 2;
        }

        // heal neighbours
        private void Heal()
        {

        }
    }
}