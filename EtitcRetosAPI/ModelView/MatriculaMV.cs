namespace EtitcRetosAPI.ModelView
{
    public class MatriculaMV
    {
        public int IdMatricula { get; set; }
        public string? Codigo { get; set; }
        public string? Estado { get; set; }
        public DateTime? ActivaDesde { get; set; }
        public DateTime? ActivaHasta { get; set; }
    }
}
