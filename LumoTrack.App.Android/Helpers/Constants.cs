using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using LumoTrack.App.Android.Models;

namespace LumoTrack.App.Android.Helpers
{
    public class Constants
    {
        public static string partidoActual = "PRD";
        public static string municipioActual = "Tultepec";

        public static string Mipmap = "mipmap";

        public static List<PartidosColores> GetListaPartidosColores() {
            List<PartidosColores> listPartidosColores = new List<PartidosColores> {
                new PartidosColores("MORENA", "#ff4d4d", "#ffffff", "#bbff4d4d"),
                new PartidosColores("PRD", "#ffce00", "#000000", "#bbffce00"),
                new PartidosColores("PRI", "#006847", "#CE1126", "#bb006847"),
                new PartidosColores("PAN", "#39ac39", "#ffffff", "#bb39ac39")
            };
            return listPartidosColores;
        }

        public static List<MunicipioRecursos> GetListaMunicipiosRecursos() {
            List<MunicipioRecursos> listMunicipiosRecursos = new List<MunicipioRecursos> {
                new MunicipioRecursos("Tultepec", "bgPRD", "SigueTuCamionPRD", "MunicipioTultepec")
            };
            return listMunicipiosRecursos;
        }
        
        public static string ColorPrimario = GetListaPartidosColores().Where(x => x.Partido == partidoActual).Select(d=>d.ColorPrimario).FirstOrDefault();
        public static string ColorSecundario = GetListaPartidosColores().Where(x => x.Partido == partidoActual).Select(d => d.ColorSecundario).FirstOrDefault();
        public static string ColorHint = GetListaPartidosColores().Where(x => x.Partido == partidoActual).Select(d => d.ColorHint).FirstOrDefault();

        public static string Background = GetListaMunicipiosRecursos().Where(x => x.Municipio == municipioActual).Select(d => d.Background).FirstOrDefault();
        public static string SigueTuCamion = GetListaMunicipiosRecursos().Where(x => x.Municipio == municipioActual).Select(d => d.SigueTuCamion).FirstOrDefault();
        public static string MunicipioImg = GetListaMunicipiosRecursos().Where(x => x.Municipio == municipioActual).Select(d => d.MunicipioImg).FirstOrDefault();

    }
}