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

namespace LumoTrack.App.Android.Adapters
{
    public class TruckSpinner : ArrayAdapter
    {
        Context context;
        string[] array;
        List<DTO.VehicleDTO> elements;

        public TruckSpinner(Context context, int resourceId, string[] array, List<DTO.VehicleDTO> elements) : base(context, resourceId, array)
        {
            this.context = context;
            this.array = array;
            this.elements = elements;
        }

        public override Java.Lang.Object GetItem(int position) => array[position];
        public override int Count => array.Length;
        
        internal int GetIdByValue(string textValue)
        {
            var element = elements.FirstOrDefault(x => x.TruckName == textValue);
            var value = element != null ? element.Id : 0;
            return value;
        }
    }
}