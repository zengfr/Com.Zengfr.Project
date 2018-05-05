using System;
using System.Data;
using System.Text;
using System.IO;
using System.Xml;
namespace Com.Zfrong.Common.IO
{
    /// <summary>
    /// FObject : �ļ�ϵͳ�Ĵ�����
    /// </summary>
    public abstract class FileOperate
    {

        /// <summary>
        /// �ļ�ϵͳ�Ĵ������
        /// </summary>
        public enum FsoMethod
        {
            /// <summary>
            /// �����ڴ����ļ���
            /// </summary>
            Folder = 0,
            /// <summary>
            /// �����ڴ����ļ�
            /// </summary>
            File,
            /// <summary>
            /// �ļ����ļ��ж����봦��
            /// </summary>
            All
        }

        # region "�ļ��Ķ�д����"

        /// <summary>
        /// ���ļ�������ʽ��ȡָ���ļ�������
        /// </summary>
        /// <param name="file">ָ�����ļ�����ȫ·��</param>
        /// <returns>���� String</returns>
        public static string ReadFile(string file)
        {
            string strResult = "";

            FileStream fStream = new FileStream(file, FileMode.Open, FileAccess.Read);
            StreamReader sReader = new StreamReader(fStream, Encoding.UTF8);

            try
            {
                strResult = sReader.ReadToEnd();
            }
            catch { }
            finally
            {
                fStream.Flush();
                fStream.Close();
                sReader.Close();
            }

            return strResult;
        }

        /// <summary>
        /// ���ļ�������ʽ������д�뵽ָ���ļ��У�������ļ����ļ��в������򴴽���
        /// </summary>
        /// <param name="file">�ļ�����ָ��·��</param>
        /// <param name="fileContent">�ļ�����</param>
        /// <returns>���ز���ֵ</returns>
        public static string WriteFile(string file, string fileContent)
        {
            FileInfo f = new FileInfo(file);
            // ����ļ����ڵ��ļ��в������򴴽��ļ���
            if (!Directory.Exists(f.DirectoryName)) Directory.CreateDirectory(f.DirectoryName);

            FileStream fStream = new FileStream(file, FileMode.Create, FileAccess.Write);
            StreamWriter sWriter = new StreamWriter(fStream, Encoding.UTF8);

            try
            {
                sWriter.Write(fileContent);
                return fileContent;
            }
            catch (Exception exc)
            {
                throw new Exception(exc.ToString());
            }
            finally
            {
                sWriter.Flush();
                fStream.Flush();
                sWriter.Close();
                fStream.Close();
            }
        }

        /// <summary>
        /// ���ļ�������ʽ������д�뵽ָ���ļ��У�������ļ����ļ��в������򴴽���
        /// </summary>
        /// <param name="file">�ļ�����ָ��·��</param>
        /// <param name="fileContent">�ļ�����</param>
        /// <param name="Append">�Ƿ�׷��ָ�����ݵ����ļ���</param>
        /// <returns>���ز���ֵ</returns>
        public static void WriteFile(string file, string fileContent, bool Append)
        {
            FileInfo f = new FileInfo(file);
            // ����ļ����ڵ��ļ��в������򴴽��ļ���
            if (!Directory.Exists(f.DirectoryName)) Directory.CreateDirectory(f.DirectoryName);

            StreamWriter sWriter = new StreamWriter(file, Append, Encoding.Default);

            try
            {
                sWriter.Write(fileContent);
            }
            catch (Exception exc)
            {
                throw new Exception(exc.ToString());
            }
            finally
            {
                sWriter.Flush();
                sWriter.Close();
            }
        }
        # endregion


        # region "�ļ�����Ϣ�Ķ�ȡ"


        /// <summary>
        /// ��ȡָ��Ŀ¼�µ�����Ŀ¼�����ļ���Ϣ
        /// </summary>
        /// <param name="dir">�ļ���</param>
        /// <param name="method">��ȡ��ʽ��</param>
        /// <returns>DataTable</returns>
        public static DataTable getDirectoryAllInfos(string dir, FsoMethod method)
        {
            DataTable Dt;

            try
            {
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                DirectoryInfo dInfo = new DirectoryInfo(dir);
                Dt = getDirectoryAllInfo(dInfo, method);
            }
            catch (Exception exc)
            {
                throw new Exception(exc.ToString());
            }

            return Dt;
        }

