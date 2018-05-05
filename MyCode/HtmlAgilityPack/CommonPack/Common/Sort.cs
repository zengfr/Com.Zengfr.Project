// ����Sorting ��������͵ķ���ֵ�������⡣

//ѧϰ�ص㣺
//1�����������㷨�����򷽷���ԭ��
//2�����˽�ԭ��Ļ����ϣ������㷨��ʱ�临�ӶȺͿռ临�Ӷȣ���������������֤����
//3����⣬�㷨�����һ��һ�����ϸĽ��ģ��Ľ��㷨ʱ���õ�˼�����Ժͷ�����

//����Ҫ�㣺
//��������
//ȫ�򼯣���֤���ݼ����ڸ�������֮��ɱȽϴ�С��
//�ȶ�����Stable������ȵ����������������ԭ�е��Ⱥ��ϵ���ֲ��䣻
//�ڲ�����Internal�������ݴ洢���ڴ��У����ݵıȽϺ��ƶ��Ǵ�����ȵĲ�����
//�ⲿ����External�������ݴ洢��Ӳ�̣������豸���У����ݵ��ƶ��������Ը��ڱȽϲ�����
//ԭַ���򣨣����������������Ҫ�Ķ���洢�ռ������������n�Ĵ�С�޹ء�

//1�����������򷽷� O(n2) �� �� ѡ������ ���������� ��ϣ������
//2���Ͽ�����򷽷� O(nlogn) �� �� �������� ���ϲ����� �� ������

//��������������죬һ�η����䲻���ݻ��Ĺ��̣�
//ѡ������(selection sorting)
//�㷨���Ѵ������������ݿ��������֣�L��1......i-1��Ϊ�����򲿷֣�L��i......n��Ϊ�����򲿷֡�ÿ�δӴ����򲿷�������СԪ��Ȼ����ò�����Ԫ�ؽ���λ�á�
//������
//    1�����㷨Ϊԭַ������ֻ���ڽ���Ԫ��ʱ��Ҫ����Ĵ洢�ռ䣬�Ƿ�Ϊ�ȶ�����Ҫ�������㷨������
//    2���Ƚϲ����Ĵ�����
//          ��Ϊÿ���ڴ����򲿷�������СԪ����������Ҫ����Ƚϣ������ܵıȽϴ����ǹ̶��ģ���������Сn�޹أ�������������˳���޹ء�
//          �Ƚϴ�����(n-1) + (n-2) + ...... +2 + 1 = n(n-1)/2
//          ʱ�临�Ӷȣ� W(n) = A(n) = n(n-1)/2 =  n2 ��

//��������Insertion Sorting ��
//�㷨�������ݿ������򲿷�L��1... i�������򲿷�L��i+1 ...  n���������֡�ÿ�δ����򲿷���ȡһ��Ԫ�أ����뵽���򲿷ֵ���ȷλ���С�
//������
//        1�����㷨Ϊԭַ�����ȶ�����
//        2���Ƚϲ����Ĵ�����
//             �Ƚϴ���������˳���йء�������˳��ʱ���Ƚϴ������٣�Ϊ(n-1)������������ʱ���Ƚϴ�����࣬Ϊ1+2+3+...+(n-1) = n(n-1)/2  ��
//             ƽ���Ƚ������L[i]���뵽L��1... i-1��ʱ���������ÿ��λ�õĸ�������ȵġ���Ϊi-1��Ԫ�أ������i�����ܲ����λ�ã�����ÿ��λ�õĲ���ĸ���Ϊ1/i ����ʱ���Ƚϴ���������ֵΪ��1/i*1 + 1/i*2+...+1/i*(i-2)+1/i*(i-1)+1/i*(i-1) = (i+1)/2 - 1/i    ,  ע�⣺��ǰ����λ�õıȽϴ�����һ���ġ�
//             A(n) ���Ƶ��� n2/4 



//��������Bubble Sorting��
//����һ��ѡ����������ԭַ�����ȶ�����


