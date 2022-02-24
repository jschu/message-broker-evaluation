using System.Threading.Tasks;

namespace Shared
{
    public interface IMessageHandler<TMessage>
    {
        public void Invoke(TMessage message);
    }
}