        /// <summary>
        /// ��ȡָ��Ŀ¼�µ��ļ���Ϣ
        /// </summary>
        /// <param name="d">ʵ������Ŀ¼</param>
        /// <param name="method">��ȡ��ʽ������ȡ�ļ��ṹ </param>
        /// <returns></returns>
        public static DataTable searchDirectoryAllInfo(String dir, String search)
        {
            DirectoryInfo d = new DirectoryInfo(dir);
            DataTable Dt = new DataTable();
            DataRow Dr;

            Dt.Columns.Add("name");			//����
            Dt.Columns.Add("rname");		//����
            Dt.Columns.Add("content_type");	//�ļ�MIME���ͣ�������ļ������ÿ�
            Dt.Columns.Add("type");			//���ͣ�1Ϊ�ļ��У�2Ϊ�ļ�
            Dt.Columns.Add("path");			//�ļ�·��
            Dt.Columns.Add("creatime");		//����ʱ��
            Dt.Columns.Add("size");			//�ļ���С


            FileInfo[] files = d.GetFiles(search);
            foreach (FileInfo file in files)
            {
                Dr = Dt.NewRow();

                Dr[0] = file.Name;
                Dr[1] = file.FullName;
                Dr[2] = file.Extension.Replace(".", "");
                Dr[3] = 2;
                Dr[4] = file.DirectoryName + "\\";
                Dr[5] = file.CreationTime;
                Dr[6] = file.Length;

                Dt.Rows.Add(Dr);
            }


            return Dt;
        }


        /// <summary>
        /// ��ȡָ��Ŀ¼�µ�����Ŀ¼�����ļ���Ϣ
        /// </summary>
        /// <param name="d">ʵ������Ŀ¼</param>
        /// <param name="method">��ȡ��ʽ��1������ȡ�ļ��нṹ  2������ȡ�ļ��ṹ  3��ͬʱ��ȡ�ļ����ļ�����Ϣ</param>
        /// <returns></returns>
        private static DataTable getDirectoryAllInfo(DirectoryInfo d, FsoMethod method)
        {
            DataTable Dt = new DataTable();
            DataRow Dr;

            Dt.Columns.Add("name");			//����
            Dt.Columns.Add("rname");		//����
            Dt.Columns.Add("content_type");	//�ļ�MIME���ͣ�������ļ������ÿ�
            Dt.Columns.Add("type");			//���ͣ�1Ϊ�ļ��У�2Ϊ�ļ�
            Dt.Columns.Add("path");			//�ļ�·��
            Dt.Columns.Add("creatime");		//����ʱ��
            Dt.Columns.Add("size");			//�ļ���С

            // ��ȡ�ļ��нṹ��Ϣ
            DirectoryInfo[] dirs = d.GetDirectories();
            foreach (DirectoryInfo dir in dirs)
            {
                if (method == FsoMethod.File)
                {
                    Dt = copyDT(Dt, getDirectoryAllInfo(dir, method));
                }
                else
                {
                    Dr = Dt.NewRow();

                    Dr[0] = dir.Name;
                    Dr[1] = dir.FullName;
                    Dr[2] = "";
                    Dr[3] = 1;
                    Dr[4] = dir.FullName.Replace(dir.Name, "");
                    Dr[5] = dir.CreationTime;
                    Dr[6] = "";

                    Dt.Rows.Add(Dr);

                    Dt = copyDT(Dt, getDirectoryAllInfo(dir, method));
                }
            }

            // ��ȡ�ļ��ṹ��Ϣ
            if (method != FsoMethod.Folder)
            {
                FileInfo[] files = d.GetFiles();
                foreach (FileInfo file in files)
                {
                    Dr = Dt.NewRow();

                    Dr[0] = file.Name;
                    Dr[1] = file.FullName;
                    Dr[2] = file.Extension.Replace(".", "");
                    Dr[3] = 2;
                    Dr[4] = file.DirectoryName + "\\";
                    Dr[5] = file.CreationTime;
                    Dr[6] = file.Length;

                    Dt.Rows.Add(Dr);
                }
            }

            return Dt;
        }

