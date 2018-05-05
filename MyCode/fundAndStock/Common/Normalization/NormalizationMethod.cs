using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Standardization 一般是 z-score规范化方法：(x-mean(x))/std(x)
namespace Normalization
{
    /// <summary>
    /// y = 2*(x-min)/(max-min)-1 ->[-1,1]
    /// </summary>
    public class NormalizationMinMax2Method : NormalizationMinMaxMethod
    {
        protected override double Scale(double x)
        {
            return 2*((x - Min) / (Max - Min)) -1;
        }

        public override double UnScale(double y)
        {
            return (y+1)/2 * (Max - Min)+Min;
        }
 
    }
    /// <summary>
    /// y=(x-min)/(max-min)->[0,1] or [0.1,0.9]; default:[0.1,0.9].
    /// </summary>
    public class NormalizationMinMaxMethod : NormalizationMethod<double>, INormalizationMethod
    {
        protected virtual double c { get; set; }
        protected virtual double d { get; set; }
        protected virtual double Min { get; set; }
        protected virtual double Max { get; set; }
        public NormalizationMinMaxMethod():this(0.1,0.9)
        {

        }
        public NormalizationMinMaxMethod(double resultMin) : this(resultMin,1.0)
        {

        }
        public NormalizationMinMaxMethod(double resultMin, double resultMax)
        {
            this.c = resultMin;
            this.d = resultMax;
            InitParams(resultMin);
            InitParams(resultMax);
        }
        protected override double Scale(double x)
        {
            return  ((x-Min)/(Max-Min))*(d-c)+c;
        }
        public override double UnScale(double y)
        {
            return ((y-c)/ (d - c)) * (Max - Min) + Min;
        }
        protected override void InitParams(double v)
        {
            if (v > Max)
            {
                Max = v;
            }
            else if (v < Min)
            {
                Min = v;
            }
        }
    }
    /// <summary>
    /// y=log10(x)/log(max)->[0,1]
    /// </summary>
    public class NormalizationLogMethod : NormalizationMethod<double>, INormalizationMethod
    {
        protected virtual double Max { get; set; }
        protected override double Scale(double x)
        {
            return Math.Log10(x)/ Math.Log10(Max);
        }
        public override double UnScale(double y)
        {
            return Math.Pow(10,y/ Math.Log10(Max));
        }
        protected override void InitParams(double v)
        {
            if (v > Max)
            {
                Max =v;
            }
        }
    }
    /// <summary>
    /// y=atan(x)*2/PI->[-1,1]
    /// </summary>
    public class NormalizationAtanMethod: NormalizationMethod<double>,INormalizationMethod
    {
        protected override double Scale(double x)
        {
            return Math.Atan(x) * 2 / Math.PI;
        }
        public override double UnScale(double y)
        {
            return Math.Tan(y * Math.PI / 2);
        }
        protected override void InitParams(double v)
        {
           
        }
    }
   
        public abstract class NormalizationMethod<T> : INormalizationMethod<T>
    {

        protected abstract T Scale(T x);
        public abstract T UnScale(T y);
       
        public T[] Scale(T[] vv)
        {
            for(long index=0; index<vv.LongLength;index++)
            {
                vv[index] = Scale(vv[index]);
            }
            return vv;
        }
        public T[] UnScale(T[] vv)
        {
            for (long index = 0; index < vv.LongLength; index++)
            {
                vv[index] = UnScale(vv[index]);
            }
            return vv;
        }
        public T[][] Scale(T[][] vv)
        {
            for (long index = 0; index < vv.LongLength; index++)
            {
                vv[index] = Scale(vv[index]);
            }
            return vv;
        }
        public T[][] UnScale(T[][] vv)
        {
            for (long index = 0; index < vv.LongLength; index++)
            {
                vv[index] = UnScale(vv[index]);
            }
            return vv;
        }
        public T[][][] Scale(T[][][] vv)
        {
            for (long index = 0; index < vv.LongLength; index++)
            {
                vv[index] = Scale(vv[index]);
            }
            return vv;
        }
        public T[][][] UnScale(T[][][] vv)
        {
            for (long index = 0; index < vv.LongLength; index++)
            {
                vv[index] = UnScale(vv[index]);
            }
            return vv;
        }
        public virtual void InitParams(T[][][] vv)
        {
            for (long index = 0; index < vv.LongLength; index++)
            {
                InitParams(vv[index]);
            }
        }
        public virtual void InitParams(T[][] vv)
        {
            for (long index = 0; index < vv.LongLength; index++)
            {
                InitParams(vv[index]);
            }
        }
        public virtual void InitParams(T[] vv)
        {
            for (long index = 0; index < vv.LongLength; index++)
            {
                InitParams(vv[index]);
            }
        }
        protected abstract void InitParams(T v);
    }
}
