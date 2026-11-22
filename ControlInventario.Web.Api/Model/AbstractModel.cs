using System.Net;
using System.Runtime.InteropServices;

namespace ControlInventario.Web.Api.Model
{
    public abstract class AbstractModel<T>
    {
        public HttpStatusCode HttpStatusCode
        {
            get;
            set;
        }

        public List<T>? DataList
        {
            get;
            set;
        }

        public T? Data
        {
            get;
            set;
        }
    }
}