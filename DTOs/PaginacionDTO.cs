namespace tickets.DTOs
{
    public class PaginacionDTO
    {
        public int Pagina { get; set; } = 1;
        private int recordsPorPagina { get; set; } = 4;

        private readonly int cantMax = 10;

        public int RecordsPorPagina
        {
            get
            {
                return recordsPorPagina;
            }
            set
            {
                recordsPorPagina = (value > cantMax) ? cantMax : value;   
            }
        }

    }
}
