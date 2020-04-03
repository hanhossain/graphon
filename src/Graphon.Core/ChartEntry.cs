namespace Graphon.Core
{
	public class ChartEntry<Tx, Ty>
		where Tx : struct
		where Ty : struct
	{
		public Tx X { get; set; }

		public Ty Y { get; set; }
    }
}
