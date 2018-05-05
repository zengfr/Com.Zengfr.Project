// 排序（Sorting ）是最典型的非数值计算问题。

//学习重点：
//1、几种排序算法的排序方法和原理；
//2、在了解原理的基础上，分析算法的时间复杂度和空间复杂度，并对其给予推算和证明；
//3、理解，算法是如何一步一步不断改进的，改进算法时采用的思考策略和方法。

//内容要点：
//基本概念
//全序集：保证数据集合内各数据项之间可比较大小；
//稳定排序（Stable）：相等的数据项在排序后，其原有的先后关系保持不变；
//内部排序（Internal）：数据存储在内存中，数据的比较和移动是代价相等的操作；
//外部排序（External）：数据存储在硬盘（低速设备）中，数据的移动代价明显高于比较操作；
//原址排序（）：排序过程中所需要的额外存储空间与待排序样本n的大小无关。

//1、较慢的排序方法 O(n2) 阶 ： 选择排序 、插入排序 、希尔排序
//2、较快的排序方法 O(nlogn) 阶 ： 快速排序 、合并排序 、 堆排序

//下面根据由慢到快，一次分析其不断演化的过程：
//选择排序(selection sorting)
//算法：把待排序排序数据看成两部分，L【1......i-1】为已排序部分，L【i......n】为待排序部分。每次从待排序部分中求最小元，然后与该部分首元素交换位置。
//分析：
//    1、该算法为原址排序，其只有在交换元素时需要额外的存储空间，是否为稳定排序要看具体算法而定；
//    2、比较操作的次数：
//          因为每次在待排序部分搜索最小元，在其中需要逐个比较，所以总的比较次数是固定的，与样本大小n无关，并且与样本的顺序无关。
//          比较次数＝(n-1) + (n-2) + ...... +2 + 1 = n(n-1)/2
//          时间复杂度： W(n) = A(n) = n(n-1)/2 =  n2 阶

//插入排序（Insertion Sorting ）
//算法：把数据看成有序部分L【1... i】与无序部分L【i+1 ...  n】，两部分。每次从无序部分任取一个元素，插入到有序部分的正确位置中。
//分析：
//        1、该算法为原址排序，稳定排序；
//        2、比较操作的次数：
//             比较次数与样本顺序有关。当样本顺序时，比较次数最少，为(n-1)；当样本逆序时，比较次数最多，为1+2+3+...+(n-1) = n(n-1)/2  。
//             平均比较情况：L[i]插入到L【1... i-1】时，假设插入每个位置的概率是相等的。因为i-1个元素，其包含i个可能插入的位置，所以每个位置的插入的概论为1/i 。此时，比较次数的期望值为：1/i*1 + 1/i*2+...+1/i*(i-2)+1/i*(i-1)+1/i*(i-1) = (i+1)/2 - 1/i    ,  注意：最前两个位置的比较次数是一样的。
//             A(n) 近似等于 n2/4 



//起泡排序（Bubble Sorting）
//就是一种选择排序，它是原址排序、稳定排序。


//希尔排序（Shell Sorting）
//算法：是根据减少逆序对的理论发展来的。插入排序和起泡排序，每次只能减少一个逆序对，而希尔排序可以较少一个以上的逆序对。从理论上说，希尔排序要优于插入排序。
//           把序列划分为h个间距为h的子序列。对每个子序列进行排序，子序列的排序算法可以任选。然后，逐步减小h值，直到h＝1为止。

//分析：希尔排序虽然时间复杂度没有明确的计算，但一般认为它是n2阶的，而且它要由于插入排序。

//快速排序（QuickSort）
//算法：核心是划分（Partition），通过选取任意元素（例如：首元素）作为划分元素，然后将较小元素放到划分元左边，较大元素放到划分元右边，将原序列划分成立左右两部分。换句话说，就是找到的划分元素的位置。然后，将左右两部分再按此方法递归进行下去。
//分析：
//        1、该算法不是原址排序，由于引入了递归；该算法不是稳定排序；
//        2、 比较次数：
//               最坏情况：当序列为有序时（顺序或逆序），那么划分元素总是处在序列的一端，没有起到划分为两部分的左右。比较次数＝(n-1)+(n-2)+...+1=n(n-1)/2           n2阶的
//               平均情况：可以通过求解递归方程，得到 nlogn阶。（具体求解和证明方法，嗨不太明白，需要参考其它具体资料。）
//特点：
//        1、快速排序是在实际运用中最快的排序算法，其平均时间复杂度nlogn阶，并已经接近排序算法的下届。
//        2、在最坏情形下，快速排序并不比插入排序好。
//        3、划分元的

 
//合并排序（MergeSort）
//算法：将待排序数据平均分为左右两部分，分别排序，然后将排序结果合并。
//分析：
//        1、合并排序的最坏时间复杂度为nlogn阶
//        2、合并排序的改进，在排序开始时，先扫描一次数据，将有序部分作为一个分组进行处理。这样，利用了已有顺序数据，减少了合并代价。
//        3、空间代价大
//        4、适合于并行计算