//ϣ������Shell Sorting��
//�㷨���Ǹ��ݼ�������Ե����۷�չ���ġ������������������ÿ��ֻ�ܼ���һ������ԣ���ϣ��������Խ���һ�����ϵ�����ԡ���������˵��ϣ������Ҫ���ڲ�������
//           �����л���Ϊh�����Ϊh�������С���ÿ�������н������������е������㷨������ѡ��Ȼ���𲽼�Сhֵ��ֱ��h��1Ϊֹ��

//������ϣ��������Ȼʱ�临�Ӷ�û����ȷ�ļ��㣬��һ����Ϊ����n2�׵ģ�������Ҫ���ڲ�������

//��������QuickSort��
//�㷨�������ǻ��֣�Partition����ͨ��ѡȡ����Ԫ�أ����磺��Ԫ�أ���Ϊ����Ԫ�أ�Ȼ�󽫽�СԪ�طŵ�����Ԫ��ߣ��ϴ�Ԫ�طŵ�����Ԫ�ұߣ���ԭ���л��ֳ������������֡����仰˵�������ҵ��Ļ���Ԫ�ص�λ�á�Ȼ�󣬽������������ٰ��˷����ݹ������ȥ��
//������
//        1�����㷨����ԭַ�������������˵ݹ飻���㷨�����ȶ�����
//        2�� �Ƚϴ�����
//               ������������Ϊ����ʱ��˳������򣩣���ô����Ԫ�����Ǵ������е�һ�ˣ�û���𵽻���Ϊ�����ֵ����ҡ��Ƚϴ�����(n-1)+(n-2)+...+1=n(n-1)/2           n2�׵�
//               ƽ�����������ͨ�����ݹ鷽�̣��õ� nlogn�ס�����������֤���������˲�̫���ף���Ҫ�ο������������ϡ���
//�ص㣺
//        1��������������ʵ�����������������㷨����ƽ��ʱ�临�Ӷ�nlogn�ף����Ѿ��ӽ������㷨���½졣
//        2����������£��������򲢲��Ȳ�������á�
//        3������Ԫ��

 
//�ϲ�����MergeSort��
//�㷨��������������ƽ����Ϊ���������֣��ֱ�����Ȼ���������ϲ���
//������
//        1���ϲ�������ʱ�临�Ӷ�Ϊnlogn��
//        2���ϲ�����ĸĽ���������ʼʱ����ɨ��һ�����ݣ������򲿷���Ϊһ��������д�������������������˳�����ݣ������˺ϲ����ۡ�
//        3���ռ���۴�
//        4���ʺ��ڲ��м���

//������HeapSort��
//�㷨�������ݿ���ƽ������������Ҹ����Ҫ�����������������н�㡣
//------------------------------------------------------------------------------------------------------------------
//�����㷨�Ǻ����ݽṹ������صġ�ÿ���㷨��������ض������ݽṹ������ġ�

//����������Ҫ����������͵����ݲ�����
//1����ѯ��Search ��Maximun ��Minimun ��Successor ��Predecessor
//2���޸ģ�Insert ��Delete
//�ں�����ܵĸ����㷨��������Բ�ͬ�����ݽṹ����˵�����ֲ����ĸ��Ӷȡ�


//��Ҫ˵�����¼������ݽṹ��������ϵ��㷨��

//   1. ������������Binary Search Tree��BST��
//   2. ���������������RBST��
//   3. �������Red��Black Tree��
//   4. 2��3��4��
//   5.
//      Hash

//����������
//��������������򵥵�һ�ֶ�����������Ҫ����һ�����ʣ����������н�㶼С�ڸ���㣬���������н�㶼���ڸ���㡣����������û���ϸ�������ȵ����⣩
//Search�����Ŷ������������������������Ϊ������ȡ����ǣ��ö���������Ȳ�δ���κ���Լ������������Ⱥ�����������½�����Ϊ������ȡ�
//Maximun��������������
//Minimun��������������
//Successor��1�����������������Ϊ����������Сֵ��
//                        2�������������������Ǹ��������ӣ����Ϊ�丸��㣻
//                        3�������������������Ǹ������Һ��ӣ����ظ��������������֪�������ĵ�ǰ�����Ϊ���ӳ���ʱΪֹ�����Ϊ��ǰ���ĸ���㣻�����յ��︸���ָ��Ϊnullʱ����ʾ�ѵ������㣬���̽�㲻���ڡ�
//Predecessor����Successor�����������෴