        /// <summary>
        /// �������ṹһ���� DataTable ��ϳ�һ�� DataTable
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="child"></param>
        /// <returns>DataTable</returns>
        public static DataTable copyDT(DataTable parent, DataTable child)
        {
            DataRow dr;
            for (int i = 0; i < child.Rows.Count; i++)
            {
                dr = parent.NewRow();
                for (int j = 0; j < parent.Columns.Count; j++)
                {
                    dr[j] = child.Rows[i][j];
                }
                parent.Rows.Add(dr);
            }

            return parent;
        }



        /// <summary>
        /// ��ȡָ���ļ��е���Ϣ���磺�ļ��д�С���ļ��������ļ���
        /// </summary>
        /// <param name="dir">ָ���ļ���·��</param>
        /// <returns>���� String</returns>
        public static long[] getDirInfos(string dir)
        {
            long[] intResult = new long[3];
            DirectoryInfo d = new DirectoryInfo(dir);
            intResult = DirInfo(d);

            return intResult;
        }


        private static long[] DirInfo(DirectoryInfo d)
        {
            long[] intResult = new long[3];

            long Size = 0;
            long Dirs = 0;
            long Files = 0;
            // �����ļ���С
            FileInfo[] files = d.GetFiles();
            Files += files.Length;
            foreach (FileInfo file in files)
            {
                Size += file.Length;
            }
            // �����ļ���
            DirectoryInfo[] dirs = d.GetDirectories();
            Dirs += dirs.Length;
            foreach (DirectoryInfo dir in dirs)
            {
                Size += DirInfo(dir)[0];
                Dirs += DirInfo(dir)[1];
                Files += DirInfo(dir)[2];
            }

            intResult[0] = Size;
            intResult[1] = Dirs;
            intResult[2] = Files;
            return intResult;
        }


        /// <summary>
        /// ��ȡָ��Ŀ¼��Ŀ¼��Ϣ
        /// </summary>
        /// <param name="dir">ָ��Ŀ¼</param>
        /// <param name="method">��ȡ��ʽ��1������ȡ�ļ��нṹ  2������ȡ�ļ��ṹ  3��ͬʱ��ȡ�ļ����ļ�����Ϣ</param>
        /// <returns>���� DataTable</returns>
        public static DataTable getDirectoryInfos(string dir, FsoMethod method)
        {
            DataTable Dt = new DataTable();
            DataRow Dr;

            Dt.Columns.Add("name");//����
            Dt.Columns.Add("type");//���ͣ�1Ϊ�ļ��У�2Ϊ�ļ�
            Dt.Columns.Add("size");//�ļ���С��������ļ������ÿ�
            Dt.Columns.Add("content_type");//�ļ�MIME���ͣ�������ļ������ÿ�
            Dt.Columns.Add("createTime");//����ʱ��
            Dt.Columns.Add("lastWriteTime");//����޸�ʱ��

            // ��ȡ�ļ��нṹ��Ϣ
            if (method != FsoMethod.File)
            {
                for (int i = 0; i < getDirs(dir).Length; i++)
                {
                    Dr = Dt.NewRow();
                    DirectoryInfo d = new DirectoryInfo(getDirs(dir)[i]);

                    Dr[0] = d.Name;
                    Dr[1] = 1;
                    Dr[2] = "";
                    Dr[3] = "";
                    Dr[4] = d.CreationTime;
                    Dr[5] = d.LastWriteTime;

                    Dt.Rows.Add(Dr);
                }
            }

            // ��ȡ�ļ��ṹ��Ϣ
            if (method != FsoMethod.Folder)
            {
                for (int i = 0; i < getFiles(dir).Length; i++)
                {
                    Dr = Dt.NewRow();
                    FileInfo f = new FileInfo(getFiles(dir)[i]);

                    Dr[0] = f.Name;
                    Dr[1] = 2;
                    Dr[2] = f.Length;
                    Dr[3] = f.Extension.Replace(".", "");
                    Dr[4] = f.CreationTime;
                    Dr[5] = f.LastWriteTime;

                    Dt.Rows.Add(Dr);
                }
            }

            return Dt;
        }


        private static string[] getDirs(string dir)
        {
            return Directory.GetDirectories(dir);
        }

