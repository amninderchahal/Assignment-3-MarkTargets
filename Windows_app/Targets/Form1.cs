using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Targets
{
    public partial class Form1 : Form
    {
        GMapOverlay targetsOverlay = new GMapOverlay();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var request = WebRequest.CreateHttp("https://android-lab-8.firebaseio.com/.json");
            request.Method = "GET";
            request.ContentType = "application/json";

            var response = request.GetResponse();

            var json = (new StreamReader(response.GetResponseStream())).ReadToEnd();
            
            var target = JObject.Parse(json);
            
            Dictionary<string, string> data = target["Targets"].ToObject<Dictionary<string, string>>();

            GMapOverlay markers = new GMap.NET.WindowsForms.GMapOverlay("markers");

            foreach (var kv in data)
            {
                Double latdecimal = Convert.ToDouble(kv.Value.Split('|')[0]);
                Double londecimal = Convert.ToDouble(kv.Value.Split('|')[1]);
                GMarkerGoogle marker = new GMarkerGoogle(new PointLatLng(latdecimal, londecimal), GMarkerGoogleType.green);
                markers.Markers.Add(marker);
            }
            gMapControl.Overlays.Add(markers);
            gMapControl.Update();
        }

        private void gMapControl_Load(object sender, EventArgs e)
        {
            gMapControl.MapProvider = GMap.NET.MapProviders.BingMapProvider.Instance;
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerOnly;
            gMapControl.SetPositionByKeywords("Kitchener, Canada");
            gMapControl.ShowCenter = false;
        }
    }
}
