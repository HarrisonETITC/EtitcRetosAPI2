namespace EtitcRetosAPI.ModelView
{
    public class SolicitudRetoMV
    {
        public int IdSolicitud { get; set; }
        public DateTime? FechaSolicitado { get; set; }
        public string? Reviso { get; set; }
        public string? Solicito { get; set; }
        public string? SolicitoEmpresa { get; set; }
        public string? Tipo { get; set; }
        public string? Estado { get; set; }
        public string? Observacion { get; set; }
    }
}
