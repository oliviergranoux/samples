using System.Threading;
using System.Threading.Tasks;

namespace Worker.Services
{
  public interface ISecondService
  {
    Task ExecuteAsync(CancellationToken cancellationToken);
  }
}