        private static string[] getFiles(string dir)
        {
            return Directory.GetFiles(dir);
        }

        # endregion


        # region "�ļ�ϵͳ����Ӧ����"

        /// <summary>
        /// �ж��ļ����ļ����Ƿ����
        /// </summary>
        /// <param name="file">ָ���ļ�����·��</param>
        /// <param name="method">�жϷ�ʽ</param>
        /// <returns>���ز���ֵ</returns>
        public static bool IsExist(string file, FsoMethod method)
        {
            try
            {
                if (method == FsoMethod.File)
                {
                    return File.Exists(file);
                }
                else if (method == FsoMethod.Folder)
                {
                    return Directory.Exists(file);
                }
                else
                {
                    return false;
                }
            }
            catch (Exception exc)
            {
                throw new Exception(exc.ToString());
            }
        }

        # region "�½�"

        /// <summary>
        /// �½��ļ����ļ���
        /// </summary>
        /// <param name="file">�ļ����ļ��м���·��</param>
        /// <param name="method">�½���ʽ</param>
        public static void Create(string file, FsoMethod method)
        {
            try
            {
                if (method == FsoMethod.File)
                {
                    WriteFile(file, "");
                }
                else if (method == FsoMethod.Folder)
                {
                    Directory.CreateDirectory(file);
                }
            }
            catch (Exception exc)
            {
                throw new Exception(exc.ToString());
            }
        }

        # endregion


        # region "����"

        # region "�����ļ�"
        /// <summary>
        /// �����ļ������Ŀ���ļ��Ѿ������򸲸ǵ�
        /// </summary>
        /// <param name="oldFile">Դ�ļ�</param>
        /// <param name="newFile">Ŀ���ļ�</param>
        public static void CopyFile(string oldFile, string newFile)
        {
            try
            {
                File.Copy(oldFile, newFile, true);
            }
            catch (Exception exc)
            {
                throw new Exception(exc.ToString());
            }
        }


        /// <summary>
        /// ��������ʽ���ƿ����ļ�
        /// </summary>
        /// <param name="oldPath">Դ�ļ�</param>
        /// <param name="newPath">Ŀ���ļ�</param>
        /// <returns></returns>
        public static bool CopyFileStream(string oldPath, string newPath)
        {
            try
            {
                //��������FileStream����
                FileStream fsOld = new FileStream(oldPath, FileMode.Open, FileAccess.Read);
                FileStream fsNew = new FileStream(newPath, FileMode.Create, FileAccess.Write);

                //�ֱ���һ����д��
                BinaryReader br = new BinaryReader(fsOld);
                BinaryWriter bw = new BinaryWriter(fsNew);

                //����ȡ�ļ�����ָ��ָ������ͷ��
                br.BaseStream.Seek(0, SeekOrigin.Begin);
                //��д���ļ�����ָ��ָ������β��
                bw.BaseStream.Seek(0, SeekOrigin.End);

                while (br.BaseStream.Position < br.BaseStream.Length)
                {
                    //��br���ж�ȡһ��Byte������д��bw��
                    bw.Write(br.ReadByte());
                }
                //�ͷ����б�ռ�õ���Դ
                br.Close();
                bw.Close();
                fsOld.Flush();
                fsOld.Close();
                fsNew.Flush();
                fsNew.Close();
                return true;
            }
            catch
            {
                return false;
            }

        }

        # endregion

        # region "�����ļ���"

        /// <summary>
        /// �����ļ����е��������ݼ�����Ŀ¼�����ļ�
        /// </summary>
        /// <param name="oldDir">Դ�ļ��м���·��</param>
        /// <param name="newDir">Ŀ���ļ��м���·��</param>
        public static void CopyDirectory(string oldDir, string newDir)
        {
            try
            {
                DirectoryInfo dInfo = new DirectoryInfo(oldDir);
                CopyDirInfo(dInfo, oldDir, newDir);
            }
            catch (Exception exc)
            {
                throw new Exception(exc.ToString());
            }
        }