//堆排序（HeapSort）
//算法：把数据看成平衡二叉树，并且根结点要大于左右子树的所有结点。
//------------------------------------------------------------------------------------------------------------------
//搜索算法是和数据结构密切相关的。每种算法都是针对特定的数据结构提出来的。

//搜索问题主要设计两种类型的数据操作：
//1、查询：Search 、Maximun 、Minimun 、Successor 、Predecessor
//2、修改：Insert 、Delete
//在后面介绍的各种算法，都是针对不同的数据结构，来说明各种操作的复杂度。


//主要说明以下几种数据结构及其基础上的算法：

//   1. 二叉搜索树（Binary Search Tree，BST）
//   2. 随机二叉搜索树（RBST）
//   3. 红黑树（Red－Black Tree）
//   4. 2－3－4树
//   5.
//      Hash

//二叉搜索树
//二叉搜索树是最简单的一种二叉树。他主要满足一条性质：左子树所有结点都小于根结点，右子树所有结点都大于根结点。（我在这里没有严格区分相等的问题）
//Search：沿着二叉树搜索，最坏情形搜索长度为树的深度。但是，该二叉树的深度并未受任何制约，所有树的深度很随机。最坏情况下结点个数为树的深度。
//Maximun：沿右子树遍历
//Minimun：沿左子树遍历
//Successor：1）包含右子树，后继为右子树的最小值；
//                        2）不包含右子树，但是父结点的左孩子，后继为其父结点；
//                        3）不包含右子树，但是父结点的右孩子，则沿父结点向上搜索，知道搜索的当前结点作为左孩子出现时为止，后继为当前结点的父结点；若最终到达父结点指向为null时，表示已到达根结点，则后继结点不存在。
//Predecessor：与Successor的搜索正好相反

//Insert ：仿照search中的方式，将要插入的结点放入相应的叶子结点为止。与search的复杂度相同
//Delete ：1）叶结点，直接删除；
//                 2）包含一个子树，删除后，直接关联父子结点；
//                 3）包含两个子树，删除后，从其右子树中找到其后继结点，然后替换到原结点处。
//                 因为复杂度完全取决于搜索操作，所以与search的复杂度相同。

//二叉搜索树的最大问题在于：其复杂度取决于树的高度，但是由于在Insert插入时，如果对于根结点的选择不好，树高是不可控制的。也就是，二叉树高度的随机性，不能保证操作复杂度的稳定性，尤其是在最坏情形下。

//随机二叉搜索树
//证明了二叉搜数树的期望树高h＝O（logn）
//证明过程



//为了让二叉树深度能够保持在期望值附近，所以希望能够在插入结点时，对二叉树结构进行控制。也就是，维持树高在期望值附近。
//据此，提出了平衡二叉树的概念

//平衡二叉树——红黑树

//用红黑标志，作为树形调整的依据。
//问题：红黑标志的变化原理
 
 
//2－3－4树
//是对平衡二叉树的扩展，它不拘泥于二叉的限制，出现了三叉。但是，其主要的目标是让所有的叶子结点都在同一深度。
//查询操作与普通二叉搜索树形似，关键是Insert和Delete操作。
//Insert ：在插入新结点后，如果有结点变为4－结点，则将4－结点转换为3个2－结点形式，其中一个2－结点，放入父结点中，再重新检测是否出现了新的4－结点。改过程，先从根到叶搜索要插入的位置，然后进行插入时再从叶到根依次处理结点结构转换。
//Delete：首先，从根到叶搜索要删除的元素结点位置；然后再从叶到根依次处理结点结构转换。1）在2－结点上删除，。。。。。。
 
//Hash 技术
/**************************************************************************************
 * 功能:测试各种排序算法的效率
 * 
 * 作者:清风工作室
 * 日期:2007-7-15
**************************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Security.Cryptography;
namespace LMB.Sort
{
    /// <summary>
    /// 测试各种排序算法
    /// </summary>
    class SortTest
    {

        [System.Runtime.InteropServices.DllImport("Kernel32.dll")]
        static extern bool QueryPerformanceCounter(ref long count);

        [System.Runtime.InteropServices.DllImport("Kernel32.dll")]
        static extern bool QueryPerformanceFrequency(ref long count);

        //测试数据个数
        const int MAXSIZE = 1024 * 100;

        //生成随机测试数组
#region 生成随机测试数组

        /**//// <summary>
        /// 生成随机测试数组
        /// </summary>
        /// <param name="list"></param>
        static void RandomArray(ref int[] list)
        {
            for (int i = 0; i < MAXSIZE; i++)
            {
                //使用简单的随机数生成器
                int randomNum = new Random().Next(1, 1024 * 1024);

                //使用加密服务生成随机数
                byte[] randomNumber = new byte[1];
                RNGCryptoServiceProvider Gen = new RNGCryptoServiceProvider();
                Gen.GetBytes(randomNumber);

                //合成两者作为随机数
                list[i] = Convert.ToInt32(randomNumber[0]) % randomNum + randomNum;
            }
        }

        #endregion

        //冒泡排序
