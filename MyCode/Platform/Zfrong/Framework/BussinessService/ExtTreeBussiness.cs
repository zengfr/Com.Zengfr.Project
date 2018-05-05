using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Zfrong.Framework.Core.IModel;
using Zfrong.Framework.Core.DataContract;
using Zfrong.Framework.Core.IRepository;
using Zfrong.Framework.Core.IBussiness;
namespace Zfrong.Framework.Bussiness
{
    public partial class ExtTreeBussiness4Tree<T> : IExtTreeBussiness<T> where T : class,IExtTreeModel<T>, new()
    {
        public virtual IExtTreeNHibernateRepository<T, long> CurrentRepository { get; set; }
        #region
        public virtual IList<T> NodeFindAll(string parentNode, string checkedValue)
        {
            IList<T> objs = this.CurrentRepository.NodeFindAll(parentNode, checkedValue);
            return objs;
        }
        public virtual ExtResponse<T> NodeCreate(long parentId, string checkedValue, T dataItem)
        {
            T obj = this.CurrentRepository.NodeCreate(parentId, checkedValue, dataItem);
            ExtResponse<T> response = new ExtResponse<T>();
            response.success = true;
            response.message = "TreeNodeCreate";
            response.data = obj;
            return response;
        }
        public virtual ExtResponse<T> NodeRemove(long id)
        {
            this.CurrentRepository.NodeRemove(id);
            ExtResponse<T> response = new ExtResponse<T>();
            response.success = true;
            response.message = "TreeNodeRemove";
            return response;
        }
        public virtual ExtResponse<T> NodeUpdate(long id, T dataItem)
        {
            T obj = this.CurrentRepository.NodeUpdate(id,dataItem);

            ExtResponse<T> response = new ExtResponse<T>();
            response.data = obj;
            response.success = true;
            response.message = "TreeNodeUpdate";
            return response;
        }
        public virtual ExtResponse<T> NodeMove(long id, long parentId)
        {
            T obj = this.CurrentRepository.NodeMove(id, parentId);
            ExtResponse<T> response = new ExtResponse<T>();
            response.data = obj;
            response.success = true;
            response.message = "TreeNodeMove";
            return response;
        }
        #endregion
    }
}
