namespace Graphon.Core
{
    public class BoundsContext<Tx, Ty>
        where Tx : struct
        where Ty : struct
    {
        public Tx XMin { get; set; }
        
        public Tx XMax { get; set; }
        
        public Ty YMin { get; set; }
        
        public Ty YMax { get; set; }
    }
}