#region 冒泡排序
        
        /**//// <summary>
        /// 冒泡排序
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

        //选择排序
#region 选择排序

        /**//// <summary>
        /// 选择排序
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

        //插入排序
#region 插入排序

        /**//// <summary>
        /// 插入排序
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

       // 希尔排序
#region 希尔排序

        /**//// <summary>
        /// 希尔排序
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

        //快速排序

#region 快速排序
  //      编程思想:
  // 快速排序算法的基本思想本身就是分治法。通过分割，将无序序列分成两部分，
  //其中前一部分的元素值都小于后一部分的元素值。然后每一部分再各自递归进行上
  //述过程，直到每一部分的长度为1为止。
  // 首先，在序列的第一个，中间一个，最后一个元素中选取中项，设为p[middle],
  //并作temp = p[middle](保存中项);
  // 其次，将序列中的第一个元素移到p[middle]的位置上；
  // 然后，设两个指针i,j分别指向将排序序列的第一个元素和最后一个元素；
  // 重复以上两步，直到i = j为止；
  // 最后，将array[i] = temp(将tmep移到array[i])。
        /**//// <summary>
        /// 快速排序辅助函数(两数互交换)
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
        /// 快速排序辅助排序函数
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
        /// 快速排序辅助函数(分割数组)
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
        /// 快速排序
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
        /// 测试程序
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


//            //冒泡排序测试
//#region 冒泡排序测试
//            int[] listDouble = new int[MAXSIZE];
//            list.CopyTo(listDouble, 0);
//            QueryPerformanceFrequency(ref freq);
//            QueryPerformanceCounter(ref count);
//            DoubleSorter(listDouble);
//            QueryPerformanceCounter(ref count1);
//            count = count1 - count;
//            result = (double)(count) / (double)freq;
//            Console.WriteLine("冒泡排序用的时间是：{0} 秒", result);
//            #endregion

//            //选择排序测试
//#region 选择排序测试
//            int[] listSelection = new int[MAXSIZE];
//            list.CopyTo(listSelection, 0);
//            QueryPerformanceFrequency(ref freq);
//            QueryPerformanceCounter(ref count);
//            SelectionSorter(listSelection);
//            QueryPerformanceCounter(ref count1);
//            count = count1 - count;
//            result = (double)(count) / (double)freq;
//            Console.WriteLine("选择排序用的时间是：{0} 秒", result);
//            #endregion

//            //插入排序测试
//#region 插入排序测试
//            int[] listInsertion = new int[MAXSIZE];
//            list.CopyTo(listInsertion, 0);
//            QueryPerformanceFrequency(ref freq);
//            QueryPerformanceCounter(ref count);
//            InsertionSorter(listInsertion);
//            QueryPerformanceCounter(ref count1);
//            count = count1 - count;
//            result = (double)(count) / (double)freq;
//            Console.WriteLine("插入排序用的时间是：{0} 秒", result);
//            #endregion

//            //希尔排序测试
//#region 希尔排序测试
//            int[] listShell = new int[MAXSIZE];
//            list.CopyTo(listShell, 0);
//            QueryPerformanceFrequency(ref freq);
//            QueryPerformanceCounter(ref count);
//            ShellSorter(listShell);
//            QueryPerformanceCounter(ref count1);
//            count = count1 - count;
//            result = (double)(count) / (double)freq;
//            Console.WriteLine("希尔排序用的时间是：{0} 秒", result);
//            #endregion

//            //快速排序测试
//            #region 快速排序测试
//            int[] listQuick = new int[MAXSIZE];
//            list.CopyTo(listQuick, 0);
//            QueryPerformanceFrequency(ref freq);
//            QueryPerformanceCounter(ref count);
//            QuickSorter(listQuick, 0, MAXSIZE - 1);
//            QueryPerformanceCounter(ref count1);
//            count = count1 - count;
//            result = (double)(count) / (double)freq;
//            Console.WriteLine("快速排序用的时间是：{0} 秒", result);
//            #endregion

//            Console.ReadLine();

//        }
    }
}