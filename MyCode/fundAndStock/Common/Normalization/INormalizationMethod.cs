namespace Normalization
{
    public interface INormalizationMethod:INormalizationMethod<double>
    {

    }
    public interface INormalizationMethod<T>
    {
        void InitParams(T[][][] vv);
        void InitParams(T[][] vv);
        void InitParams(T[] vv);

        T[][] Scale(T[][] xx);
        T[][][] Scale(T[][][] xx);
        T[] Scale(T[] xx);
       
        T[][][] UnScale(T[][][] yy);
        T[][] UnScale(T[][] yy);
        T[] UnScale(T[] yy);
        T UnScale(T yy);
    }
}