//Insert ������search�еķ�ʽ����Ҫ����Ľ�������Ӧ��Ҷ�ӽ��Ϊֹ����search�ĸ��Ӷ���ͬ
//Delete ��1��Ҷ��㣬ֱ��ɾ����
//                 2������һ��������ɾ����ֱ�ӹ������ӽ�㣻
//                 3����������������ɾ���󣬴������������ҵ����̽�㣬Ȼ���滻��ԭ��㴦��
//                 ��Ϊ���Ӷ���ȫȡ��������������������search�ĸ��Ӷ���ͬ��

//����������������������ڣ��临�Ӷ�ȡ�������ĸ߶ȣ�����������Insert����ʱ��������ڸ�����ѡ�񲻺ã������ǲ��ɿ��Ƶġ�Ҳ���ǣ��������߶ȵ�����ԣ����ܱ�֤�������Ӷȵ��ȶ��ԣ���������������¡�

//�������������
//֤���˶�������������������h��O��logn��
//֤������



//Ϊ���ö���������ܹ�����������ֵ����������ϣ���ܹ��ڲ�����ʱ���Զ������ṹ���п��ơ�Ҳ���ǣ�ά������������ֵ������
//�ݴˣ������ƽ��������ĸ���

//ƽ����������������

//�ú�ڱ�־����Ϊ���ε��������ݡ�
//���⣺��ڱ�־�ı仯ԭ��
 
 
//2��3��4��
//�Ƕ�ƽ�����������չ�����������ڶ�������ƣ����������档���ǣ�����Ҫ��Ŀ���������е�Ҷ�ӽ�㶼��ͬһ��ȡ�
//��ѯ��������ͨ�������������ƣ��ؼ���Insert��Delete������
//Insert ���ڲ����½�������н���Ϊ4����㣬��4�����ת��Ϊ3��2�������ʽ������һ��2����㣬���븸����У������¼���Ƿ�������µ�4����㡣�Ĺ��̣��ȴӸ���Ҷ����Ҫ�����λ�ã�Ȼ����в���ʱ�ٴ�Ҷ�������δ�����ṹת����
//Delete�����ȣ��Ӹ���Ҷ����Ҫɾ����Ԫ�ؽ��λ�ã�Ȼ���ٴ�Ҷ�������δ�����ṹת����1����2�������ɾ����������������
 
//Hash ����
/**************************************************************************************
 * ����:���Ը��������㷨��Ч��
 * 
 * ����:��繤����
 * ����:2007-7-15
**************************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Security.Cryptography;
namespace LMB.Sort
{
    /// <summary>
    /// ���Ը��������㷨
    /// </summary>
    class SortTest
    {

        [System.Runtime.InteropServices.DllImport("Kernel32.dll")]
        static extern bool QueryPerformanceCounter(ref long count);

        [System.Runtime.InteropServices.DllImport("Kernel32.dll")]
        static extern bool QueryPerformanceFrequency(ref long count);

        //�������ݸ���
        const int MAXSIZE = 1024 * 100;

        //���������������
#region ���������������

        /**//// <summary>
        /// ���������������
        /// </summary>
        /// <param name="list"></param>
        static void RandomArray(ref int[] list)
        {
            for (int i = 0; i < MAXSIZE; i++)
            {
                //ʹ�ü򵥵������������
                int randomNum = new Random().Next(1, 1024 * 1024);

                //ʹ�ü��ܷ������������
                byte[] randomNumber = new byte[1];
                RNGCryptoServiceProvider Gen = new RNGCryptoServiceProvider();
                Gen.GetBytes(randomNumber);

                //�ϳ�������Ϊ�����
                list[i] = Convert.ToInt32(randomNumber[0]) % randomNum + randomNum;
            }
        }

        #endregion

        //ð������
