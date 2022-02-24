using System.Threading.Tasks;

namespace Shared
{
    public interface IMessageHandler<TMessage>
    {
        public Task Invoke(TMessage message);
    }
}
