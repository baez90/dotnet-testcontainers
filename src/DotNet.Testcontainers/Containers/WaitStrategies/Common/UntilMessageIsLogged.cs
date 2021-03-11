namespace DotNet.Testcontainers.Containers.WaitStrategies.Common
{
  using System;
  using System.IO;
  using System.Text;
  using System.Text.RegularExpressions;
  using System.Threading.Tasks;
  using DotNet.Testcontainers.Containers.Configurations;

  internal class UntilMessageIsLogged : IWaitUntil
  {
    private readonly string message;

    private readonly Stream stream;

    public UntilMessageIsLogged(Stream stream, string message)
    {
      this.stream = stream;
      this.message = message;
    }

    public async Task<bool> Until(IDockerClientAuthenticationConfiguration clientAuthConfig, string id)
    {
      this.stream.Seek(0, SeekOrigin.Begin);

      using (var streamReader = new StreamReader(this.stream, Encoding.UTF8, false, 4096, true))
      {
        var output= await streamReader.ReadToEndAsync()
          .ConfigureAwait(false);
        return Regex.IsMatch(output, this.message);
      }
    }
  }
}