#region ð������
        
        /**//// <summary>
        /// ð������
        /// </summary>
        /// <param name="list"></param>
        static void DoubleSorter(int[] list)
        {
            int i, j, temp;
            bool done = false;
            j = 1;
            while ((j < list.Length) && (!done))
            {
                done = true;
                for (i = 0; i < list.Length - j; i++)
                {
                    if (list[i] > list[i + 1])
                    {
                        done = false;
                        temp = list[i];
                        list[i] = list[i + 1];
                        list[i + 1] = temp;
                    }
                }
                j++;
            }
        
        }
        #endregion

        //ѡ������
#region ѡ������

        /**//// <summary>
        /// ѡ������
        /// </summary>
        /// <param name="list"></param>
        static void SelectionSorter(int[] list)
        {
            int min;
            for (int i = 0; i < list.Length - 1; i++)
            {
                min = i;
                for (int j = i + 1; j < list.Length; j++)
                {
                    if (list[j] < list[min])
                        min = j;
                }
                int t = list[min];
                list[min] = list[i];
                list[i] = t;
            }
        }

        #endregion

        //��������
#region ��������

        /**//// <summary>
        /// ��������
        /// </summary>
        /// <param name="list"></param>
        static void InsertionSorter(int[] list)
        {
            for (int i = 1; i < list.Length; i++)
            {
                int t = list[i];
                int j = i;
                while ((j > 0) && (list[j - 1] > t))
                {
                    list[j] = list[j - 1];
                    j--;
                }
                list[j] = t;
            }
        }

        #endregion

       // ϣ������
#region ϣ������

        /**//// <summary>
        /// ϣ������
        /// </summary>
        /// <param name="list"></param>
        static void ShellSorter(int[] list)
        {
            int inc;
            for (inc = 1; inc <= list.Length / 9; inc = 3 * inc + 1) ;
            for (; inc > 0; inc /= 3)
            {
                for (int i = inc + 1; i <= list.Length; i += inc)
                {
                    int t = list[i - 1];
                    int j = i;
                    while ((j > inc) && (list[j - inc - 1] > t))
                    {
                        list[j - 1] = list[j - inc - 1];
                        j -= inc;
                    }
                    list[j - 1] = t;
                }
            }
        }

        #endregion

        //��������

#region ��������
  //      ���˼��:
  // ���������㷨�Ļ���˼�뱾����Ƿ��η���ͨ���ָ���������зֳ������֣�
  //����ǰһ���ֵ�Ԫ��ֵ��С�ں�һ���ֵ�Ԫ��ֵ��Ȼ��ÿһ�����ٸ��Եݹ������
  //�����̣�ֱ��ÿһ���ֵĳ���Ϊ1Ϊֹ��
  // ���ȣ������еĵ�һ�����м�һ�������һ��Ԫ����ѡȡ�����Ϊp[middle],
  //����temp = p[middle](��������);
  // ��Σ��������еĵ�һ��Ԫ���Ƶ�p[middle]��λ���ϣ�
  // Ȼ��������ָ��i,j�ֱ�ָ���������еĵ�һ��Ԫ�غ����һ��Ԫ�أ�
  // �ظ�����������ֱ��i = jΪֹ��
  // ��󣬽�array[i] = temp(��tmep�Ƶ�array[i])��
        /**//// <summary>
        /// ��������������(����������)
        /// </summary>
        /// <param name="l"></param>
        /// <param name="r"></param>
        static void Swap(ref int left, ref int right)
        {
            int tmp;
            tmp = left;
            left = right;
            right = tmp;
        }

        /**//// <summary>
        /// ����������������
        /// </summary>
        /// <param name="list"></param>
        /// <param name="low"></param>
        /// <param name="high"></param>
        static void Sort(int[] list, int low, int high)
        {
            if (high <= low)
            {
                return;
            }
            else if (high == low + 1)
            {
                if (list[low] > list[high])
                {
                    Swap(ref list[low], ref list[high]);
                    return;
                }
            }
            QuickSorter(list, low, high);
        }

        /**//// <summary>
        /// ��������������(�ָ�����)
        /// </summary>
        /// <param name="list"></param>
        /// <param name="low"></param>
        /// <param name="high"></param>
        /// <returns></returns>
        static int Partition(int[] list, int low, int high)
        {
            int pivot;
            pivot = list[low];
            while (low < high)
            {
                while (low < high && list[high] >= pivot)
                {
                    high--;
                }
                if (low != high)
                {
                    Swap(ref list[low], ref list[high]);
                    low++;
                }
                while (low < high && list[low] <= pivot)
                {
                    low++;
                }
                if (low != high)
                {
                    Swap(ref list[low], ref list[high]);
                    high--;
                }
            }
            return low;
        }

        /**//// <summary>
        /// ��������
        /// </summary>
        /// <param name="list"></param>
        /// <param name="low"></param>
        /// <param name="high"></param>
        static void QuickSorter(int[] list, int low, int high)
        {
            if (low < high)
            {
                int pivot = Partition(list, low, high);
                QuickSorter(list, low, pivot - 1);
                QuickSorter(list, pivot + 1, high);
            }
        }

        #endregion

        /// <summary>
        /// ���Գ���
        /// </summary>
        /// <param name="args"></param>
