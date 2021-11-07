using System.Threading;
using System.Threading.Tasks;

namespace Worker.Services
{
  public interface IFirstService
  {
    Task ExecuteAsync(CancellationToken cancellationToken);
  }
}