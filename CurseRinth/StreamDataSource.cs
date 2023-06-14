using ICSharpCode.SharpZipLib.Zip;

namespace CurseRinth;

public class StreamDataSource : IStaticDataSource
{
	public StreamDataSource(Stream stream)
	{
		stream.Position = 0;
		Stream = stream;
	}
	public Stream Stream { get; }
	public Stream GetSource() => Stream;
}