//        static void Main(string[] args)
//        {
//            long count = 0;
//            long count1 = 0;
//            long freq = 0;
//            double result = 0;

//            int[] list = new int[MAXSIZE];
//            RandomArray(ref list);


//            //ð���������
//#region ð���������
//            int[] listDouble = new int[MAXSIZE];
//            list.CopyTo(listDouble, 0);
//            QueryPerformanceFrequency(ref freq);
//            QueryPerformanceCounter(ref count);
//            DoubleSorter(listDouble);
//            QueryPerformanceCounter(ref count1);
//            count = count1 - count;
//            result = (double)(count) / (double)freq;
//            Console.WriteLine("ð�������õ�ʱ���ǣ�{0} ��", result);
//            #endregion

//            //ѡ���������
//#region ѡ���������
//            int[] listSelection = new int[MAXSIZE];
//            list.CopyTo(listSelection, 0);
//            QueryPerformanceFrequency(ref freq);
//            QueryPerformanceCounter(ref count);
//            SelectionSorter(listSelection);
//            QueryPerformanceCounter(ref count1);
//            count = count1 - count;
//            result = (double)(count) / (double)freq;
//            Console.WriteLine("ѡ�������õ�ʱ���ǣ�{0} ��", result);
//            #endregion

//            //�����������
//#region �����������
//            int[] listInsertion = new int[MAXSIZE];
//            list.CopyTo(listInsertion, 0);
//            QueryPerformanceFrequency(ref freq);
//            QueryPerformanceCounter(ref count);
//            InsertionSorter(listInsertion);
//            QueryPerformanceCounter(ref count1);
//            count = count1 - count;
//            result = (double)(count) / (double)freq;
//            Console.WriteLine("���������õ�ʱ���ǣ�{0} ��", result);
//            #endregion

//            //ϣ���������
//#region ϣ���������
//            int[] listShell = new int[MAXSIZE];
//            list.CopyTo(listShell, 0);
//            QueryPerformanceFrequency(ref freq);
//            QueryPerformanceCounter(ref count);
//            ShellSorter(listShell);
//            QueryPerformanceCounter(ref count1);
//            count = count1 - count;
//            result = (double)(count) / (double)freq;
//            Console.WriteLine("ϣ�������õ�ʱ���ǣ�{0} ��", result);
//            #endregion

//            //�����������
//            #region �����������
//            int[] listQuick = new int[MAXSIZE];
//            list.CopyTo(listQuick, 0);
//            QueryPerformanceFrequency(ref freq);
//            QueryPerformanceCounter(ref count);
//            QuickSorter(listQuick, 0, MAXSIZE - 1);
//            QueryPerformanceCounter(ref count1);
//            count = count1 - count;
//            result = (double)(count) / (double)freq;
//            Console.WriteLine("���������õ�ʱ���ǣ�{0} ��", result);
//            #endregion

//            Console.ReadLine();

//        }
    }
}