        private static void CopyDirInfo(DirectoryInfo od, string oldDir, string newDir)
        {
            // Ѱ���ļ���
            if (!IsExist(newDir, FsoMethod.Folder)) Create(newDir, FsoMethod.Folder);
            DirectoryInfo[] dirs = od.GetDirectories();
            foreach (DirectoryInfo dir in dirs)
            {
                CopyDirInfo(dir, dir.FullName, newDir + dir.FullName.Replace(oldDir, ""));
            }
            // Ѱ���ļ�
            FileInfo[] files = od.GetFiles();
            foreach (FileInfo file in files)
            {
                CopyFile(file.FullName, newDir + file.FullName.Replace(oldDir, ""));
            }
        }

        # endregion

        # endregion


        # region "�ƶ�"

        /// <summary>
        /// �ƶ��ļ����ļ���
        /// </summary>
        /// <param name="oldFile">ԭʼ�ļ����ļ���</param>
        /// <param name="newFile">Ŀ���ļ����ļ���</param>
        /// <param name="method">�ƶ���ʽ��1��Ϊ�ƶ��ļ���2��Ϊ�ƶ��ļ���</param>
        public static void Move(string oldFile, string newFile, FsoMethod method)
        {
            try
            {
                if (method == FsoMethod.File)
                {
                    File.Move(oldFile, newFile);
                }
                if (method == FsoMethod.Folder)
                {
                    Directory.Move(oldFile, newFile);
                }
            }
            catch (Exception exc)
            {
                throw new Exception(exc.ToString());
            }
        }

        # endregion


        # region "ɾ��"

        /// <summary>
        /// ɾ���ļ����ļ���
        /// </summary>
        /// <param name="file">�ļ����ļ��м���·��</param>
        /// <param name="method">ɾ����ʽ��1��Ϊɾ���ļ���2��Ϊɾ���ļ���</param>
        public static void Delete(string file, FsoMethod method)
        {
            try
            {
                if (method == FsoMethod.File)
                {
                    File.Delete(file);
                }
                if (method == FsoMethod.Folder)
                {
                    Directory.Delete(file, true);//ɾ����Ŀ¼�µ������ļ��Լ���Ŀ¼
                }
            }
            catch (Exception exc)
            {
                throw new Exception(exc.ToString());
            }
        }

        # endregion

        # endregion

        #region �ļ���������
        /// <summary>
        /// ���������㷨
        /// </summary>
        /// ��������Ϊ���ȶ�����,ʱ�临�Ӷ�O(nlog2n),Ϊͬ���������������򷽷�
        /// <param name="arr">���ֵ�����</param>
        /// <param name="low">����Ͷ��ϱ�</param>
        /// <param name="high">����߶��±�</param>
        /// <returns></returns>
        static int Partition(FileInfo[] arr, int low, int high)
        {
            //����һ�˿�������,�����������¼λ��
            // arr[0] = arr[low];
            FileInfo pivot = arr[low];//������������arr[0]
            while (low < high)
            {
                while (low < high && arr[high].CreationTime <= pivot.CreationTime)
                    --high;
                //�����������¼С���Ƶ��Ͷ�
                Swap(ref arr[high], ref arr[low]);
                while (low < high && arr[low].CreationTime >= pivot.CreationTime)
                    ++low;
                Swap(ref arr[high], ref arr[low]);
                //�����������¼����Ƶ��߶�
            }
            arr[low] = pivot; //�������Ƶ���ȷλ��
            return low;  //����������λ��
        }
        static void Swap(ref FileInfo i, ref FileInfo j)
        {
            FileInfo t;
            t = i;
            i = j;
            j = t;
        }
        /// <summary>
        /// ���������㷨
        /// </summary>
        /// ��������Ϊ���ȶ�����,ʱ�临�Ӷ�O(nlog2n),Ϊͬ���������������򷽷�
        /// <param name="arr">���ֵ�����</param>
        /// <param name="low">����Ͷ��ϱ�</param>
        /// <param name="high">����߶��±�</param>
        public static void QuickSort(FileInfo[] arr, int low, int high)
        {
            if (low <= high - 1)//�� arr[low,high]Ϊ�ջ�ֻһ����¼��������
            {
                int pivot = Partition(arr, low, high);
                QuickSort(arr, low, pivot - 1);
                QuickSort(arr, pivot + 1, high);
            }
        }
        #endregion
    }
}