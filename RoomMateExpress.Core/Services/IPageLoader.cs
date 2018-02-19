using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RoomMateExpress.Core.Services
{
    public interface IPageLoader<TElement>
    {
        Task<IEnumerable<TElement>> LoadMoreElements(IEnumerable<TElement> elements);
        bool IsLoading { get; }
    }
}
