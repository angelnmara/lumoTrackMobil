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

namespace LumoTrack.App.Android.Models
{
    public class PartidosColores
    {
        public PartidosColores(string partido, string colorPrimario, string colorSeundario, string colorHint) {
            Partido = partido;
            ColorPrimario = colorPrimario;
            ColorSecundario = colorSeundario;
            ColorHint = colorHint;
        }
        public string Partido { get; set; }
        public string ColorPrimario { get; set; }
        public string ColorSecundario { get; set; }
        public string ColorHint { get; set; }
    }

    public class MunicipioRecursos {
        public MunicipioRecursos(string municipio, string background, string sigueTuCamion, string municipioImg) {
            Municipio = municipio;
            Background = background;
            SigueTuCamion = sigueTuCamion;
            MunicipioImg = municipioImg;
        }
        public string Municipio { get; set; }
        public string Background { get; set; }
        public string SigueTuCamion { get; set; }
        public string MunicipioImg { get; set; }
    }
}