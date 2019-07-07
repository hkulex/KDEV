namespace alternatereality
{
    public interface IHittable
    {
        void Hit(int damage);
        void Death();
    }
}