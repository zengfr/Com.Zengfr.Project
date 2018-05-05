using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Com.Zfrong.Common.Algorithms;
namespace Com.Zfrong.Common.Algorithms.TFIDF
{
  public  class URLTFIDFMeasure:TFIDFMeasure
    { 
      #region 字段
      /// <summary>
      /// 相似度,此相似度的Doc个数
      /// </summary>
      public SortedList<float, int> List = new SortedList<float, int>();//
      /// <summary>
      /// List的值排序
      /// </summary>
      public List<KeyValuePair<float, int>> VList = new List<KeyValuePair<float, int>>();//
      /// <summary>
      /// 要比较的Doc序号
      /// </summary>
      public int DocIndex = 0;
      /// <summary>
      /// 小于此值的将忽略
      /// </summary>
      public double MinF =0.00000001;
      public URLTFIDFMeasure(string[] doc)
          : base(doc)
      {

      }
       #endregion
      #region
      /// <summary>
      /// 
      /// </summary>
      public new void Init()
      {
         base.Init();//
          //StopWordsHandler stopword = new Com.Zfrong.Common.Algorithms.TFIDF.StopWordsHandler();
          this.GetMeasure();

          foreach (KeyValuePair<float, int> e in this.List)
          {
              this.VList.Add(e);//
          }
          this.VList.Sort(new PowerComparer<float, int>(PowerComparerType.Value,true));//
      }
     /// <summary>
     /// 
     /// </summary>
      private void GetMeasure()
      {
          for (int i = 0; i <_docs.Length; i++)
          {
              float f = GetSimilarity(this.DocIndex, i);//
              if (f >=this.MinF)
              {
                  if (!this.List.ContainsKey(f))
                  {
                      this.List.Add(f, 1);//
                  }
                  else {
                      this.List[f] += 1;//
                  }
              }
          }
      }
       #endregion
      #region
      /// <summary>
      /// 选取与this.DocIndex比较的 按相似度排序的 集合
      /// </summary>
      /// <returns></returns>
      public IList<IList<string>> GetMaxSimilarityDoc()
      {
          return GetMaxSimilarityDoc(this.DocIndex);//
      }
      /// <summary>
      /// 选取与Doc[index]比较的 按相似度排序的 集合
      /// </summary>
      /// <param name="index"></param>
      /// <returns></returns>
      private IList<IList<string>> GetMaxSimilarityDoc(int index)
      {
          IList<IList<string>> list = new List<IList<string>>();//
          for (int i = this.VList.Count - 1; i >= 0; i--)
          {
              list.Add(GetSimilarityDoc(index,this.VList[i].Key));//
          }
          return list;//
      }
      /// <summary>
      /// 在docs中找出与doc[index]相似的 集合
      /// </summary>
      /// <param name="index"></param>
      /// <param name="doc"></param>
      /// <returns></returns>
      public IList<string> GetIsSimilarity(int index, string[] docs)
      {
          IList<string> ss = GetSimilarityDoc(index);//
          int count = ss.Count + docs.Length;
          string[] doc1=new string[count];
          
          for (int i = 0; i < ss.Count;i++ )
          {
              doc1[i] = ss[i];//
          }
          for (int i =0 ; i <docs.Length; i++)
          {
              doc1[ss.Count+i] = docs[i];//
          }
          URLTFIDFMeasure u = new URLTFIDFMeasure(doc1);//
          u.Init();//
          float f1 = u.GetSimilarity(0,1);//
          return u.GetSimilarityDoc(0,f1);//
      }
      #endregion
      #region
      /// <summary>
      /// 选取与this.DocIndex比较的 相似的doc 集合
      /// </summary>
      /// <returns></returns>
      public IList<string> GetSimilarityDoc()
      {
        return  GetSimilarityDoc(this.DocIndex);//
      }
      /// <summary>
      /// 选取与this.DocIndex比较的 指定相似度的doc 集合
      /// </summary>
      /// <returns></returns>
      public IList<string> GetSimilarityDoc(float fv)
      {
          return GetSimilarityDoc(this.DocIndex,fv);//
      }
      /// <summary>
      /// 选取与doc[index]比较的 相似的doc 集合
      /// </summary>
      /// <param name="index"></param>
      /// <returns></returns>
      public IList<string> GetSimilarityDoc(int index)
      {
          return GetSimilarityDoc(index, 1);//
      }
      /// <summary>
      ///  选取与doc[index]比较的 指定相似度的doc 集合
      /// </summary>
      /// <param name="index"></param>
      /// <param name="fv"></param>
      /// <returns></returns>
      public IList<string> GetSimilarityDoc(int index, float fv)
      {
          return GetSimilarityDoc(index, fv, fv);//
      }
      /// <summary>
      /// 选取与doc[index]比较的 在指定相似度范围内的doc 集合
      /// </summary>
      /// <param name="index"></param>
      /// <param name="fv1"></param>
      /// <param name="fv2"></param>
      /// <returns></returns>
      public IList<string> GetSimilarityDoc(int index, float fv1,float fv2)
      {
          Trace.WriteLine("--------------------------------------");//
          IList<string> _list = new List<string>();//
          for (int i = 0; i < _docs.Length; i++)
          {
              float f = GetSimilarity(index, i);//
              if (f>=fv1&&f<=fv2)
              {
                  _list.Add(_docs[i]);//
                 Trace.WriteLine(f+"     "+_docs[i]);//
              }
          }
          return _list;//
      }
        #endregion
    }
}
